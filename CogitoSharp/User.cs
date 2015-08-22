using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using System.Windows.Markup;

namespace CogitoSharp
{
	enum Genders : int { None = 0, Male, Female, Transgender }
	enum Status : int { online = 0, busy, dnd, looking, away }

	/// <summary>User (synonymous with Character)</summary>
	public class User : IComparable
	{
		private readonly int UID = ++User.Count;
		private static int Count;
		public readonly string Name = null;
		public readonly int Age = 0;
		private readonly Genders Gender = Genders.None;
		internal Status status = Status.online;
		public Dictionary<string, object> Stats;
		public Dictionary<string, List<string>> Kinks;
		internal bool isInteresting;
		internal BitmapImage Avatar;

		public User(string nName) { this.Name = nName; }

		public User(string nName, int nAge)
		{
			this.Name = nName;
			this.Age = nAge;
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

		public void GetAvatar()
		{
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
	}
}
