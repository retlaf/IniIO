using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IniIO
{
    public class IniWriter
    {
        private IniWriter() { }

        public static void Write(IniFile iniFile, string filename)
        {
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                Write(iniFile, streamWriter);
            }
        }

        public static void Write(IniFile iniFile, TextWriter textWriter)
        {
            foreach (IniSection section in iniFile.Sections)
            {
                if (section != iniFile.Sections.First())
                {
                    textWriter.WriteLine();
                }

                if (section.Comments.Count > 0)
                {
                    foreach (string comment in section.Comments)
                    {
                        textWriter.WriteLine(";"  + comment);
                    }
                }

                textWriter.WriteLine("[" + section.Name + "]");

                foreach (IniKeyValue keyValue in section.KeyValues)
                {
                    if (keyValue.Comments.Count > 0)
                    {
                        foreach (string comment in keyValue.Comments)
                        {
                            textWriter.WriteLine(";" + comment);
                        }
                    }

                    textWriter.WriteLine(keyValue.Key + "=" + keyValue.Value);
                }
            }
        }
    }
}
