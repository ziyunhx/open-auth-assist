using OpenAuth.Assist.Client;
using System;
using System.Collections.Generic;

namespace OpenAuth.ConsoleDemo
{
    class SinaWeiboTest
	{
		public SinaWeiboTest()
		{
			Console.WriteLine("Waitting for user authentication...");

			var openAuth = new SinaWeiboClient("1402038860", "62e1ddd4f6bc33077c796d5129047ca2", "http://qcyn.sina.com.cn");

            openAuth.DoLogin("46248069@qq.com", "3792500");
		}

		private void StartTest(SinaWeiboClient openAuth)
		{
			Console.WriteLine("Press enter to post a weibo.");
			if(Console.ReadKey(true).Key == ConsoleKey.Enter)
			    PostStatus(openAuth);

			Console.WriteLine("Press enter to get user's timeline.");
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                GetFrindTimeline(openAuth);
		}

		private void GetFrindTimeline(SinaWeiboClient openAuth)
		{
			Console.WriteLine("get user's timeline...");

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
				Console.WriteLine("success！");
			}
		}

		private void PostStatus(SinaWeiboClient openAuth)
		{
			Console.WriteLine("post a weibo...");

			var result = openAuth.HttpPost("statuses/update.json", new Dictionary<string, object>
			{
				{"status" , string.Format("post from OpenAuth.Assist! @{0:HH:mm:ss}", DateTime.Now)}
			});

			Console.WriteLine("{0}", result);
			if (result.IsSuccessStatusCode)
			{
				Console.WriteLine(result.Content.ReadAsStringAsync().Result);
				Console.WriteLine("success！");
			}
		}
	}
}
