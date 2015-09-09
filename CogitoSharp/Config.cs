using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogitoSharp
{
	class Config{
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
			internal static TimeSpan reconnectionStagger = new TimeSpan(0, 0, 10);
			internal static TimeSpan ticketLifetime = new TimeSpan(5, 0, 0);
			internal static TimeSpan userProfileRefreshPeriod = new TimeSpan(7, 0, 0, 0);
			internal const long loggingInterval = 10000;
			internal static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
			internal static string LoggingPath = Properties.Settings.Default.LogPath != "" ? Properties.Settings.Default.LogPath : AppDomain.CurrentDomain.BaseDirectory;
			internal static string PluginsPath = AppPath + @"plugins\";
			internal static string DataPath = AppPath + @"data\";
			internal static string AvatarPath = DataPath + @"avatars\";
			internal const string UserFileName = "Account.dat";
			internal const string UserDBFileName = "Users.dat";
			internal const string ChannelDBFileName = "Channels.dat";
			internal const string MasterKey = "";
			internal const string TriggerPrefix = ".";
			internal static string DefaultAvatarFile = DataPath + "DefaultAvatar.bmp";
			internal const int MessageBufferSize = 256;
		}

		internal static Dictionary<string, Delegate> AITriggers = new Dictionary<string, Delegate>();

		internal static void RegisterPluginTrigger(string Trigger, Delegate OnTrigger){
			if (!Trigger.StartsWith(AppSettings.TriggerPrefix)) { Trigger.Insert(0, AppSettings.TriggerPrefix); }
			AITriggers.Add(Trigger, OnTrigger);
			Core.SystemLog.Log(String.Format("Added trigger {0} for Delegate {1}", Trigger, OnTrigger));
		}
	}
}
