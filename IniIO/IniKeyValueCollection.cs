using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniIO
{
    public class IniKeyValueCollection : IEnumerable<IniKeyValue>
    {
        private List<IniKeyValue> _keyValues;

        public IniKeyValueCollection()
        {
            _keyValues = new List<IniKeyValue>();
        }

        public IniKeyValue this[string key]
        {
            get
            {
                foreach (IniKeyValue keyValue in _keyValues)
                {
                    if (keyValue.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        return keyValue;
                    }
                }

                return null;
            }
        }

        public int Count
        {
            get { return _keyValues.Count; }
        }

        public void Add(string key, string value)
        {
            Add(new IniKeyValue(key, value));
        }

        public void Add(IniKeyValue keyValue)
        {
            IniKeyValue existingKeyValue = this[keyValue.Key];
            if (existingKeyValue != null) Remove(keyValue.Key);

            _keyValues.Add(keyValue);
        }

        public bool Remove(IniKeyValue keyValue)
        {
            return _keyValues.Remove(keyValue);
        }

        public IniKeyValue Remove(string key)
        {
            IniKeyValue keyValueToRemove = null;

            foreach (IniKeyValue keyValue in _keyValues)
            {
                if (keyValue.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    keyValueToRemove = keyValue;
                    break;
                }
            }

            if (keyValueToRemove != null) _keyValues.Remove(keyValueToRemove);
            return keyValueToRemove;
        }

        public IEnumerator<IniKeyValue> GetEnumerator()
        {
            return _keyValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
