using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using CogitoSharp.IO;
using Newtonsoft.Json;

namespace CogitoSharp
{
	/// <summary>
	/// Contains all methods that process FList server/client commands
	/// </summary>
	internal sealed class FListProcessor
	{
		/// <summary>This command requires chat op or higher. Request a character's account be banned from the server.
		/// Send  As: ACB { "character": string }</summary>
		public static void ACB(SystemCommand c) { }

		/// <summary>Sends the client the current list of chatops.
		/// Received: ADL { "ops": [string] }</summary>
		internal static void ADL(SystemCommand c) {
			List<string> AllOpsData = ((Newtonsoft.Json.Linq.JArray)c.Data["ops"]).ToObject<List<string>>();
			foreach (string op in AllOpsData){
				User _op = Core.getUser(op);
				Core.globalOps.Add(_op);
			}
		}

		/// <summary>The given character has been promoted to chatop. 
		/// Received: AOP { "character": string }
		/// Send  As: AOP { "character": string }</summary>
		internal static void AOP(SystemCommand c) { }

		/// <summary>This command requires chat op or higher. Requests a list of currently connected alts for a characters account. 
		/// Send  As: AWC { "character": string }</summary>
		internal static void AWC(SystemCommand c) { }

		/// <summary>Incoming admin broadcast. 
		/// Received: BRO { "message": string }
		/// Send  As: BRO { "message": string } (as if) </summary>
		internal static void BRO(SystemCommand c) { }

		/// <summary>This command requires channel op or higher. Request the channel banlist.
		/// Send  As: CBL { "channel": string } </summary>
		internal static void CBL(SystemCommand c) { }

		/// <summary> This command requires channel op or higher. Bans a character from a channel. 
		/// Send  As: CBU {"character": string, "channel": string}
		/// Received: CBU {"operator":string,"channel":string,"character":string}</summary>
		internal static void CBU(SystemCommand c) { }

		/// <summary>  Create a private, invite-only channel. 
		/// Send  As: CCR { "channel": string } </summary>
		internal static void CCR(SystemCommand c) { }

		/// <summary>Alerts the client that that the channel's description has changed. This is sent whenever a client sends a JCH to the server. 
		/// Received: CDS { "channel": string, "description": string }
		/// Send  As: CDS { "channel": string, "description": string }</summary>
		internal static void CDS(SystemCommand c) { }

		/// <summary> Sends the client a list of all public channels.
		/// Send  As: CHA
		/// Received: CHA { "channels": [object] } </summary>
		//  CHA {"channels":[{"name":"Gay Males","mode":"both","characters":0}, [...] } ] }
		internal static void CHA(SystemCommand c) {
			try{
				Newtonsoft.Json.Linq.JArray AllChannelData = Newtonsoft.Json.Linq.JArray.Parse(c.Data["channels"].ToString());
				List<Dictionary<string, object>> _AllChannelData = AllChannelData.ToObject<List<Dictionary<string, object>>>();
				foreach (Dictionary<string, object> currentChannel in _AllChannelData){
					Channel ch = Core.getChannel(currentChannel["name"].ToString());
					ch.mode = (ChannelMode)Enum.Parse(typeof(ChannelMode), currentChannel["mode"].ToString());
					Console.WriteLine("Got data for Channel " + ch.name);
				}
				Core.getChannel("Development").Join();
			}
			catch (Exception e){ Core.ErrorLog.Log(String.Format("Error whilst parsing channel list: {0}", e.Message)); }
			//TODO Entry point for all auto-joins, now that we know the channels
			
		}

		/// <summary>  Invites a user to a channel. Sending requires channel op or higher.
		/// Send  As: CIU { "channel": string, "character": string }
		/// Received: CIU { "sender":string,"title":string,"name":string } </summary>
		internal static void CIU(SystemCommand c) {
			Channel ch = Core.getChannel(c.Data["title"].ToString());
			Core.SystemLog.Log(String.Format("Joining channel '{0}' by invitation of user '{1}'", c.Data["title"].ToString(), c.Data["sender"].ToString()));
			ch.Join();
		}

		/// <summary> This command requires channel op or higher. Kicks a user from a channel. 
		/// Received: CKU {"operator":string,"channel":string,"character":string}
		/// Send  As: CKU { "channel": string, "character": string }</summary>
		internal static void CKU(SystemCommand c) { }

