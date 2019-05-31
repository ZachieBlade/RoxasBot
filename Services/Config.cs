using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace RoxasBot.Services
{
    public class Config
    {
        /// <summary>
        /// Make the bot instead load data from this CS file instead of config.json
        /// </summary>
        public static bool UseFile = true;

        /// <summary>
        /// If you'd rather use this CS file as config than json fill in the details here and set UseFile to false.
        /// </summary>
        internal static Config LoadFromCS()
        {
            return new Config
            {
                Token = "",
                DbName = "",
                DbUser = "",
                DbPass = "",
                DbServer = "",
                Prefix = ""
            };
        }

        /// <summary>
        /// Your bot's token.
        /// </summary>
        [JsonProperty("token")]
        internal string Token = "Your token..";

        /// <summary>
        /// Your bot's prefix
        /// </summary>
        [JsonProperty("prefix")]
        internal string Prefix = "'";

        /// <summary>
        /// Your bot's database server adress
        /// </summary>
        [JsonProperty("DbServer")]
        public string DbServer = "192.0.0.1";

        /// <summary>
        /// Your bot's database user
        /// </summary>
        [JsonProperty("DbUser")]
        public string DbUser = "IndigoBot";

        /// <summary>
        /// Your database table
        /// </summary>
        [JsonProperty("DbName")]
        public string DbName = "league";

        /// <summary>
        /// Your bot's database user's password
        /// </summary>
        [JsonProperty("DbPass")]
        public string DbPass = "12345";

        /// <summary>
        /// Your favourite color.
        /// </summary>
        [JsonProperty("color")]
        private readonly string _color = "#7289DA";

        /// <summary>
        /// Your favourite color exposed as a DiscordColor object.
        /// </summary>
        internal DiscordColor Color => new DiscordColor(_color);

        /// <summary>
        /// Loads config from a JSON file.
        /// </summary>
        /// <param name="path">Path to your config file.</param>
        /// <returns></returns>
        public static Config LoadFromFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// Saves config to a JSON file.
        /// </summary>
        /// <param name="path"></param>
        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(JsonConvert.SerializeObject(this));
            }
        }
    }
}
