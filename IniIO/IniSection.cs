using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniIO
{
    public class IniSection
    {
        public IniSection(string name)
        {
            _name = name;
            KeyValues = new IniKeyValueCollection();
            Comments = new List<string>();
        }

        public string this[string key]
        {
            get
            {
                IniKeyValue keyValue = KeyValues[key];
                if (keyValue == null) return null;
                else return keyValue.Value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public IniKeyValueCollection KeyValues { get; set; }
        public List<string> Comments { get; set; }

        public string GetValueOrEmpty(string key)
        {
            string value = this[key];
            if (value == null) return string.Empty;
            else return value;
        }
    }
}
