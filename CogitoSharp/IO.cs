using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json;

using CogitoSharp;

namespace CogitoSharp.IO
{
	class SystemCommand
	{

		internal protected string opcode { get; set; }
		internal protected Dictionary<string, object> data = new Dictionary<string, object>();

		/// <summary>
		/// Sends the message by adding it to the OutgoingMessageQueue
		/// </summary>
		internal void send() { Core.OutgoingMessageQueue.Enqueue(this); }

		/// <summary>
		/// Produces a string that fserv can interpret.
		/// </summary>
		/// <returns>A string "OPCODE {JSONKEY: "value", [...]}"</returns>
		public string ToServerString() { return this.opcode.ToUpperInvariant() + " " + JsonConvert.SerializeObject(this.data).ToString(); }

		public SystemCommand(string rawmessage){
			this.opcode = rawmessage.Substring(0, 3);
			if (rawmessage.Length<=4) { return; }
			this.data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawmessage.Substring(4));
		}

		public SystemCommand(Message parentMessage){
			this.opcode = parentMessage.opcode;
			this.data = parentMessage.data;
		}

		public SystemCommand() { }
	}

	class Message : SystemCommand{
		//TODO Finish Parsing
		internal User sourceUser = null;
		internal Channel sourceChannel = null;

		/// <summary> Maximum length (in bytes) of a channel message; longer and you gotta split it </summary>
		internal static int chat_max = 4096;
		/// <summary> Maximum length (in bytes) of a private message. </summary>
		internal static int priv_max = 50000;
		
		internal string message{
			get { return this.data["message"].ToString(); }
			set{ this.data["message"] = value; }
		}

		internal string[] args { 
			get { return this.message.Split(' '); } 
			set	{ this.args = value; this.message = string.Join(" ", this.args); }
			}

		public Message(SystemCommand s)	: base(){
			this.opcode = s.opcode;
			this.sourceUser = this.data.ContainsKey("character") ? Core.getUser((string)this.data["character"]) : null;
			this.sourceChannel = this.data.ContainsKey("channel") ? Core.getChannel((string)this.data["channel"]) : null;
			this.data = s.data;
		}

		public Message(string messageBody, Message parentMessage) : base(parentMessage) { 
			this.sourceUser = parentMessage.sourceUser;
			this.sourceChannel = parentMessage.sourceChannel;	
			this.message = messageBody;	
		}

		public Message() : base() { }

		/// <summary>
		/// Sends the message by adding it to the OutgoingMessageQueue
		/// </summary>
		internal new void send(){
			if (this.opcode == null)
			{
				this.opcode = this.sourceUser == null ? "MSG" : "PRI"; //sets Opcode to MSG (send to entire channel) if no user is specified, else to PRI (only to user)
				if (this.sourceUser != null) { this.data.Add("recipient", this.sourceUser.Name); }
				else { this.data.Add("channel", this.sourceChannel.key); }
			};

			//This should, in theory, make sure we don't send any too-long messages.
			//y u no autosplit your buffer
			int MessageLength = System.Text.Encoding.UTF8.GetByteCount(this.message);
			int MaxLength = this.opcode == "MSG" ? Message.chat_max : Message.priv_max;
			if (MessageLength > MaxLength) {
				List<Message> messages = new List<Message>();
				Message subMessage = new Message("", this);
				messages.Add(this);
				for (int i = 0; MessageLength > MaxLength; i++){
					subMessage.message.Insert(subMessage.message.Length, this.message[this.message.Length - i].ToString());
					this.message = this.message.Substring(0, this.message.Length - 1);
					MessageLength = System.Text.Encoding.UTF8.GetByteCount(this.message);
					messages.Add(subMessage);
				}
				messages.ForEach(x => x.send()); //If we did this recursively, the last subMessage would send first, chunks | to reversed  | leading 
			}
			base.send();
		}

		internal int getByteLength(){ return (System.Text.Encoding.UTF8.GetByteCount(this.message)); }

		/// <summary>
		/// Replies to the message by posting to the same user/channel where the Message originated
		/// </summary>
		/// <param name="replyText">Text to reply with.</param>
		internal void reply(string replyText){ new Message(replyText, this).send(); }

		public override string ToString(){
			if (this.data["message"].ToString().StartsWith("/me")) { return this.sourceUser.Name + " " + this.data["message"].ToString().Substring(3); }
			else {return this.sourceUser.Name + ": " + this.data["message"].ToString();}
		}
	}
	internal class Logging{
		internal class LogFile : IDisposable{
			private System.Timers.Timer flushTimer = new System.Timers.Timer();
			private FileStream logFileStream = null;
			private StreamWriter logger = null;
			private bool disposed = false;
			
			public void Log(string s){ 
				s = String.Format("<{0}> -- {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), s);
				Console.Write(s);
				this.logger.Write(s); 
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
						// Free other state (managed objects).
					}
					this.flushTimer.Stop();
					this.flushTimer.Dispose();
					this.logger.Flush();
					this.logger.Dispose();
					this.disposed = true;
				}
			}

		} //class LogFile
		internal delegate void ConsoleLoggingDelegate(string s);

		ConsoleLoggingDelegate WriteToConsole = CogitoUI.console.console.AppendText;

	} // class Logging
} // namespace IO
