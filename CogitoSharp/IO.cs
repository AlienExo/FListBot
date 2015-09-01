using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
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
		/// Implementation for fserv - turns data into OPCODE and JSON
		/// </summary>
		/// <returns>A string OPCODE {JSONKEY: "value"}</returns>
		public override string ToString() { return this.opcode.ToUpperInvariant() + " " + JsonConvert.SerializeObject(this.data).ToString(); }

		public SystemCommand(string rawmessage)
		{
#if DEBUG
			Console.WriteLine("Command Instance Creation from: " + rawmessage);
#endif
			this.opcode = rawmessage.Substring(0, 3);
			if (rawmessage.Length<=4) { return; }
			this.data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawmessage.Substring(4));
		}

		public SystemCommand(Message parentMessage)
		{
			this.opcode = parentMessage.opcode;
			this.data = parentMessage.data;
		}

		public SystemCommand() { }
	}

	class Message : SystemCommand
	{
		//TODO Finish Parsing
		internal User sourceUser = null;
		internal Channel sourceChannel = null;
		internal string message;
		internal string[] args { 
			get { return this.message.Split(' '); } 
			set	{ this.args = value; this.message = string.Join(" ", this.args); }
			}

		public Message(SystemCommand s)
			: base()
		{
			this.opcode = s.opcode;
			this.sourceUser = this.data.ContainsKey("character") ? Core.getUser((string)this.data["character"]) : CogitoUI.SYSTEMUSER;
			this.sourceChannel = this.data.ContainsKey("channel") ? Core.getChannel((string)this.data["channel"]) : CogitoUI.SYSTEMCHANNEL;
			this.message = this.data["message"].ToString();
		}

		public Message(string MessageBody, Message parentMessage) : base(parentMessage) { }

		/// <summary>
		/// Sends the message by adding it to the OutgoingMessageQueue
		/// </summary>
		internal new void send()
		{
			if (this.opcode == null)
			{
				this.opcode = this.sourceUser == null ? "MSG" : "PRI"; //sets Opcode to MSG (send to entire channel) if no user is specified, else to PRI (only to user)
				if (this.sourceUser != null) { this.data.Add("recipient", this.sourceUser.Name); }
				else { this.data.Add("channel", this.sourceChannel.key); }
			};
			base.send();
		}

		/// <summary>
		/// Replies to the message by posting to the same user/channel where the Message originated
		/// </summary>
		/// <param name="replyText">Text to reply with.</param>
		internal void reply(string replyText)
		{
			new Message(replyText, this).send();
		}

		public override string ToString()
		{
			string MessageAsString = this.opcode.ToUpperInvariant() + " " + JsonConvert.SerializeObject(this.data).ToString();
#if DEBUG
			Console.WriteLine("Created Message Output String:\n\t" + MessageAsString);
#endif
			return MessageAsString;
		}
	}

	internal class Logging{
		internal class LogFile{
			private System.Timers.Timer flushTimer = new System.Timers.Timer();
			private FileStream logFileStream = null;
			private StreamWriter logger = null;
			
			public void Log(string s){ logger.Write(String.Format("<{0}> -- {1}", DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"), s)); }

			/// <summary>
			/// Creates a FileStream for writing to the logfile, and periodically (default: 10 sec.) flushes the buffer to preserve that data in event of failure. 
			/// Keeping the file open rathern than open -> append -> close aparently improves performance
			/// </summary>
			/// <param name="DestinationFile">The Filename of the file to be logged to. Folder is automatically added.</param>
			/// /// <param name="subfolder">The folder below the root logging folder, if any, this log should be put into. Default is none.</param>
			/// /// <param name="writeInterval">The interval, in milliseconds, between calling Flush().</param>
			public LogFile(string DestinationFile, string subfolder = "", long writeInterval = Config.AppSettings.loggingInteval){
				try{
					//TODO check if path
					DestinationFile = Path.Combine(new string[] {CogitoSharp.Config.AppSettings.LoggingPath, "/logs/", DestinationFile});
					logFileStream = File.Open(DestinationFile, FileMode.Append, FileAccess.Write);
					logger = new StreamWriter(this.logFileStream);
					flushTimer.Interval = writeInterval;
					flushTimer.Elapsed += flushTimer_Elapsed;
					flushTimer.Start();
				}

				catch{
					//todo implement
				}
			}

			void flushTimer_Elapsed(object sender, ElapsedEventArgs e){
				this.logFileStream.Flush();
			}
		}
	}
}
