using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IniIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IniIOTests
{
    [TestClass]
    public class IniFileTest
    {
        [TestMethod]
        public void New_Parameterless_InitializedAndEmpty()
        {
            // Arrange
            IniFile file;

            // Act
            file = new IniFile();

            // Assert
            Assert.IsNotNull(file);
            Assert.AreEqual(0, file.Sections.Count);
        }

        [TestMethod]
        public void AddSection_AddMultipleSections_SectionsAreAdded()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            IniSection section3 = new IniSection("testSection3");

            // Act
            file.Sections.Add(section1);
            file.Sections.Add(section2);
            file.Sections.Add(section3);

            // Assert
            Assert.AreEqual(3, file.Sections.Count);
            Assert.AreSame(section1, file.Sections.First());
            Assert.AreSame(section3, file.Sections.Last());
        }

        [TestMethod]
        public void AddSection_NameExists_OldSectionIsReplaced()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection");
            IniSection section2 = new IniSection("testSection");

            // Act
            file.Sections.Add(section1);
            file.Sections.Add(section2);

            // Assert
            Assert.AreNotSame(section1, file["testSection"]);
            Assert.AreSame(section2, file["testSection"]);
            Assert.AreEqual(1, file.Sections.Count);
        }

        [TestMethod]
        public void AddSection_SectionExists_SectionIsAddedOnlyOnce()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection");
            IniSection section2 = section1;

            // Act
            file.Sections.Add(section1);
            file.Sections.Add(section2);

            // Assert
            Assert.AreSame(section1, file["testSection"]);
            Assert.AreSame(section2, file["testSection"]);
            Assert.AreSame(section1, section2);
            Assert.AreEqual(1, file.Sections.Count);
        }

        [TestMethod]
        public void IndexerGet_IndexNames_SectionsAreIndexed()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            IniSection section3 = new IniSection("testSection3");
            file.Sections.Add(section1);
            file.Sections.Add(section2);
            file.Sections.Add(section3);

            // Act
            IniSection indexedSection1 = file["testSection1"];
            IniSection indexedSection2 = file["testSection2"];
            IniSection indexedSection3 = file["testSection3"];

            // Assert
            Assert.AreSame(section1, indexedSection1);
            Assert.AreSame(section2, indexedSection2);
            Assert.AreSame(section3, indexedSection3);
        }

        [TestMethod]
        public void IndexerGet_IndexWithWeirdCasing_IndexerIsCaseInsensitive()
        {
            // Arrange
            IniFile iniFile = new IniFile();
            IniSection iniSection1 = new IniSection("testSection1");
            IniSection iniSection2 = new IniSection("testSection2");
            IniSection iniSection3 = new IniSection("testSection3");
            iniFile.Sections.Add(iniSection1);
            iniFile.Sections.Add(iniSection2);
            iniFile.Sections.Add(iniSection3);

            // Act
            IniSection indexedSection1 = iniFile["TeStSeCtIoN1"];
            IniSection indexedSection2 = iniFile["TESTSECTION2"];
            IniSection indexedSection3 = iniFile["testsection3"];

            // Assert
            Assert.AreSame(iniSection1, indexedSection1);
            Assert.AreSame(iniSection2, indexedSection2);
            Assert.AreSame(iniSection3, indexedSection3);
        }

        [TestMethod]
        public void IndexerGet_IndexNameThatDoesNotExist_ReturnsNull()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            file.Sections.Add(section1);
            file.Sections.Add(section2);

            // Act
            IniSection indexedSection = file["otherSection"];

            // Assert
            Assert.IsNull(indexedSection);
        }

        [TestMethod]
        public void SectionsIndexerGet_IndexNames_SectionsAreIndexed()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            IniSection section3 = new IniSection("testSection3");
            file.Sections.Add(section1);
            file.Sections.Add(section2);
            file.Sections.Add(section3);

            // Act
            IniSection indexedSection1 = file.Sections["testSection1"];
            IniSection indexedSection2 = file.Sections["testSection2"];
            IniSection indexedSection3 = file.Sections["testSection3"];

            // Assert
            Assert.AreSame(section1, indexedSection1);
            Assert.AreSame(section2, indexedSection2);
            Assert.AreSame(section3, indexedSection3);
        }

        [TestMethod]
        public void RemoveSection_ByNameExists_SectionIsRemoved()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            file.Sections.Add(section1);
            file.Sections.Add(section2);

            // Act
            IniSection removedSection = file.Sections.Remove(section1.Name);

            // Assert
            Assert.AreSame(removedSection, section1);
            Assert.AreEqual("testSection1", removedSection.Name);
            Assert.IsNull(file["testSection1"]);
            Assert.AreEqual(1, file.Sections.Count);
        }

        [TestMethod]
        public void RemoveSection_ByReferenceExists_SectionIsRemoved()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            file.Sections.Add(section1);
            file.Sections.Add(section2);

            // Act
            bool success = file.Sections.Remove(section1);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNull(file["testSection1"]);
            Assert.AreEqual(1, file.Sections.Count);
        }

        [TestMethod]
        public void RemoveSection_ByNameDoesNotExist_ReturnsNull()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            file.Sections.Add(section1);
            file.Sections.Add(section2);

            // Act
            IniSection removedSection = file.Sections.Remove("testSection3");

            // Assert
            Assert.IsNull(removedSection);
            Assert.IsNull(file["testSection3"]);
            Assert.AreEqual(2, file.Sections.Count);
        }

        [TestMethod]
        public void RemoveSection_ByReferenceDoesNotExist_NotSuccess()
        {
            // Arrange
            IniFile file = new IniFile();
            IniSection section1 = new IniSection("testSection1");
            IniSection section2 = new IniSection("testSection2");
            file.Sections.Add(section1);
            file.Sections.Add(section2);

            // Act
            IniSection sectionOther = new IniSection("testSectionOther");
            bool success = file.Sections.Remove(sectionOther);

            // Assert
            Assert.IsFalse(success);
            Assert.IsNull(file["testSectionOther"]);
            Assert.AreEqual(2, file.Sections.Count);
        }
    }
}
