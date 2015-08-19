using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniIO
{
    public class IniSectionCollection : IEnumerable<IniSection>
    {
        private List<IniSection> _sections;

        public IniSectionCollection()
        {
            _sections = new List<IniSection>();
        }

        public IniSection this[string name]
        {
            get
            {
                foreach (IniSection section in _sections)
                {
                    if (section.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return section;
                    }
                }

                return null;
            }
        }

        public int Count
        {
            get { return _sections.Count; }
        }

        public void Add(IniSection newSection)
        {
            IniSection existingSection = this[newSection.Name];
            if (existingSection != null) Remove(newSection.Name);

            _sections.Add(newSection);
        }

        public bool Remove(IniSection section)
        {
            return _sections.Remove(section);
        }

        public IniSection Remove(string name)
        {
            IniSection sectionToRemove = null;

            foreach (IniSection section in _sections)
            {
                if (section.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    sectionToRemove = section;
                    break;
                }
            }

            if (sectionToRemove != null) _sections.Remove(sectionToRemove);
            return sectionToRemove;
        }

        public IEnumerator<IniSection> GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
