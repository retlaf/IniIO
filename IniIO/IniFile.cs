using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniIO
{
    public class IniFile
    {
        public IniFile()
        {
            Sections = new IniSectionCollection();
        }

        public IniSection this[string name]
        {
            get { return Sections[name]; }
        }

        public IniSectionCollection Sections { get; set; }
    }
}
