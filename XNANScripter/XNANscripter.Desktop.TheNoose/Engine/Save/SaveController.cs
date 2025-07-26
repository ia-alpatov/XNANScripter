using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Save
{
    public static class SaveController
    {
        public static void Save(string GameName,int SlotId)
        {

        }

        public static void Load(string GameName, int SlotId)
        {

        }

        public static async Task<List<KeyValuePair<int,DateTime>>> GetSlots(string GameName)
        {
            List<KeyValuePair<int, DateTime>> result = new List<KeyValuePair<int, DateTime>>();

            try
            {
                var savesJson = await GetFileContent(GameName + ".json");
                var deserializerSettings = new JsonSerializerSettings()
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset
                };
                var saves = JsonConvert.DeserializeObject<List<Dictionary<string,object>>>(savesJson, deserializerSettings);

                foreach (var save in saves)
                {
                    int nubmer = int.Parse(save["number"].ToString());
                    result.Add(new KeyValuePair<int, DateTime>(nubmer, DateTime.Parse(save["datetime"].ToString()) ));
                }

                for (int i = 1; i <= SaveSystem.SaveNumber; i++)
                {
                    if (result.Count(var=>var.Key == i)<1)
                    {
                        result.Add(new KeyValuePair<int, DateTime>(i, DateTime.MinValue));
                    }
                }
            }
            catch (Exception)
            {

                for (int i = 1; i <= SaveSystem.SaveNumber; i++)
                {
                    result.Add(new KeyValuePair<int, DateTime>(i, DateTime.MinValue));
                }
            }


            return result;
        }
        
        private static async Task<string> GetFileContent(string FileName)
        {
            // reads the contents of file 'filename' in the app's local storage folder and returns it as a string

            // access the local folder
           
            // open the file 'filename' for reading
            using Stream stream =  File.OpenRead(FileName);
            string text;

            // copy the file contents into the string 'text'
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return text;

        }
    }
}
