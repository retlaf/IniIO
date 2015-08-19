using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniIO.Impl
{
    class IniLineProcessor
    {
        private IniFile _iniFile;
        private IniSection _currentSection;
        private IniKeyValue _currentKeyValue;
        private List<string> _currentComments;

        public IniLineProcessor(IniFile iniFile)
        {
            _iniFile = iniFile;
            _currentSection = null;
            _currentKeyValue = null;
            _currentComments = new List<string>();
        }

        public void ProcessLine(string line)
        {
            line = line.Trim();

            if (IsSection(line))
            {
                _currentSection = ProcessSection(line, _iniFile);
                _currentSection.Comments.AddRange(_currentComments);
                _currentComments.Clear();
            }
            else if (IsKeyValue(line))
            {
                if (_currentSection == null)
                {
                    _currentSection = new IniSection(string.Empty);
                    _iniFile.Sections.Add(_currentSection);
                }

                _currentKeyValue = ProcessKeyValue(line, _currentSection);
                _currentKeyValue.Comments.AddRange(_currentComments);
                _currentComments.Clear();
            }
            else if (IsComment(line))
            {
                string comment = ProcessComment(line, _currentKeyValue);
                _currentComments.Add(comment);
            }
            else
            {
                // Blank (or junk) line. Ignore.
            }
        }

        private bool IsSection(string line)
        {
            return line.StartsWith("[") && line.EndsWith("]");
        }

        private bool IsKeyValue(string line)
        {
            return line.Contains('=');
        }

        private bool IsComment(string line)
        {
            return line.StartsWith(";") || line.StartsWith("#");
        }

        private IniSection ProcessSection(string line, IniFile iniFile)
        {
            string name = line.Substring(1, line.Length - 2);
            IniSection section = new IniSection(name);
            iniFile.Sections.Add(section);
            return section;
        }

        private IniKeyValue ProcessKeyValue(string line, IniSection section)
        {
            string[] split = line.Split('=');
            IniKeyValue keyValue = new IniKeyValue(split[0].Trim(), split[1].Trim());
            section.KeyValues.Add(keyValue);
            return keyValue;
        }

        private string ProcessComment(string line, IniKeyValue keyValue)
        {
            string comment = line.Remove(0, 1).Trim();
            return comment;
        }
    }
}