		/// <summary> This command requires channel op or higher. Promotes a user to channel operator.
		/// Received: COA {"character":string, "channel":string}
		/// Send  As: COA { "channel": string, "character": string }</summary>
		internal static void COA(SystemCommand c) { }

		/// <summary> Gives a list of channel ops. Sent in response to JCH.
		/// Received: COL { "channel": string, "oplist": [string] }
		/// Send  As: COL { "channel": string }</summary>
		internal static void COL(SystemCommand c) { 
		
		}

		/// <summary> After connecting and identifying you will receive a CON command, giving the number of connected users to the network.
		/// Received: CON { "count": int }</summary>
		internal static void CON(SystemCommand c) { 
			using (FileStream fs = File.Open(Config.AppSettings.LoggingPath + "Connections.log", FileMode.Append)){
				StreamWriter fsw = new StreamWriter(fs);
				DateTime Now = DateTime.Now;
				fsw.Write(String.Format("{0}\t{1}\t{2}\tusers connected\r\n", Now.ToString("yyyy-MM-dd"), Now.ToString("HH:mm:ss"), c.Data["count"].ToString()));
				fsw.Flush();
				fsw.Close();
			}
		//new IO.SystemCommand("ORS").Send();
		//new IO.SystemCommand("CHA").Send();
		//new IO.SystemCommand("STA { \"status\": \"online\", \"statusmsg\": \"Running Cogito# v" + System.Windows.Forms.Application.ProductVersion + "\" }").Send();

		}

		/// <summary> This command requires channel op or higher. Demotes a channel operator (channel moderator) to a normal user.
		/// Send  As: COR { "channel": string, "character": string }
		/// Received: COR {"character":"character_name", "channel":"channel_name"}</summary>
		internal static void COR(SystemCommand c) { }

		/// <summary> This command is admin only. Creates an official channel.
		/// Send  As: CRC { "channel": string }</summary>
		internal static void CRC(SystemCommand c) { }

		/// <summary> This command requires channel op or higher. Set a new channel owner.
		/// Received: CSO {"character":"string","channel":"string"}
		/// Send  As: CSO {"character":"string","channel":"string"}</summary>
		internal static void CSO(SystemCommand c) { }

		/// <summary> This command requires channel op or higher. Temporarily bans a user from the channel for 1-90 minutes. A channel timeout.
		/// Send  As: CTU { "channel":string, "character":string, "length":int }
		/// Received: CTU {"operator":"string","channel":"string","length":int,"character":"string"}</summary>
		internal static void CTU(SystemCommand c) { }

		/// <summary> This command requires channel op or higher. Unbans a user from a channel.
		/// Send  As: CUB { channel: "channel", character: "character" }</summary>
		internal static void CUB(SystemCommand c) { }

		/// <summary> This command is admin only. Demotes a chatop (global moderator).
		/// Received: DOP { "character": character }
		/// Send  As: DOP { "character": string }</summary>
		internal static void DOP(SystemCommand c) { }

		/// <summary> Indicates that the given error has occurred.
		/// Received: ERR {"message": "string", "number": int}</summary>
		internal static void ERR(SystemCommand c) { Core.ErrorLog.Log(String.Format("F-List Error {0} : {1}", c.Data["number"], c.Data["message"])); }

		/// <summary> Search for characters fitting the user's selections. Kinks is required, all other parameters are optional.
		/// Send  As: FKS { "kinks": [int], "genders": [enum], "orientations": [enum], "languages": [enum], "furryprefs": [enum], "roles": [enum] }
		/// Received: FKS { "characters": [object], "kinks": [object] }</summary>
		internal static void FKS(SystemCommand c) { }

		/// <summary> Sent by the server to inform the client a given character went offline.
		/// Received: FLN { "character": string }</summary>
		internal static void FLN(SystemCommand c) { }

		/// <summary> Initial friends list.
		/// Received: FRL { "characters": [string] }</summary>
		internal static void FRL(SystemCommand c) { }

		/// <summary> Server hello command. Tells which server version is running and who wrote it.
		/// Received: HLO { "message": string }</summary>
		internal static void HLO(SystemCommand c) {	}

