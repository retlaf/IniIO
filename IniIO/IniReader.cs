using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IniIO.Impl;

namespace IniIO
{
    public class IniReader
    {
        private IniReader() { }

        public static IniFile Open(string filename)
        {
            using (StreamReader streamReader = new StreamReader(filename))
            {
                return Open(streamReader);
            }
        }

        public static IniFile Open(TextReader textReader)
        {
            IniFile iniFile = new IniFile();
            IniLineProcessor iniLineProcessor = new IniLineProcessor(iniFile);

            string line;
            while ((line = textReader.ReadLine()) != null)
            {
                iniLineProcessor.ProcessLine(line);
            }

            return iniFile;
        }
    }
}
