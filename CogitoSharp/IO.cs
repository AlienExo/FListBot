﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using DNA.Text;

using CogitoSharp;

namespace CogitoSharp.IO
{
	class SystemCommand{
		internal protected string OpCode { get; set; }
		internal protected Dictionary<string, object> Data = new Dictionary<string, object>();

		internal protected User sourceUser = null;
		internal protected Channel sourceChannel = null;

		/// <summary>
		/// Sends the message by adding it to the OutgoingMessageQueue
		/// </summary>
		internal void Send() { Core.OutgoingMessageQueue.Enqueue(this); }

		/// <summary>
		/// Produces a string that fserv can interpret.
		/// </summary>
		/// <returns>A string "OPCODE {JSONKEY: "value", [...]}"</returns>
		public string ToServerString() {
			if (this.Data != null) { return this.OpCode.ToUpperInvariant() + " " + JsonConvert.SerializeObject(this.Data).ToString(); }
			else { return this.OpCode.ToUpperInvariant(); }
		}

		public SystemCommand(string rawmessage){
			this.OpCode = rawmessage.Substring(0, 3);
			if (rawmessage.Length > 4) { 
				this.Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(HttpUtility.HtmlDecode(rawmessage.Substring(4)));
				try {
					this.sourceChannel = this.Data.ContainsKey("channel") ? Core.getChannel((string)this.Data["channel"]) : null;
					this.sourceUser = this.Data.ContainsKey("character") ? Core.getUser((string)this.Data["character"]) : null;
				}
				catch (Exception) { Core.SystemLog.Log(this.Data.ToString()); }
			}
			else { this.Data = null; }
		}

		public SystemCommand(Message parentMessage){
			this.OpCode = parentMessage.OpCode;
			this.Data = parentMessage.Data;
		}

		public SystemCommand() { }
	} //class SystemCommand

	class Message : SystemCommand{
		//TODO Finish Parsing
		internal new User sourceUser = null;
		internal new Channel sourceChannel = null;

		/// <summary> Maximum length (in bytes) of a channel message; longer and you gotta split it </summary>
		internal static int chat_max = 4096;
		/// <summary> Maximum length (in bytes) of a private message. </summary>
		internal static int priv_max = 50000;
		/// <summary> Minimum number of milliseconds to wait in between sending chat messages (flood avoidance)</summary>
		internal static int chat_flood = 550;
		
		internal string Body{
			get { return this.Data["message"].ToString(); }
			set{ this.Data["message"] = DNA.Text.TextEngine.BBCode(value).ToString(); }
		}

		internal string[] args { 
			get { return this.Body.Split(' '); } 
			set	{ this.args = value; this.Body = string.Join(" ", this.args); }
		}

		public Message(SystemCommand s){
			this.OpCode = s.OpCode;
			this.Data = s.Data;
			this.Body = this.Data["message"].ToString();
			this.sourceChannel = s.sourceChannel;
			this.sourceUser = s.sourceUser;
		}

		public Message(string messageBody, Message parentMessage) : base(parentMessage) { 
			this.sourceUser = parentMessage.sourceUser;
			this.sourceChannel = parentMessage.sourceChannel;	
			this.Body = messageBody;	
		}

		public Message() : base() { }

		/// <summary>
		/// Sends the message by adding it to the OutgoingMessageQueue
		/// </summary>
		internal new void Send(){
			if (this.OpCode == null)
			{
				this.OpCode = this.sourceUser == null ? "MSG" : "PRI"; //sets Opcode to MSG (send to entire channel) if no user is specified, else to PRI (only to user)
				if (this.sourceUser != null) { this.Data.Add("recipient", this.sourceUser.Name); }
				else { this.Data.Add("channel", this.sourceChannel.Key); }
			};

			//This should, in theory, make sure we don't send any too-long messages.
			//y u no autosplit your buffer
			int MessageLength = System.Text.Encoding.UTF8.GetByteCount(this.Body);
			int MaxLength = this.OpCode == "MSG" ? Message.chat_max : Message.priv_max;
			if (MessageLength > MaxLength) {
				List<Message> messages = new List<Message>();
				Message subMessage = new Message("", this);
				messages.Add(this);
				for (int i = 0; MessageLength > MaxLength; i++){
					subMessage.Body.Insert(subMessage.Body.Length, this.Body[this.Body.Length - i].ToString());
					this.Body = this.Body.Substring(0, this.Body.Length - 1);
					MessageLength = System.Text.Encoding.UTF8.GetByteCount(this.Body);
					messages.Add(subMessage);
				}
				messages.ForEach(x => x.Send()); //If we did this recursively, the last subMessage would send first,[ chunks | to reversed  | leading ]
			}
			base.Send();
		}

