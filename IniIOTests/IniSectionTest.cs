using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using IniIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IniIOTests
{
    [TestClass]
    public class IniSectionTest
    {
        [TestMethod]
        public void New_SetName_InitalizedWithNameAndEmpty()
        {
            // Arrange
            IniSection section;

            // Act
            section = new IniSection("testSection");

            // Assert
            Assert.IsNotNull(section);
            Assert.AreEqual("testSection", section.Name);
            Assert.AreEqual(0, section.KeyValues.Count);
        }

        [TestMethod]
        public void AddKeyValue_AddMultipleKeyValues_KeyValuesAreAdded()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            IniKeyValue keyValue1 = new IniKeyValue("testKey1", "testValue1");
            IniKeyValue keyValue2 = new IniKeyValue("testKey2", "testValue2");
            IniKeyValue keyValue3 = new IniKeyValue("testKey3", "testValue3");

            // Act
            section.KeyValues.Add(keyValue1);
            section.KeyValues.Add(keyValue2);
            section.KeyValues.Add(keyValue3);

            // Assert
            Assert.AreEqual(3, section.KeyValues.Count);
            Assert.AreSame(keyValue1, section.KeyValues.First());
            Assert.AreSame(keyValue3, section.KeyValues.Last());
        }

        [TestMethod]
        public void AddKeyValue_AddMultipleKeyValuesByStringParameters_KeyValuesAreAdded()
        {
            // Arrange
            IniSection section = new IniSection("testSection");

            // Act
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");
            section.KeyValues.Add("testKey3", "testValue3");

            // Assert
            Assert.AreEqual(3, section.KeyValues.Count);
            Assert.AreEqual("testKey1", section.KeyValues.First().Key);
            Assert.AreEqual("testValue1", section.KeyValues.First().Value);
            Assert.AreEqual("testKey3", section.KeyValues.Last().Key);
            Assert.AreEqual("testValue3", section.KeyValues.Last().Value);
        }

        [TestMethod]
        public void AddKeyValue_KeyExists_OldValueIsReplaced()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey", "oldValue");

            // Act
            section.KeyValues.Add("testKey", "newValue");

            // Assert
            Assert.AreEqual(1, section.KeyValues.Count);
            Assert.AreEqual("newValue", section["testKey"]);
        }

        [TestMethod]
        public void AddKeyValue_KeyExists_OldReferenceIsDiscarded()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            IniKeyValue oldKeyValue = new IniKeyValue("testKey", "oldValue");
            IniKeyValue newKeyValue = new IniKeyValue("testKey", "newValue");

            // Act
            section.KeyValues.Add(oldKeyValue);
            section.KeyValues.Add(newKeyValue);

            // Assert
            Assert.AreEqual(1, section.KeyValues.Count);
            Assert.AreNotSame(oldKeyValue, section.KeyValues["testKey"]);
            Assert.AreSame(newKeyValue, section.KeyValues["testKey"]);
        }

        [TestMethod]
        public void IndexerGet_IndexKeys_ValuesAreIndexed()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");
            section.KeyValues.Add("testKey3", "testValue3");

            // Act
            string indexedValue1 = section["testKey1"];
            string indexedValue2 = section["testKey2"];
            string indexedValue3 = section["testKey3"];

            // Assert
            Assert.AreEqual("testValue1", indexedValue1);
            Assert.AreEqual("testValue2", indexedValue2);
            Assert.AreEqual("testValue3", indexedValue3);
        }

        [TestMethod]
        public void IndexerGet_IndexWithWeirdCasing_IndexerIsCaseInsensitive()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");
            section.KeyValues.Add("testKey3", "testValue3");

            // Act
            string indexedValue1 = section["TeStKeY1"];
            string indexedValue2 = section["TESTKEY2"];
            string indexedValue3 = section["testkey3"];

            // Assert
            Assert.AreEqual("testValue1", indexedValue1);
            Assert.AreEqual("testValue2", indexedValue2);
            Assert.AreEqual("testValue3", indexedValue3);
        }

        [TestMethod]
        public void IndexerGet_IndexKeyThatDoesNotExist_ReturnsNull()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");

            // Act
            string indexedValue = section["testKeyNull"];

            // Assert
            Assert.IsNull(indexedValue);
        }

        [TestMethod]
        public void KeyValuesIndexerGet_IndexKeys_KeyValuesAreIndexed()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");
            section.KeyValues.Add("testKey3", "testValue3");

            // Act
            IniKeyValue indexedKeyValue1 = section.KeyValues["testKey1"];
            IniKeyValue indexedKeyValue2 = section.KeyValues["testKey2"];
            IniKeyValue indexedKeyValue3 = section.KeyValues["testKey3"];

            // Assert
            Assert.AreSame("testKey1", indexedKeyValue1.Key);
            Assert.AreSame("testKey2", indexedKeyValue2.Key);
            Assert.AreSame("testKey3", indexedKeyValue3.Key);
            Assert.AreSame("testValue1", indexedKeyValue1.Value);
            Assert.AreSame("testValue2", indexedKeyValue2.Value);
            Assert.AreSame("testValue3", indexedKeyValue3.Value);
        }

        [TestMethod]
        public void GetValueOrEmpty_LookUpKeyThatDoesExist_ReturnsValue()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");

            // Act
            string lookedUpValue = section.GetValueOrEmpty("testKey1");

            // Assert
            Assert.AreEqual("testValue1", lookedUpValue);
        }

        [TestMethod]
        public void GetValueOrEmpty_LookUpKeyThatDoesNotExist_ReturnsEmpty()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");

            // Act
            string lookedUpValue = section.GetValueOrEmpty("testKeyNull");

            // Assert
            Assert.AreEqual(string.Empty, lookedUpValue);
        }

        [TestMethod]
        public void RemoveKeyValue_ByKeyExists_KeyValueIsRemoved()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");

            // Act
            IniKeyValue removedKeyValue = section.KeyValues.Remove("testKey1");

            // Assert
            Assert.AreEqual(1, section.KeyValues.Count);
            Assert.AreEqual("testKey1", removedKeyValue.Key);
            Assert.AreEqual("testValue1", removedKeyValue.Value);
            Assert.IsNull(section["testKey1"]);
        }

        [TestMethod]
        public void RemoveKeyValue_ByReferenceExists_KeyValueIsRemoved()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            IniKeyValue keyValue1 = new IniKeyValue("testKey1", "testValue1");
            IniKeyValue keyValue2 = new IniKeyValue("testKey2", "testValue2");
            section.KeyValues.Add(keyValue1);
            section.KeyValues.Add(keyValue2);

            // Act
            bool success = section.KeyValues.Remove(keyValue1);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNull(section["testKey1"]);
            Assert.AreEqual(1, section.KeyValues.Count);
        }

        [TestMethod]
        public void RemoveKeyValue_ByKeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            section.KeyValues.Add("testKey1", "testValue1");
            section.KeyValues.Add("testKey2", "testValue2");

            // Act
            IniKeyValue removedKeyValue = section.KeyValues.Remove("otherKey");

            // Assert
            Assert.AreEqual(2, section.KeyValues.Count);
            Assert.IsNull(removedKeyValue);
            Assert.IsNull(section["otherKey"]);
        }

        [TestMethod]
        public void RemoveKeyValue_ByReferenceDoesNotExist_NotSuccess()
        {
            // Arrange
            IniSection section = new IniSection("testSection");
            IniKeyValue keyValue1 = new IniKeyValue("testKey1", "testValue1");
            IniKeyValue keyValue2 = new IniKeyValue("testKey2", "testValue2");
            section.KeyValues.Add(keyValue1);
            section.KeyValues.Add(keyValue2);

            // Act
            IniKeyValue keyValueOther = new IniKeyValue("testKeyOther", "testValueOther");
            bool success = section.KeyValues.Remove(keyValueOther);

            // Assert
            Assert.IsFalse(success);
            Assert.IsNull(section["testKeyOther"]);
            Assert.AreEqual(2, section.KeyValues.Count);
        }
    }
}
