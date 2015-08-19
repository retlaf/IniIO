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
    public class IniReaderTest
    {
        [TestMethod]
        public void Open_NormalFile()
        {
            // Arrange
            IniFile file;
            StringBuilder fileText = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                stringWriter.WriteLine("[Fruit]");
                stringWriter.WriteLine("apple=red");
                stringWriter.WriteLine("orange=orange");
                stringWriter.WriteLine("banana=yellow");
                stringWriter.WriteLine();
                stringWriter.WriteLine("[Veggie]");
                stringWriter.WriteLine("tomato=red");
                stringWriter.WriteLine("zucchini=green");
                stringWriter.WriteLine("cucumber=green");
            }

            // Act
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                file = IniReader.Open(stringReader);
            }

            // Assert
            Assert.IsNotNull(file["Fruit"]);
            Assert.AreEqual("red", file["Fruit"]["apple"]);
            Assert.AreEqual("orange", file["Fruit"]["orange"]);
            Assert.AreEqual("yellow", file["Fruit"]["banana"]);
            Assert.IsNotNull(file["Veggie"]);
            Assert.AreEqual("red", file["Veggie"]["tomato"]);
            Assert.AreEqual("green", file["Veggie"]["zucchini"]);
            Assert.AreEqual("green", file["Veggie"]["cucumber"]);
        }

        [TestMethod]
        public void Open_FileWithReasonableBlankSpaces()
        {
            // Arrange
            IniFile file;
            StringBuilder fileText = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                stringWriter.WriteLine("[Fruit]");
                stringWriter.WriteLine("apple = red");
                stringWriter.WriteLine("orange = orange");
                stringWriter.WriteLine("banana = yellow");
                stringWriter.WriteLine();
                stringWriter.WriteLine("[Veggie]");
                stringWriter.WriteLine("  tomato  =  red");
                stringWriter.WriteLine("  zucchini  =  green");
                stringWriter.WriteLine("  cucumber  =  green");
            }

            // Act
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                file = IniReader.Open(stringReader);
            }

            // Assert
            Assert.IsNotNull(file["Fruit"]);
            Assert.AreEqual("red", file["Fruit"]["apple"]);
            Assert.AreEqual("orange", file["Fruit"]["orange"]);
            Assert.AreEqual("yellow", file["Fruit"]["banana"]);
            Assert.IsNotNull(file["Veggie"]);
            Assert.AreEqual("red", file["Veggie"]["tomato"]);
            Assert.AreEqual("green", file["Veggie"]["zucchini"]);
            Assert.AreEqual("green", file["Veggie"]["cucumber"]);
        }

        [TestMethod]
        public void Open_FileWithOddBlankSpaces()
        {
            // Arrange
            IniFile file;
            StringBuilder fileText = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                stringWriter.WriteLine();
                stringWriter.WriteLine();
                stringWriter.WriteLine("   [Fruit] ");
                stringWriter.WriteLine();
                stringWriter.WriteLine("apple=red   ");
                stringWriter.WriteLine();
                stringWriter.WriteLine("   orange=orange");
                stringWriter.WriteLine("   banana=yellow   ");
                stringWriter.WriteLine("[Veggie]   ");
                stringWriter.WriteLine();
                stringWriter.WriteLine();
                stringWriter.WriteLine("   tomato=red ");
                stringWriter.WriteLine(" zucchini=green   ");
                stringWriter.WriteLine();
                stringWriter.WriteLine(" cucumber=green ");
                stringWriter.WriteLine();
            }

            // Act
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                file = IniReader.Open(stringReader);
            }

            // Assert
            Assert.IsNotNull(file["Fruit"]);
            Assert.AreEqual("red", file["Fruit"]["apple"]);
            Assert.AreEqual("orange", file["Fruit"]["orange"]);
            Assert.AreEqual("yellow", file["Fruit"]["banana"]);
            Assert.IsNotNull(file["Veggie"]);
            Assert.AreEqual("red", file["Veggie"]["tomato"]);
            Assert.AreEqual("green", file["Veggie"]["zucchini"]);
            Assert.AreEqual("green", file["Veggie"]["cucumber"]);
        }

        [TestMethod]
        public void Open_IndexesThatDontExistAreNull()
        {
            // Arrange
            IniFile file;
            StringBuilder fileText = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                stringWriter.WriteLine("[Fruit]");
                stringWriter.WriteLine("apple=red");
                stringWriter.WriteLine("orange=orange");
                stringWriter.WriteLine("banana=yellow");
            }

            // Act
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                file = IniReader.Open(stringReader);
            }

            // Assert
            Assert.IsNotNull(file["Fruit"]);
            Assert.IsNull(file["Grains"]);
            Assert.IsNotNull(file["Fruit"]["apple"]);
            Assert.IsNull(file["Fruit"]["peach"]);
        }

        [TestMethod]
        public void Open_EmptyKeysAndOrValues()
        {
            // Arrange
            IniFile file;
            StringBuilder fileText = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                stringWriter.WriteLine("[Fruit]");
                stringWriter.WriteLine("apple=");
                stringWriter.WriteLine("=orange");
                stringWriter.WriteLine();
                stringWriter.WriteLine("[Veggie]");
                stringWriter.WriteLine("=");
            }

            // Act
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                file = IniReader.Open(stringReader);
            }

            // Assert
            Assert.IsNotNull(file["Fruit"]);
            Assert.AreEqual(string.Empty, file["Fruit"]["apple"]);
            Assert.AreEqual("orange", file["Fruit"][string.Empty]);
            Assert.AreEqual(string.Empty, file["Veggie"][string.Empty]);
        }

        [TestMethod]
        public void Open_EmptySections()
        {
            // Arrange
            IniFile file;
            StringBuilder fileText = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                stringWriter.WriteLine("[]");
                stringWriter.WriteLine("apple=red");
            }

            // Act
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                file = IniReader.Open(stringReader);
            }

            // Assert
            Assert.IsNotNull(file[string.Empty]);
            Assert.AreEqual("red", file[string.Empty]["apple"]);
        }

        [TestMethod]
        public void Open_FileWithComments()
        {
            // Arrange
            IniFile file;
            StringBuilder fileText = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(fileText))
            {
                stringWriter.WriteLine("; Healthy, but full of sugar.");
                stringWriter.WriteLine("[Fruit]");
                stringWriter.WriteLine("apple=red");
                stringWriter.WriteLine("orange=orange");
                stringWriter.WriteLine(";Why isn't it called a yellow?");
                stringWriter.WriteLine(";Probably because that would be weird.");
                stringWriter.WriteLine("banana=yellow");
                stringWriter.WriteLine();
                stringWriter.WriteLine("; Key to a balanced lifestyle.");
                stringWriter.WriteLine("[Veggie]");
                stringWriter.WriteLine(";Is it really a vegetable though?");
                stringWriter.WriteLine("tomato=red");
                stringWriter.WriteLine("zucchini=green");
                stringWriter.WriteLine("cucumber=green");
            }

            // Act
            using (StringReader stringReader = new StringReader(fileText.ToString()))
            {
                file = IniReader.Open(stringReader);
            }

            // Assert
            Assert.IsNotNull(file["Fruit"]);
            Assert.AreEqual("red", file["Fruit"]["apple"]);
            Assert.AreEqual("orange", file["Fruit"]["orange"]);
            Assert.AreEqual("yellow", file["Fruit"]["banana"]);
            Assert.IsNotNull(file["Veggie"]);
            Assert.AreEqual("red", file["Veggie"]["tomato"]);
            Assert.AreEqual("green", file["Veggie"]["zucchini"]);
            Assert.AreEqual("green", file["Veggie"]["cucumber"]);

            Assert.AreEqual(1, file["Fruit"].Comments.Count);
            Assert.AreEqual(2, file["Fruit"].KeyValues["banana"].Comments.Count);
            Assert.AreEqual(1, file["Veggie"].Comments.Count);
            Assert.AreEqual(1, file["Veggie"].KeyValues["tomato"].Comments.Count);

            Assert.AreEqual("Healthy, but full of sugar.", file["Fruit"].Comments[0]);
            Assert.AreEqual("Why isn't it called a yellow?", file["Fruit"].KeyValues["banana"].Comments[0]);
            Assert.AreEqual("Probably because that would be weird.", file["Fruit"].KeyValues["banana"].Comments[1]);
            Assert.AreEqual("Key to a balanced lifestyle.", file["Veggie"].Comments[0]);
            Assert.AreEqual("Is it really a vegetable though?", file["Veggie"].KeyValues["tomato"].Comments[0]);
        }
    }
}