		internal int getByteLength(){ return (System.Text.Encoding.UTF8.GetByteCount(this.Body)); }

		/// <summary>
		/// Replies to the message by posting to the same user/channel where the Message originated
		/// </summary>
		/// <param name="replyText">Text to reply with.</param>
		/// <param name="forcePrivate">Should the message be sent as private regardless of parent message origin?</param>
		internal void Reply(string replyText, bool forcePrivate = true){ 
			Message reply = new Message(replyText, this);
			if (forcePrivate) { reply.OpCode = "PRI"; }
			reply.Send();
		}

		public override string ToString(){
			string _message;
			if (this.Data["message"].ToString().StartsWith("/me")) { _message = this.sourceUser.Name + " " + this.Data["message"].ToString().Substring(3); }
			else {_message = this.sourceUser.Name + ": " + this.Data["message"].ToString();}
			return String.Format("<{0}> -- {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _message, Environment.NewLine);
		}
	} //class Message

	internal class Logging{
		internal class LogFile : IDisposable{
			private System.Timers.Timer flushTimer = new System.Timers.Timer();
			private FileStream logFileStream = null;
			private StreamWriter logger = null;
			private bool disposed = false;
			
			public void Log(string s){ 
				s = String.Format("<{0}> -- {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), s, Environment.NewLine);
				Console.Write(s);
				this.logger.Write(s);
				//#if DEBUG
				CogitoUI.console.console.AppendText(s);
				//#endif
				}

			/// <summary>
			/// To be called when you've already processed the mesasge with a timestamp for the channel; just pass the message.ToString() result and it's logged 'raw'
			/// </summary>
			public void LogRaw(string s){
				Console.Write(s);
				this.logger.Write(s);
				//#if DEBUG
				CogitoUI.console.console.AppendText(s);
				//#endif
			}

			/// <summary>
			/// Creates a FileStream for writing to the logfile, and periodically (default: 10 sec.) flushes the buffer to preserve that data in event of failure. 
			/// Keeping the file open rathern than open -> append -> close aparently improves performance
			/// </summary>
			/// <param name="FileName">The Filename of the file to be logged to. Folder is automatically added.</param>
			/// <param name="subfolder">The folder below the root logging folder, if any, this log should be put into. Default is none.</param>
			/// <param name="writeInterval">The interval, in milliseconds, between calling Flush().</param>
			/// <param name="extension">The file extension for the log, default ".txt".</param>
			public LogFile(string FileName, string subfolder = "logs", string extension = ".txt", long writeInterval = Config.AppSettings.loggingInterval){
				string FilePath;
				try{
					FilePath = Path.Combine(CogitoSharp.Config.AppSettings.LoggingPath, subfolder);
					System.IO.Directory.CreateDirectory(FilePath);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.InnerException +"\n\tGetting Temp Path for log file instead...");
					FilePath = Path.GetTempPath();
				}
				FilePath += @"\" + FileName + extension;
				this.logFileStream = File.Open(FilePath, FileMode.Append, FileAccess.Write);
				this.logger = new StreamWriter(this.logFileStream);
				this.flushTimer.Interval = writeInterval;
				this.flushTimer.Elapsed += flushTimer_Elapsed;
				this.flushTimer.AutoReset = true;
				this.flushTimer.Start();

			}

			void flushTimer_Elapsed(object sender, ElapsedEventArgs e){ this.logger.Flush(); }

			~LogFile(){ Dispose(false); }

			//implement IDisposable
			public void Dispose(){
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing){
				if (!this.disposed){
					if (disposing){
						this.logger.Dispose();
						this.flushTimer.Dispose();
						// Free other state (managed objects).
					}
					this.disposed = true;
				}
			}

		} //class LogFile
		internal delegate void ConsoleLoggingDelegate(string s);

		ConsoleLoggingDelegate WriteToConsole = CogitoUI.console.console.AppendText;

	} //class Logging
} //namespace IO
