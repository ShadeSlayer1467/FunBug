using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunBugTutorialDiscordBot.config
{
    public class JSONReader
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public ulong BasicGuildID { get; set; }

        public async Task ReadConfigJSON()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();
                ConfigJSONStructure data = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigJSONStructure>(json);

                this.token = data.token;
                this.prefix = data.prefix;
                this.BasicGuildID = data.basicGuildID;
            }
        }
    }
    internal sealed class ConfigJSONStructure
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public ulong basicGuildID { get; set;}
    }
}
