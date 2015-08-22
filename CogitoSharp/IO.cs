using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		internal User user = null;
		internal Channel channel = null;
		internal string message;
		internal string[] args { get { return this.message.Split(' '); } }

		public Message(SystemCommand s)
			: base()
		{
			this.opcode = s.opcode;
			this.user = this.data.ContainsKey("character") ? Core.getUser((string)this.data["character"]) : Account.SYSTEMUSER;
			this.channel = this.data.ContainsKey("channel") ? Core.getChannel((string)this.data["channel"]) : Account.SYSTEMCHANNEL;
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
				this.opcode = this.user == null ? "MSG" : "PRI"; //sets Opcode to MSG (send to entire channel) if no user is specified, else to PRI (only to user)
				if (this.user != null) { this.data.Add("recipient", this.user.Name); }
				else { this.data.Add("channel", this.channel.key); }
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
}