		/// <summary> Initial channel data. Received in response to JCH, along with CDS.
		/// Received: ICH { "users": [object], "channel": string, "title": string, "mode": enum }
		/// ICH {"users": [{"identity": "Shadlor"}], "channel": "Frontpage", mode: "chat"}</summary>
		internal static void ICH(SystemCommand c) { }

		internal static void IDN(SystemCommand c) { }

		/// <summary> A multi-faceted command to handle actions related to the ignore list. 
		/// The server does not actually handle much of the ignore process, as it is the client's responsibility to block out messages it recieves from the server if that character is on the user's ignore list.
		/// Received: IGN { "action": string, "characters": [string] | "character":object }
		/// TODO: ???
		/// </summary>
		internal static void IGN(SystemCommand c) { }

		/// <summary>Indicates the given user has joined the given channel. This may also be the client's character.
		///	Received: JCH { "channel": string, "character": object, "title": string }
		///	Send  As: JCH { "channel": string } </summary>
		internal static void JCH(SystemCommand c) { 
			Channel ch = Core.getChannel(c.Data["channel"].ToString()); //"title" would be the channel's name, which in case of private channels can collide!
			Dictionary<string, object> UserData = (Dictionary<string, object>)c.Data["character"];
			User us = Core.getUser(UserData["identity"].ToString());
			ch.Log(String.Format("User '{0}' joined Channel '{1}'", us.Name, ch.name));
			ch.Users.Add(us); 
			}

		/// <summary> Kinks data in response to a KIN client command.
		/// Received: KID { "type": enum, "message": string, "key": [int], "value": [int] }</summary>
		internal static void KID(SystemCommand c) { }

		/// <summary> This command requires chat op or higher. Request a character be kicked from the server.
		/// Send  As: KIK { "character": string }
		/// </summary>
		internal static void KIK(SystemCommand c) { }

		/// <summary> Request a list of a user's kinks.
		/// Send  As: KIN { "character": string }</summary>
		internal static void KIN(SystemCommand c) { }

		/// <summary> An indicator that the given character has left the channel. This may also be the client's character.
		/// Received: LCH { "channel": string, "character": character }
		/// Send  As: LCH { "channel": string }</summary>
		internal static void LCH(SystemCommand c) { Core.getChannel(c.Data["channel"].ToString()).Leave(); }

