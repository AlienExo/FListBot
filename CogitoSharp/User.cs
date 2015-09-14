using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Markup;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace CogitoSharp
{
	enum Status : int { online = 0, crown, looking, idle, busy, dnd, away, offline}
	/// <summary>User (synonymous with Character)</summary>
	[Serializable]
	public class User : IComparable, IDisposable{
		[NonSerialized] private static int Count;
		[NonSerialized] private readonly int UID = ++User.Count;
		[NonSerialized] internal object UserLock = new Object();
		[NonSerialized] internal ChatTab userTab;
		[NonSerialized] internal IO.Logging.LogFile userLog = null;
		[NonSerialized] private bool disposed = false;

		/// <summary> xXxSEPHIROTHxXx </summary>
		public readonly string Name = null;

		/// <summary> 2 shota 4 u </summary>
		public int Age
		{
			get{
				if (this.age == 0 && (DateTime.Now - dataTakenOn) >= Config.AppSettings.userProfileRefreshPeriod) { GetProfileInfo(); } //Profile data is hella old; refresh
				return this.age;
			}
		}
		private int age = 0;

		/// <summary> You're fat. </summary>
		public int Weight;

		internal string Gender = "None";
		internal Status status = Status.online;
		/// <summary> Sexual orientation; can't be made an enum due to hyphens not working and that fucking up parsing, iirc</summary>
		internal string Orientation;

		/// <summary> Stores user-generated Memos, to be displayed in a sidebar or w/e</summary>
		internal string[] Memos;

		/// <summary> Since currently I'd have to implement custom parsing code for all of this, instead I just stick it into a dict. Eye color, dick size, everything is in here. Probably.</summary>
		public Dictionary<string, object> Stats;

		/// <summary> Here's where things get WEIRD.</summary>
		public Dictionary<string, List<string>> Kinks;
		
		/// <summary> Filtering ideas blatantly stolen from slimCat; sorry, Andrew.</summary>
		internal bool isInteresting;
		internal Bitmap Avatar; //...does this get serialized? like, do I have to bother with the... oh, fuck it, I'll just slap non-serialized on it.

		/// <summary> Stores the DateTime on which the profile was last scraped, allowing the program to self-update every... what, week?</summary>
		internal DateTime dataTakenOn = new DateTime(1, 1, 1);

		internal DateTime avatarTakenOn = new DateTime(1, 1, 1);

		public User(string nName) { 
			this.Name = nName;
			try{ this.Avatar = (Bitmap)Bitmap.FromFile(Config.AppSettings.AvatarPath + this.Name.ToLowerInvariant() + ".bmp"); }
			catch (FileNotFoundException){ this.Avatar = Core.DefaultAvatar; }

			catch (DirectoryNotFoundException){
				Core.ErrorLog.Log("Avatar directory did not exist. Returning (null) and creating directory for future I/O");
				Directory.CreateDirectory(CogitoSharp.Config.AppSettings.AvatarPath);
			}

			Core.allGlobalUsers.Add(this); 
		}

		public User(string nName, int nAge) : this(nName){ this.age = nAge; }

		public override string ToString() { return this.Name; }

		public override bool Equals(object obj){
			if (obj == null) { return false; }
			User o = obj as User;
			//TODO: Needs more complex comparison logic. Whilst character names are unique... wait, DOES it need more complex comparison logic in that case?
			if (this.Name == o.Name) { return true; }
			else { return false; }
		}

		public override int GetHashCode(){ 
			//TODO: make more complex function based on available data (collision old database-derived user and new from web user)
			return this.UID; }

		public bool Equals(User user)
		{
			if (this.Name == user.Name) { return true; }
			else { return false; }
		}

		public int CompareTo(object obj)
		{
			if (obj == null) { return 1; }
			User o = obj as User;
			if (o != null) { return this.Name.CompareTo(o.Name); }
			else { throw new ArgumentException("Object cannot be made into User. Cannot CompareTo()."); }
		}

		/// <summary> Retriedves the users's avatar (100 * 100 px Bitmap image) from F-Lists' server, and saves it. If it's less than a week old, it won't bother fetching it.
		/// Background worker code blatantly inspired by slimCat, do not steal.</summary>
		public void GetAvatar(){
			if (Name == null || ((DateTime.Now - this.avatarTakenOn) <= Config.AppSettings.userProfileRefreshPeriod) ) { return; }
			try{ this.Avatar = (Bitmap)Bitmap.FromFile(Config.AppSettings.AvatarPath + this.Name.ToLowerInvariant() + ".bmp"); }
			catch (FileNotFoundException){
				var worker = new BackgroundWorker();
				worker.DoWork += (s, e) =>
				{
					Uri uri = new Uri(System.Net.WebUtility.HtmlEncode((string)e.Argument), UriKind.Absolute);
					using (WebClient webClient = new WebClient())
					{
						webClient.Proxy = null;
						webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Revalidate);
						try
						{
							var imageBytes = webClient.DownloadData(uri);

							if (imageBytes == null)
							{
								e.Result = null;
								return;
							}

							MemoryStream imageStream = new MemoryStream(imageBytes);
							Bitmap image = new Bitmap(imageStream);

							//if (!Directory.Exists(Config.AppSettings.AvatarPath)){ Directory.CreateDirectory(Config.AppSettings.AvatarPath); }
							image.Save(CogitoSharp.Config.AppSettings.AvatarPath + this.Name.ToLowerInvariant() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp); //let us never web-load this again
							imageStream.Close();
							imageStream.Dispose();

							e.Result = image;
						}
						catch (Exception) { }
					}
				};

				worker.RunWorkerCompleted += (s, e) =>
				{
					var bitmapImage = e.Result as Bitmap;
					if (bitmapImage != null) { Avatar = bitmapImage; }
					//TODO - update avatar in component this.userTab.Container.Components.
					if (((ChatTab)CogitoUI.chatUI.chatTabs.SelectedTab).parent == this) { CogitoUI.chatUI.currenctCharAvatar.Image = this.Avatar; }
					worker.Dispose();
				};

				worker.RunWorkerAsync(Config.URLConstants.CharacterAvatar + Name.ToLower() + ".png");	
			}
		}

		/// <summary> Connects to F-List and grabs all of the user's delicious information. Data is serialized when program shuts down to cut down on API connections / web scraping bandwith </summary>
		public void GetProfileInfo(){
			if ((DateTime.Now - this.dataTakenOn) >= Config.AppSettings.userProfileRefreshPeriod){
				var worker = new BackgroundWorker();
				worker.DoWork += (s, e) =>
				{
					Dictionary<string, object> ProfileData = new Dictionary<string, object>();

					WebRequest wr = WebRequest.Create(CogitoSharp.Config.URLConstants.CharacterInfo);
					WebHeaderCollection RequestData = new WebHeaderCollection();
					RequestData["name"] = this.Name.ToLower();
					if ((TimeSpan)(DateTime.Now - CogitoSharp.Account.LoginKey.ticketTaken) > CogitoSharp.Config.AppSettings.ticketLifetime) {
						//TODO Get new Login Ticket
					}
					RequestData["ticket"] = CogitoSharp.Account.LoginKey.ticket;
					RequestData["account"] = CogitoSharp.Account.LoginKey.account_id.ToString();
					RequestData["warning"] = "1";
					wr.Headers = RequestData;
					Dictionary<string, object> Response = JsonConvert.DeserializeObject<Dictionary<string, object>>(wr.GetResponse().ToString());			
					if (Response["error"].ToString().Length > 0) {
						using (WebClient wc = new WebClient()){
							wc.Headers.Add("warning,1");
							HtmlDocument Profile = new HtmlDocument();
							Profile.LoadHtml(wc.DownloadString(CogitoSharp.Config.URLConstants.ProfileRoot + HttpUtility.HtmlEncode(this.Name)));
							foreach(HtmlNode ProfileItem in Profile.DocumentNode.SelectNodes("div[@class='infodatabox']")){
								Match dataToken = CogitoSharp.Utils.RegularExpressions.ProfileHTMLTags.Match(ProfileItem.ToString());
								Console.WriteLine(dataToken.ToString());
								ProfileData[dataToken.Groups[0].ToString()] = dataToken.Groups[1]; 
							}
						}
					}
					e.Result = ProfileData;
				};

				worker.RunWorkerCompleted += (s, e) =>
				{
					lock (UserLock){
						var Results = e.Result as Dictionary<string, object>;
						if (Results.ContainsKey("age")){ this.age = int.Parse(Utils.RegularExpressions.AgeSearch.Match(Results["age"].ToString()).Groups[0].ToString()); }
						if (Results.ContainsKey("weight")){ this.Weight = int.Parse(Utils.RegularExpressions.AgeSearch.Match(Results["age"].ToString()).Groups[0].ToString()); }
						if (Results.ContainsKey("gender")){ this.Gender = Results["gender"].ToString(); }
						this.Stats = Results; //Catch-all data assignment
						this.dataTakenOn = DateTime.Now; //sets the flag
						//TODO etc etc
						worker.Dispose();
					}
				};
				worker.RunWorkerAsync();
			}
		}

		internal void MessageReceived(IO.Message m){
			if (this.userLog == null) { this.userLog = new IO.Logging.LogFile(this.Name); }
			if (this.userTab == null) { this.userTab = new ChatTab(this); }
			if (!CogitoUI.chatUI.chatTabs.TabPages.Contains(this.userTab)) { CogitoUI.chatUI.chatTabs.TabPages.Add(this.userTab); }
			this.userTab.MessageReceived(m, this.userLog);
		}

		public virtual void Dispose(){
			if (!disposed){
				if (this.userTab != null) { this.userTab.Dispose(); }
				if (this.userLog!= null) { this.userLog.Dispose(); }
			}
			this.disposed = true;
		}
	}
}
