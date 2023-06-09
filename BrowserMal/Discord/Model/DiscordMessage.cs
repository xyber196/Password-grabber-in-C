﻿using System.Collections.Generic;
using System.Reflection;

namespace BrowserMal.Discord.Model
{
    [Obfuscation(ApplyToMembers = false)]
    public class DiscordMessage
    {
        public string username { get; set; }
        public string avatar_url { get; set; }
        public string content { get; set; }
        //public AllowedMentions allowed_mentions { get; set; }

        public List<Embed> embeds { get; set; }

        public DiscordMessage(string username, string content = "")
        {
            this.username = username;
            this.content = content;

            embeds = new List<Embed>();
        }
    }
}
