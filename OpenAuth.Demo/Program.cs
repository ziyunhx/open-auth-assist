using System;
using OpenAuth.ConsoleDemo;

namespace OpenAuth.Demo
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Weibo SDK demo");
			var loop = true;
			while (loop)
			{
				Console.WriteLine("1. test sina weibo");
				Console.WriteLine("Q: quit");

				Console.WriteLine("choose one item:");

				var key = Console.ReadKey();

				switch (key.Key)
				{
				case ConsoleKey.D1:
					var sina = new SinaWeiboTest();
					break;
				case ConsoleKey.D2:
					//var tencent = new QQConnectTest();
					break;
				case ConsoleKey.Q:
					loop = false;
					break;
				}
			}
		}
	}
}
