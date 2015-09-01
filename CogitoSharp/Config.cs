﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CogitoSharp.Config
{
	internal static class URLConstants{
		const string AvatarURI = "";
		internal const string Domain = @"https://www.f-list.net";
		internal const string Api = Domain + @"/json/api/";
		internal const string CharacterInfo = Api + @"character-info.php";
		internal const string GetTicket = Domain + @"/json/getApiTicket.php";
		internal const string Login = Domain + @"/action/script_login.php";
		internal const string ReadLog = Domain + @"/fchat/getLog.php?log=";
		internal const string ViewNote = Domain + @"/view_note.php?note_id=";
		internal const string UploadLog = Domain + @"/json/api/report-submit.php";
		internal const string ViewHistory = Domain + @"/history.php?name=";
		internal const string SendNote = Domain + @"/json/notes-send.json";
		internal const string SearchFields = Domain + @"/json/chat-search-getfields.json?ids=true";
		internal const string ProfileImages = Domain + @"/json/profile-images.json";
		internal const string KinkList = Domain + @"/json/api/kink-list.php";
		internal const string IncomingFriendRequests = Api + "request-list.php";
		internal const string OutgoingFriendRequests = Api + "request-pending.php";
		internal const string CharacterPage = Domain + "/c/";
		internal const string StaticDomain = @"https://static.f-list.net";
		internal const string CharacterAvatar = StaticDomain + @"/images/avatar/";
		internal const string EIcon = StaticDomain + @"/images/eicon/";
		internal const string ProfileRoot = Domain + @"/c/";
	}

	internal static class AppSettings{
		internal static TimeSpan ticketLifetime = new TimeSpan(5, 0, 0);
		internal static TimeSpan reconnectionStagger = new TimeSpan(0, 0, 10);
		internal static string LoggingPath = Properties.Settings.Default.LogPath != "" ? Properties.Settings.Default.LogPath : new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
		internal const long loggingInteval = 10000;
	}

	internal static class AITriggers{
	
	}
}