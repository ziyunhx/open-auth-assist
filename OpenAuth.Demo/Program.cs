﻿using System;
using OpenAuth.ConsoleDemo;

namespace OpenAuth.Demo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Weibo SDK demo");
            var loop = true;
            while (loop)
            {
                Console.WriteLine("1. test sina weibo");
                Console.WriteLine("Q: quit");

                Console.WriteLine("choose one item:");

                string key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        var sina = new SinaWeiboTest();
                        break;
                    case "2":
                        //var tencent = new QQConnectTest();
                        break;
                    case "Q":
                    case "q":
                        loop = false;
                        break;
                }
            }
        }
    }
}
