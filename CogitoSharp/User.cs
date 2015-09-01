using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Reflection;
using System.Runtime.InteropServices.Expando;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace CogitoSharp
{
	enum Genders : int { None = 0, Male, Female, Transgender }
	enum Status : int { online = 0, busy, dnd, looking, away }

	/// <summary>User (synonymous with Character)</summary>
	public class User : IComparable
	{
		private static int Count;
		private readonly int UID = ++User.Count;
		private object UserLock;

		public readonly string Name = null;
		private int age = 0;
		public int Age {
			get{
				if (this.age == 0){
					GetProfileInfo();
				}
				return this.age;
			}
		}

		public int Weight;

		internal Genders Gender = Genders.None;
		internal Status status = Status.online;
		internal string Orientation;
		public Dictionary<string, object> Stats;
		public Dictionary<string, List<string>> Kinks;
		internal bool isInteresting;
		internal BitmapImage Avatar;

		public User(string nName) { this.Name = nName; Core.users.Add(this); }

		public User(string nName, int nAge)
		{
			this.Name = nName;
			this.age = nAge;
			Core.users.Add(this);
		}

		public override string ToString() { return this.Name; }

		public override bool Equals(object obj)
		{
			if (obj == null) { return false; }
			User o = obj as User;
			if (this.Name == o.Name) { return true; }
			else { return false; }
		}

		public override int GetHashCode()
		{
			return this.UID;
		}

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

		public void GetAvatar(){
			if (Name == null)
				return;
			var worker = new BackgroundWorker();
			worker.DoWork += (s, e) =>
			{
				var uri = new Uri((string)e.Argument, UriKind.Absolute);

				using (var webClient = new WebClient())
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

						var imageStream = new MemoryStream(imageBytes);
						var image = new BitmapImage();

						image.BeginInit();
						image.StreamSource = imageStream;
						image.CacheOption = BitmapCacheOption.OnLoad;
						image.EndInit();

						//image.Freeze();
						imageStream.Close();

						e.Result = image;
					}
					catch (Exception)
					{
					}
				}
			};

			worker.RunWorkerCompleted += (s, e) =>
			{
				var bitmapImage = e.Result as BitmapImage;
				if (bitmapImage != null)
					Avatar = bitmapImage;

				worker.Dispose();
			};

			worker.RunWorkerAsync(Config.URLConstants.CharacterAvatar + Name.ToLower() + ".png");
		}

		public void GetProfileInfo(){
			var worker = new BackgroundWorker();
			worker.DoWork += (s, e) =>
			{
				Dictionary<string, object> ProfileData = new Dictionary<string, object>();

				WebRequest wr = WebRequest.Create(CogitoSharp.Config.URLConstants.CharacterInfo);
				WebHeaderCollection RequestData = new WebHeaderCollection();
				RequestData["name"] = this.Name.ToLower();
				if ((TimeSpan)(DateTime.UtcNow - CogitoSharp.Account.LoginKey.ticketTaken) > CogitoSharp.Config.AppSettings.ticketLifetime) {  }
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
					if (Results.ContainsKey("gender")){ this.Gender = (Genders)Enum.Parse(typeof(Genders), Results["gender"].ToString()); }
					//TODO etc etc
					worker.Dispose();
				}
			};

			worker.RunWorkerAsync(Config.URLConstants.CharacterAvatar + Name.ToLower() + ".png");
		}
	}
}
