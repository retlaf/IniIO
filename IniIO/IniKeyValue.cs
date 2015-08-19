using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniIO
{
    public class IniKeyValue
    {
        private string _key;
        private string _value;
        private List<string> _comments;

        public IniKeyValue(string key, string value)
        {
            _key = key;
            _value = value;
            _comments = new List<string>();
        }

        public string Key
        {
            get { return _key; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public List<string> Comments
        {
            get { return _comments; }
        }
    }
}
