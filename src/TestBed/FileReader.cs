
using PhoenixSystem.Monogame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Sample
{
    public class FileReader : IFileReader
    {
        public string[] GetFileContents(string Path)
        {
            return File.ReadAllLines(Path);
        }
    }
}
