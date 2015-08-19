using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IniIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IniIOTests
{
    [TestClass]
    public class IniWriterTest
    {
        [TestMethod]
        public void Write_NormalFile()
        {
            // Arrange
            IniFile file = new IniFile();
            StringBuilder fileText = new StringBuilder();

            file.Sections.Add(new IniSection("Fruit"));
            file["Fruit"].KeyValues.Add("apple", "red");
            file["Fruit"].KeyValues.Add("orange", "orange");
            file["Fruit"].KeyValues.Add("banana", "yellow");
            file.Sections.Add(new IniSection("Veggie"));
            file["Veggie"].KeyValues.Add("tomato", "red");
            file["Veggie"].KeyValues.Add("zucchini", "green");
            file["Veggie"].KeyValues.Add("cucumber", "green");

            // Act
            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                IniWriter.Write(file, stringWriter);
            }

            // Assert
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                Assert.AreEqual("[Fruit]", stringReader.ReadLine());
                Assert.AreEqual("apple=red", stringReader.ReadLine());
                Assert.AreEqual("orange=orange", stringReader.ReadLine());
                Assert.AreEqual("banana=yellow", stringReader.ReadLine());
                Assert.AreEqual(string.Empty, stringReader.ReadLine());
                Assert.AreEqual("[Veggie]", stringReader.ReadLine());
                Assert.AreEqual("tomato=red", stringReader.ReadLine());
                Assert.AreEqual("zucchini=green", stringReader.ReadLine());
                Assert.AreEqual("cucumber=green", stringReader.ReadLine());
            }
        }

        [TestMethod]
        public void Write_EmptyKeysAndOrValues()
        {
            // Arrange
            IniFile file = new IniFile();
            StringBuilder fileText = new StringBuilder();

            file.Sections.Add(new IniSection("Fruit"));
            file["Fruit"].KeyValues.Add(string.Empty, "red");
            file["Fruit"].KeyValues.Add("orange", string.Empty);
            file.Sections.Add(new IniSection("Veggie"));
            file["Veggie"].KeyValues.Add(string.Empty, string.Empty);

            // Act
            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                IniWriter.Write(file, stringWriter);
            }

            // Assert
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                Assert.AreEqual("[Fruit]", stringReader.ReadLine());
                Assert.AreEqual("=red", stringReader.ReadLine());
                Assert.AreEqual("orange=", stringReader.ReadLine());
                Assert.AreEqual(string.Empty, stringReader.ReadLine());
                Assert.AreEqual("[Veggie]", stringReader.ReadLine());
                Assert.AreEqual("=", stringReader.ReadLine());
            }
        }

        [TestMethod]
        public void Write_EmptySections()
        {
            // Arrange
            IniFile file = new IniFile();
            StringBuilder fileText = new StringBuilder();

            file.Sections.Add(new IniSection("Fruit"));
            file["Fruit"].KeyValues.Add("apple", "red");
            file["Fruit"].KeyValues.Add("orange", "orange");
            file.Sections.Add(new IniSection(string.Empty));
            file[string.Empty].KeyValues.Add("tomato", "red");
            file[string.Empty].KeyValues.Add("zucchini", "green");

            // Act
            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                IniWriter.Write(file, stringWriter);
            }

            // Assert
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                Assert.AreEqual("[Fruit]", stringReader.ReadLine());
                Assert.AreEqual("apple=red", stringReader.ReadLine());
                Assert.AreEqual("orange=orange", stringReader.ReadLine());
                Assert.AreEqual(string.Empty, stringReader.ReadLine());
                Assert.AreEqual("[]", stringReader.ReadLine());
                Assert.AreEqual("tomato=red", stringReader.ReadLine());
                Assert.AreEqual("zucchini=green", stringReader.ReadLine());
            }
        }

        [TestMethod]
        public void Write_FileWithComments()
        {
            // Arrange
            IniFile file = new IniFile();
            StringBuilder fileText = new StringBuilder();

            file.Sections.Add(new IniSection("Fruit"));
            file["Fruit"].KeyValues.Add("apple", "red");
            file["Fruit"].KeyValues.Add("orange", "orange");
            file["Fruit"].KeyValues.Add("banana", "yellow");
            file.Sections.Add(new IniSection("Veggie"));
            file["Veggie"].KeyValues.Add("tomato", "red");
            file["Veggie"].KeyValues.Add("zucchini", "green");
            file["Veggie"].KeyValues.Add("cucumber", "green");

            file["Fruit"].Comments.Add("Something about fruit.");
            file["Fruit"].KeyValues["apple"].Comments.Add("Something about apples.");
            file["Fruit"].KeyValues["apple"].Comments.Add("Something else about apples.");
            file["Veggie"].Comments.Add("Something about veggies.");
            file["Veggie"].Comments.Add("Something else about veggies.");
            file["Veggie"].Comments.Add("Even more about veggies!");
            file["Veggie"].KeyValues["zucchini"].Comments.Add("Something about zucchini.");

            // Act
            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                IniWriter.Write(file, stringWriter);
            }

            // Assert
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                Assert.AreEqual(";Something about fruit.", stringReader.ReadLine());
                Assert.AreEqual("[Fruit]", stringReader.ReadLine());
                Assert.AreEqual(";Something about apples.", stringReader.ReadLine());
                Assert.AreEqual(";Something else about apples.", stringReader.ReadLine());
                Assert.AreEqual("apple=red", stringReader.ReadLine());
                Assert.AreEqual("orange=orange", stringReader.ReadLine());
                Assert.AreEqual("banana=yellow", stringReader.ReadLine());
                Assert.AreEqual(string.Empty, stringReader.ReadLine());
                Assert.AreEqual(";Something about veggies.", stringReader.ReadLine());
                Assert.AreEqual(";Something else about veggies.", stringReader.ReadLine());
                Assert.AreEqual(";Even more about veggies!", stringReader.ReadLine());
                Assert.AreEqual("[Veggie]", stringReader.ReadLine());
                Assert.AreEqual("tomato=red", stringReader.ReadLine());
                Assert.AreEqual(";Something about zucchini.", stringReader.ReadLine());
                Assert.AreEqual("zucchini=green", stringReader.ReadLine());
                Assert.AreEqual("cucumber=green", stringReader.ReadLine());
            }
        }
    }
}
