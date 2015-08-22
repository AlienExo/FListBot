using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CogitoSharp.IO;

namespace CogitoSharp
{
	/// <summary>
	/// Contains all methods that process FList server/client commands
	/// </summary>
	internal sealed class FListProcessor
	{
		/// <summary> ACB This command requires chat op or higher. Request a character's account be banned from the server. </summary>
		public static void ACB(SystemCommand c) { }

		/// <summary> AOP The given character has been promoted to chatop. >> AOP { "character": string } / Promotes user to ChatOP</summary>
		internal static void ADL(SystemCommand c) { }

		/// <summary> </summary>
		internal static void AOP(SystemCommand c) { }

		/// <summary> </summary>
		internal static void AWC(SystemCommand c) { }

		/// <summary> </summary>
		internal static void BRO(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CBL(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CBU(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CCR(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CDS(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CHA(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CIU(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CKU(SystemCommand c) { }

		/// <summary> </summary>
		internal static void COA(SystemCommand c) { }

		/// <summary> </summary>
		internal static void COL(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CON(SystemCommand c) { }

		/// <summary> </summary>
		internal static void COR(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CRC(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CSO(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CTU(SystemCommand c) { }

		/// <summary> </summary>
		internal static void CUB(SystemCommand c) { }

		/// <summary> </summary>
		internal static void DOP(SystemCommand c) { }

		/// <summary> </summary>
		internal static void ERR(SystemCommand c) { }

		/// <summary> </summary>
		internal static void FKS(SystemCommand c) { }

		/// <summary> </summary>
		internal static void FLN(SystemCommand c) { }

		/// <summary> </summary>
		internal static void FRL(SystemCommand c) { }

		/// <summary> </summary>
		internal static void HLO(SystemCommand c) { }

		/// <summary> </summary>
		internal static void ICH(SystemCommand c) { }

		/// <summary> </summary>
		internal static void IGN(SystemCommand c) { }

		/// <summary> </summary>
		internal static void JCH(SystemCommand c) { }

		/// <summary> </summary>
		internal static void KID(SystemCommand c) { }

		/// <summary> </summary>
		internal static void KIK(SystemCommand c) { }

		/// <summary> </summary>
		internal static void KIN(SystemCommand c) { }

		/// <summary> </summary>
		internal static void LCH(SystemCommand c) { }

		/// <summary> </summary>
		internal static void LIS(SystemCommand c) { }

		/// <summary> </summary>
		internal static void LRP(SystemCommand c) { }

		/// <summary> </summary>
		internal static void MSG(SystemCommand c)
		{
			Message m = (Message)c;
		}

		/// <summary> </summary>
		internal static void NLN(SystemCommand c)
		{
			User u = Core.getUser(c.data["identity"].ToString());
			u.status = (Status)Enum.Parse(typeof(Status), c.data["status"].ToString());
		}

		/// <summary> </summary>
		internal static void ORS(SystemCommand c) { }

		/// <summary> </summary>
		internal static void PIN(SystemCommand c) { Core.websocket.Send("PIN"); Console.WriteLine(">PONG");}

		/// <summary> </summary>
		internal static void PRD(SystemCommand c) { }

		/// <summary> </summary>
		internal static void PRI(SystemCommand c)
		{
			Message message = (Message)c;
		}

		/// <summary> </summary>
		internal static void PRO(SystemCommand c) { }

		/// <summary> </summary>
		internal static void RLD(SystemCommand c) { }

		/// <summary> </summary>
		internal static void RLL(SystemCommand c) { }

		/// <summary> </summary>
		internal static void RMO(SystemCommand c) { }

		/// <summary> </summary>
		internal static void RST(SystemCommand c) { }

		/// <summary> </summary>
		internal static void RTB(SystemCommand c) { }

		/// <summary> </summary>
		internal static void RWD(SystemCommand c) { }

		/// <summary> </summary>
		internal static void SFC(SystemCommand c) { }

		/// <summary> </summary>
		internal static void STA(SystemCommand c) { }

		/// <summary> </summary>
		internal static void SYS(SystemCommand c) { }

		/// <summary> </summary>
		internal static void TMO(SystemCommand c) { }

		/// <summary> </summary>
		internal static void TPN(SystemCommand c) { }

		/// <summary> </summary>
		internal static void UBN(SystemCommand c) { }

		/// <summary> </summary>
		internal static void UPT(SystemCommand c) { }


		internal enum Permissions : int
		{
			Admin = 1, chatop = 2, chanop = 4, helpdeskchat = 8, helpdeskgeneral = 16, moderationsite = 32, reserved = 64, grouprequests = 128,
			newsposts = 256, changelog = 512, featurerequests = 1024, bugreports = 2048, tags = 4096, kinks = 8192, developer = 16384, tester = 32768,
			subscriptions = 65536, formerstaff = 131072
		};
		/// <summary> </summary>
		internal static void VAR(SystemCommand c)
		{
			//TODO: check formar
			if ((string)c.data["variable"] == "chat_max")
			{
				long NewSenderInterval = (long)(Convert.ToDouble(c.data["value"]) * 1.05);
				Core.EternalSender.Change(1000, NewSenderInterval * 1000);
				Core.cogitoUI
			}
		}
	}
	/*
	ACB This command requires chat op or higher. Request a character's account be banned from the server.<< ACB { "character": string }
	ADL Sends the client the current list of chatops. >> ADL { "ops": [string] }
	AOP The given character has been promoted to chatop. >> AOP { "character": string }
	AOP This command is admin only. Promotes a user to be a chatop (global moderator). << AOP { "character": string }
	AWC This command requires chat op or higher. Requests a list of currently connected alts for a characters account. << AWC { "character": string }
	BRO Incoming admin broadcast. >> BRO { "message": string }
	BRO This command is admin only. Broadcasts a message to all connections. << BRO { "message": string }
	CBL This command requires channel op or higher. Request the channel banlist. << CBL { "channel": string }
	CBU This command requires channel op or higher. Bans a character from a channel. << CBU {"character": string, "channel": string}
	CBU This command requires channel op or higher. Removes a user from a channel, and prevents them from re-entering. >> CBU {"operator":string,"channel":string,"character":string}
	CCR Create a private, invite-only channel. << CCR { "channel": string }
	CDS Alerts the client that that the channel's description has changed. This is sent whenever a client sends a JCH to the server. >> CDS { "channel": string, "description": string }
	CDS This command requires channel op or higher. Changes a channel's description. << CDS { "channel": string, "description": string }
	CHA Request a list of all public channels. << CHA
	CHA Sends the client a list of all public channels. >> CHA { "channels": [object] }
	CIU Invites a user to a channel. >> CIU { "sender":string,"title":string,"name":string }
	CIU This command requires channel op or higher. Sends an invitation for a channel to a user. << CIU { "channel": string, "character": string }
	CKU This command requires channel op or higher. Kicks a user from a channel. << CKU { "channel": string, "character": string }
	CKU This command requires channel op or higher. Kicks a user from a channel. >> CKU {"operator":string,"channel":string,"character":string}
	COA This command requires channel op or higher. Promotes a user to channel operator. >> COA {"character":"character_name", "channel":"channel_name"}
	COA This command requires channel op or higher. Request a character be promoted to channel operator (channel moderator). << COA { "channel": string, "character": string }
	COL Gives a list of channel ops. Sent in response to JCH. >> COL { "channel": string, "oplist": [string] }
	COL Requests the list of channel ops (channel moderators). << COL { "channel": string }
	CON After connecting and identifying you will receive a CON command, giving the number of connected users to the network. >> CON { "count": int }
	COR This command requires channel op or higher. Demotes a channel operator (channel moderator) to a normal user. << COR { "channel": string, "character": string }
	COR This command requires channel op or higher. Removes a channel operator. >> COR {"character":"character_name", "channel":"channel_name"}
	CRC This command is admin only. Creates an official channel. << CRC { "channel": string }
	CSO Sets the owner of the current channel to the character provided. >> CSO {"character":"string","channel":"string"}
	CSO This command requires channel op or higher. Set a new channel owner. << CSO {"character":"string","channel":"string"}
	CTU Temporarily bans a user from the channel for 1-90 minutes. A channel timeout. >> CTU {"operator":"string","channel":"string","length":int,"character":"string"}
	CTU This command requires channel op or higher. Temporarily bans a user from the channel for 1-90 minutes. A channel timeout. << CTU { "channel":string, "character":string, "length":num }
	CUB This command requires channel op or higher. Unbans a user from a channel. << CUB { channel: "channel", character: "character" }
	DOP The given character has been stripped of chatop status. >> DOP { "character": character }
	DOP This command is admin only. Demotes a chatop (global moderator). << DOP { "character": string }
	ERR Indicates that the given error has occurred. ERR {"message": "You have already joined this channel.", "number": 28}
	FKS Search for characters fitting the user's selections. Kinks is required, all other parameters are optional. << FKS { "kinks": [int], "genders": [enum], "orientations": [enum], "languages": [enum], "furryprefs": [enum], "roles": [enum] }
	FKS Sent by as a response to the client's FKS command, containing the results of the search. >> FKS { "characters": [object], "kinks": [object] }
	FLN Sent by the server to inform the client a given character went offline. >> FLN { "character": string }
	FRL Initial friends list. >> FRL { "characters": [string] }
	HLO Server hello command. Tells which server version is running and who wrote it. >> HLO { "message": string }
	ICH Initial channel data. Received in response to JCH, along with CDS. >> ICH { "users": [object], "channel": string, "title": string, "mode": enum }
	* ICH {"users": [{"identity": "Shadlor"}, {"identity": "Bunnie Patcher"}, {"identity": "DemonNeko"}, {"identity": "Desbreko"}, {"identity": "Robert Bell"}, {"identity": "Jayson"}, {"identity": "Valoriel Talonheart"}, {"identity": "Jordan Costa"}, {"identity": "Skip Weber"}, {"identity": "Niruka"}, {"identity": "Jake Brian Purplecat"}, {"identity": "Hexxy"}], "channel": "Frontpage", mode: "chat"}
	IDN This command is used to identify with the server. << IDN { "method": "ticket", "account": string, "ticket": string, "character": string, "cname": string, "cversion": string }
	IDN Used to inform the client their identification is successful, and handily sends their character name along with it.>> IDN { "character": string }
	IGN A multi-faceted command to handle actions related to the ignore list. The server does not actually handle much of the ignore process, as it is the client's responsibility to block out messages it recieves from the server if that character is on the user's ignore list. << 
	IGN Handles the ignore list. >> IGN { "action": string, "characters": [string] | "character":object }
	JCH Indicates the given user has joined the given channel. This may also be the client's character. >> JCH { "channel": string, "character": object, "title": string }
	JCH Send a channel join request. << JCH { "channel": string }
	KID Kinks data in response to a KIN client command. >> KID { "type": enum, "message": string, "key": [int], "value": [int] }
	KIK This command requires chat op or higher. Request a character be kicked from the server. << KIK { "character": string }
	KIN Request a list of a user's kinks. << KIN { "character": string }
	LCH An indicator that the given character has left the channel. This may also be the client's character. >> LCH { "channel": string, "character": character }
	LCH Request to leave a channel. << LCH { "channel": string }
	LIS Sends an array of all the online characters and their gender, status, and status message. >> LIS { characters: [object] }
	LRP A roleplay ad is received from a user in a channel. LRP { "channel": "", "message": "", "character": ""}
	MSG A message is received from a user in a channel. >> MSG { "character": string, "message": string, "channel": string }
	MSG Sends a message to all other users in a channel. << MSG { "channel": string, "message": string }
	NLN A user connected. >> NLN { "identity": string, "gender": enum, "status": enum }
	ORS Gives a list of open private rooms. >> ORS { "channels": [object] } 
	* {"name":"ADH-300f8f419e0c4814c6a8","characters":0,"title":"Ariel's Fun Club"}
	ORS Request a list of open private rooms. << ORS
	PIN Ping command from the server, requiring a response, to keep the connection alive. >> PIN 
	PIN Sends a ping response to the server. Timeout detection, and activity to keep the connection alive. << PIN
	PRD Profile data commands sent in response to a PRO client command. >> PRD { "type": enum, "message": string, "key": string, "value": string }
	PRI A private message is received from another user. >> PRI { "character": string, "message": string }
	PRI Sends a private message to another user. << PRI { "recipient": string, "message": string }
	PRO Requests some of the profile tags on a character, such as Top/Bottom position and Language Preference. << PRO { "character": string }
	RLD This command requires chat op or higher. Reload certain server config files << RLD { "save": string }
	RLL Roll dice or spin the bottle. << RLL { "channel": string, "dice": string }
	RLL Rolls dice or spins the bottle. 
	RMO Change room mode to accept chat, ads, or both. >> RMO {"mode": enum, "channel": string}
	RMO This command requires channel op or higher. Change room mode to accept chat, ads, or both. << RMO {"channel": string, "mode": enum}
	RST This command requires channel op or higher. Sets a private room's status to closed or open. (private << RST { "channel": string, "status": enum }
	RTB Real-time bridge. Indicates the user received a note or message, right at the very moment this is received. >> RTB { "type": string, "character": string }
	RWD This command is admin only. Rewards a user, setting their status to 'crown' until they change it or log out. << RWD { "character": string }
	SFC Alerts admins and chatops (global moderators) of an issue. << SFC { "action": "report", "report": string, "character": string }
	SFC Alerts admins and chatops (global moderators) of an issue. >> SFC {action:"string", moderator:"string", character:"string", timestamp:"string"}
	STA A user changed their status >> STA { status: "status", character: "channel", statusmsg:"statusmsg" }
	STA Request a new status be set for your character. << STA { "status": enum, "statusmsg": string }
	SYS An informative autogenerated message from the server. >> SYS { "message": string, "channel": string }
	TMO This command requires chat op or higher. Times out a user for a given amount minutes. << TMO { "character": string, "time": int, "reason": string }
	TPN "user x is typing/stopped typing/has entered text" for private messages. << TPN { "character": string, "status": enum }
	TPN A user informs you of his typing status. >> TPN { "character": string, "status": enum }
	UBN This command requires chat op or higher. Unbans a character's account from the server. << UBN { "character": string }
	UPT Informs the client of the server's self-tracked online time, and a few other bits of information >> UPT { "time": int, "starttime": int, "startstring": string, "accepted": int, "channels": int, "users": int, "maxusers": int }
	UPT Requests info about how long the server has been running, and some stats about usage. << UPT
	VAR Variables the server sends to inform the client about server variables. 
	*	priv_max: Maximum number of bytes allowed with PRI.
	*	lfrp_max: Maximum number of bytes allowed with LRP.
	*	lfrp_flood: Required seconds between LRP messages.
	*	chat_flood: Required seconds between MSG messages.
	*	permissions: Permissions mask for this character.
	*  chat_max: Maximum number of bytes allowed with MSG.	
	*/
}
