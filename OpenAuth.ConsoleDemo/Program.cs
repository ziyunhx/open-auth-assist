using System;

namespace OpenAuth.ConsoleDemo
{
    class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			console.key("Weibo SDK demo");
			var loop = true;
			while (loop)
			{
				console.tip("1. test sina weibo");
				//console.tip("2. test tencent weibo");
				console.tip("Q: quit");

				Console.Write("choose one item:");

				var key = Console.ReadKey();
				console.log();

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


	public static class console
	{
		public static void log()
		{
			Console.WriteLine();
		}

		public static void log(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				Console.WriteLine(msg, args);
			}
			else
			{
				Console.WriteLine(msg);
			}

		}
		public static void warn(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			log(msg, args);

			Console.ResetColor();
		}

		public static void error(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			log(msg, args);
			Console.ResetColor();
		}

		public static void info(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			log(msg, args);
			Console.ResetColor();
		}

		public static void tip(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			log(msg, args);
			Console.ResetColor();
		}

		public static void attention(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			log(msg, args);
			Console.ResetColor();
		}

		public static void key(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.White;
			log(msg, args);
			Console.ResetColor();
		}

		public static void data(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			log(msg, args);
			Console.ResetColor();
		}
	}
}
