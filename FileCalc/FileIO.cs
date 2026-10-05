using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileCalc
{
    internal class FileIO
    {
        public static void WriteToFile(string filename, string text)
        {
            using (StreamWriter streamWriter = new StreamWriter(filename, false))
            {
                streamWriter.Write(text);
            }
        }
        public static string ReadFromFile(string filename)
        {
            using (StreamReader streamReader = new StreamReader(filename))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
