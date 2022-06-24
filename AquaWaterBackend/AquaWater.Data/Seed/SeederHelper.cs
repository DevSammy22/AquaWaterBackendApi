using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AquaWater.Data.Seed
{
    public class SeederHelper<T>
    {
        public static List<T> GetData(string filePath)
        {
            var baseDir = Directory.GetCurrentDirectory();
            var path2 = File.ReadAllText(FilePath(baseDir, filePath));
            return JsonConvert.DeserializeObject<List<T>>(path2);
        }

        private static string FilePath(string folderName, string fileName)
        {
            folderName += "/Json/";
            return Path.Combine(folderName, fileName);
        }
    }
}