		/// <summary> Sends an array of *all* the online characters and their gender, status, and status message.
		/// Received: LIS { characters: [object] }</summary>
		//LIS {"characters":[["Zeus Keraunos","Male","online",""],["Dionysos Thyrsos","Male","online","... Uncle, you have it bad."],["Bill Cypher","None","online",""],["Uvaxstra","Male","online",""]]}
		// /!\ OBVIOUSLY A HUGE POTENTIAL PERFORMANCE SINK /!\
		internal static void LIS(SystemCommand c) {
			try{
				foreach (List<string> currentUser in (List<List<string>>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<string>>>(c.Data["characters"].ToString())){
					//Expected format e.g. ["Zeus Keraunos","Male","online",""]
					//						Name		     Gender	Status  Statusmessage
					User u = Core.getUser(currentUser[0]);
					u.Gender = (Genders)Enum.Parse(typeof(Genders), currentUser[1]);
					u.status = (Status)Enum.Parse(typeof(Status), currentUser[2]);
				}
			}
			catch (InvalidCastException){ 
				Core.ErrorLog.Log("Could not cast contents of LIS message to List<List<string>> - See dump below: "); 
				Core.ErrorLog.Log(c.Data["characters"].ToString());
				Core.ErrorLog.Log("\t\tDump complete");	
			}
		}

		/// <summary> A roleplay ad is received from a user in a channel.
		/// Received: LRP { "channel": "", "message": "", "character": ""}</summary>
		internal static void LRP(SystemCommand c) { }

		/// <summary> Sending/Receiving Messages in a channel
		/// Received: MSG { "character": string, "message": string, "channel": string }
		/// Send  As: MSG { "channel": string, "message": string }</summary>
		internal static void MSG(SystemCommand c)
		{
			Message m = (Message)c;
			m.sourceChannel.MessageReceived(m);
			if (m.args[0].StartsWith(Config.AppSettings.TriggerPrefix)) {
				Core.SystemLog.Log("Attempting to execute bot method " + m.args[0] + "...");
				//try { typeof(Config.AITriggers).GetMethod(c.opcode, BindingFlags.NonPublic | BindingFlags.Static).Invoke(c, new Object[] { c }); }
				try { 
					Delegate AIMethod = Config.AITriggers[m.args[0]]; 
					AIMethod.Method.Invoke(c, new object[] {m});
				}
				catch (KeyNotFoundException NoMethod) { Core.ErrorLog.Log(String.Format("Invocation of Bot Method {0} failed, as the method is not registered in the AITriggers.Triggers dictionary does not exist:\n\t{1}\n\t{2}", c.OpCode, NoMethod.Message, NoMethod.InnerException)); }
				catch (TargetException NoMethod) { Core.ErrorLog.Log(String.Format("Invocation of Bot Method {0} failed, as the method does not exist:\n\t{1}\n\t{2}", c.OpCode, NoMethod.Message, NoMethod.InnerException)); }
				catch (ArgumentException WrongData) { Core.ErrorLog.Log(String.Format("Invocation of Bot Method {0} failed, due to a wrong argument:\n\t{1}\n\t{2}", c.OpCode, WrongData.Message, WrongData.InnerException)); }
				catch (Exception FuckUp) { Core.ErrorLog.Log(String.Format("Invocation of Bot Method {0} failed due to an unexpected error:\n\t{1}\n\t{2}", c.OpCode, FuckUp.Message, FuckUp.InnerException)); }
			}
			//TODO: Plugin System, Bot commands, the works.
		}

		/// <summary> A user connected.
		/// Received: NLN { "identity": string, "gender": enum, "status": enum }</summary>
		internal static void NLN(SystemCommand c)
		{
			User u = Core.getUser(c.Data["identity"].ToString());
			u.status = (Status)Enum.Parse(typeof(Status), c.Data["status"].ToString());
		}

		/// <summary> Gives a list of open private rooms.
		/// Received: ORS { "channels": [object] } 
		/// e.g. "channels": [{"name":"ADH-300f8f419e0c4814c6a8","characters":0,"title":"Ariel's Fun Club"}] etc. etc.
		/// Send  As: ORS</summary>
		internal static void ORS(SystemCommand c) { 
			try{
				List<Dictionary<string, string>> channelItems = (c.Data["channels"] as List<Dictionary<string, string>>);
				List<Dictionary<string, string>>.Enumerator e =  channelItems.GetEnumerator();
				while (e.MoveNext()){
					Dictionary<string, string> channelData = e.Current;
					Channel currentChannel = Core.getChannel(HttpUtility.HtmlDecode(channelData["title"]));
					currentChannel.key = channelData["name"];
					CogitoUI.console.console.AppendText(String.Format("Setting Key for channel {0} to '{1}'\n", currentChannel.name, currentChannel.key));
				}
			}
			catch{ }
		}

		/// <summary> Ping command from the server, requiring a response, to keep the connection alive.
		/// Received: PIN
		/// Send  As: PIN </summary>
		internal static void PIN(SystemCommand c) { 
			Core.websocket.Send("PIN"); 
		}

		/// <summary> Profile data commands sent in response to a PRO client command. 
		/// Received: PRD { "type": enum, "message": string, "key": string, "value": string }</summary>
		internal static void PRD(SystemCommand c) { }

		/// <summary> Private Messaging
		/// Received: PRI { "character": string, "message": string }
		/// Send  As: PRI { "recipient": string, "message": string }</summary>
		internal static void PRI(SystemCommand c)
		{
			Message message = (Message)c;
		}

		/// <summary> Requests some of the profile tags on a character, such as Top/Bottom position and Language Preference.
		/// Send  As: PRO { "character": string }</summary>
		internal static void PRO(SystemCommand c) { }

		/// <summary> This command requires chat op or higher. Reload certain server config files
		/// Send  As: RLD { "save": string }
		/// </summary>
		internal static void RLD(SystemCommand c) { }

		/// <summary> Roll dice or spin the bottle.
		/// Send  As: RLL { "channel": string, "dice": string }
		/// </summary>
		internal static void RLL(SystemCommand c) { }

		/// <summary> Change room mode to accept chat, ads, or both.
		/// Received: RMO {"mode": enum, "channel": string}
		/// Send  As: RMO {"channel": string, "mode": enum}</summary>
		internal static void RMO(SystemCommand c) { }

		/// <summary> This command requires channel op or higher. Sets a private room's status to closed or open.
		/// Send  As: RST { "channel": string, "status": enum }</summary>
		internal static void RST(SystemCommand c) { }

		/// <summary> Real-time bridge. Indicates the user received a note or message, right at the very moment this is received.
		/// Received: RTB { "type": string, "character": string }</summary>
		internal static void RTB(SystemCommand c) { }

		/// <summary> This command is admin only. Rewards a user, setting their status to 'crown' until they change it or log out.
		/// Send  As: RWD { "character": string }</summary>
		internal static void RWD(SystemCommand c) { }

		/// <summary> Alerts admins and chatops (global moderators) of an issue.
		/// Send  As: SFC { "action": "report", "report": string, "character": string }</summary>
		/// Received: SFC {action:"string", moderator:"string", character:"string", timestamp:"string"}
		internal static void SFC(SystemCommand c) { }

		/// <summary> A user changed their status
		/// Received: STA { status: "status", character: "channel", statusmsg:"statusmsg" }
		/// Send  As: STA { "status": enum, "statusmsg": string }</summary>
		internal static void STA(SystemCommand c) { }

		/// <summary> An informative autogenerated message from the server.
		/// Received: SYS { "message": string, "channel": string }</summary>
		internal static void SYS(SystemCommand c) { }

		/// <summary> This command requires chat op or higher. Times out a user for a given amount minutes.
		/// Send  As: TMO { "character": string, "time": int, "reason": string }</summary>
		internal static void TMO(SystemCommand c) { }

		/// <summary> "user x is typing/stopped typing/has entered text" for private messages.
		/// Send  As: TPN { "character": string, "status": enum }
		/// Received: TPN { "character": string, "status": enum }</summary>
		internal static void TPN(SystemCommand c) { }

		/// <summary> This command requires chat op or higher. Unbans a character's account from the server.
		/// Send  As: UBN { "character": string }</summary>
		internal static void UBN(SystemCommand c) { }

		/// <summary> Informs the client of the server's self-tracked online time, and a few other bits of information
		/// Received: UPT { "time": int, "starttime": int, "startstring": string, "accepted": int, "channels": int, "users": int, "maxusers": int }</summary>
		internal static void UPT(SystemCommand c) {	}

		internal enum Permissions : int{
			Admin = 1, chatop = 2, chanop = 4, helpdeskchat = 8, helpdeskgeneral = 16, moderationsite = 32, reserved = 64, grouprequests = 128,
			newsposts = 256, changelog = 512, featurerequests = 1024, bugreports = 2048, tags = 4096, kinks = 8192, developer = 16384, tester = 32768,
			subscriptions = 65536, formerstaff = 131072
		};

		//priv_max: Maximum number of bytes allowed with PRI.
		//lfrp_max: Maximum number of bytes allowed with LRP.
		//lfrp_flood: Required seconds between LRP messages.
		//chat_flood: Required seconds between MSG messages.
		//permissions: Permissions mask for this character.
		//chat_max: Maximum number of bytes allowed with MSG.	
		/// <summary> Variables the server sends to inform the client about server variables.</summary>
		internal static void VAR(SystemCommand c){
			//TODO: check format
			switch ((string)c.Data["variable"]){
				case "msg_flood":
				case "chat_flood":
					IO.Message.chat_flood = (int)(float.Parse(c.Data["value"].ToString()) * 1250); //Multiplying by 1005 for latency protection / timer shenanigans, and converting from seconds to miliseconds
					Core.EternalSender.Change(0, IO.Message.chat_flood);
					Core.SystemLog.Log("SYS: Send interval auto-adjusted to interval of " + (IO.Message.chat_flood / 1000f) + " seconds.");
					break;
				
				case "chat_max":
					IO.Message.chat_max = int.Parse(c.Data["value"].ToString());
					break;
				
				case "priv_max":
					IO.Message.priv_max = int.Parse(c.Data["value"].ToString());
					break;
			}
		}
	}
}
