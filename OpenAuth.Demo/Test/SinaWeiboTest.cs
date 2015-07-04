using OpenAuth.Assist.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenAuth.ConsoleDemo
{
    class SinaWeiboTest
	{
		public SinaWeiboTest()
		{
			Console.WriteLine("等待用户授权...");

			var openAuth = new SinaWeiboClient("1402038860", "62e1ddd4f6bc33077c796d5129047ca2", "http://qcyn.sina.com.cn");
		}

		private void StartTest(SinaWeiboClient openAuth)
		{
			Console.WriteLine("按任意键发布一条文字微博");
			Console.ReadKey(true);
			PostStatus(openAuth);

			Console.WriteLine("按任意键发布一条图片微博");
			Console.ReadKey(true);
			PostImageStatus(openAuth);

			Console.WriteLine("按任意键获取最新微博");
			Console.ReadKey(true);
			GetFrindTimeline(openAuth);
		}

		private void GetFrindTimeline(SinaWeiboClient openAuth)
		{
			Console.WriteLine("获取当前登录用户及其所关注用户的最新微博...");


			var result = openAuth.HttpGet("statuses/friends_timeline.json", new Dictionary<string, object>
			{
				{"count", 5},
				{"page", 1},
				{"base_app" , 0}
			});
			Console.WriteLine("{0}", result);

			if (result.IsSuccessStatusCode)
			{

				Console.WriteLine(result.Content.ReadAsStringAsync().Result);
				Console.WriteLine("获取成功！");
			}

		}

		private void PostImageStatus(SinaWeiboClient openAuth)
		{
			Console.WriteLine("发布一条图片微博...");

			var imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MrJSON-Production.png");
			var result = openAuth.HttpPost("statuses/upload.json", new Dictionary<string, object> {

				{"status" , string.Format("发布自SinaWeiboSDK_V3@{0:HH:mm:ss}", DateTime.Now)},
				{"pic" , new FileInfo(imgPath)}
			});

			Console.WriteLine("{0}", result);


			if (result.IsSuccessStatusCode)
			{

				Console.WriteLine(result.Content.ReadAsStringAsync().Result);
				Console.WriteLine("发布成功！");
			}
		}

		private void PostStatus(SinaWeiboClient openAuth)
		{
			Console.WriteLine("发布一条微博...");


			var result = openAuth.HttpPost("statuses/update.json", new Dictionary<string, object>
			{
				{"status" , string.Format("发布自SinaWeiboSDK_V3@{0:HH:mm:ss}", DateTime.Now)}
			});

			Console.WriteLine("{0}", result);
			if (result.IsSuccessStatusCode)
			{

				Console.WriteLine(result.Content.ReadAsStringAsync().Result);
				Console.WriteLine("发布成功！");
			}
		}
	}
}
