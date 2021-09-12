using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Model
{
    static class FileHasla
    {

        private static readonly string filePath = Environment.CurrentDirectory.Replace(@"bin\Debug", @"hasla\");

        public static bool FileExists(string fileName)
        {
            return File.Exists(Path.Combine(filePath,fileName,".txt"));
        }

        public static void CreateNewFile()
        {
            File.AppendAllText(Path.Combine(filePath, @"nowy.txt"), "programista, chop, chłop, wisiolec");
        }

        public static List<string> GetAvailableWords(string fileName)
        {
            var textFile = File.ReadAllText(Path.Combine(filePath, fileName) + ".txt");

            if(string.IsNullOrWhiteSpace(textFile))
            {
                throw new Exception("Pusty plik.");
            }

            return textFile.Split(',').Select(x => x.Trim()).ToList();
        }














    }
}
