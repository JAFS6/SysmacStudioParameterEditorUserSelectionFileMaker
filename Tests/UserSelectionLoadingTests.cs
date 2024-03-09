using Moq;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.DTOs;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Services;

namespace Tests
{
    [TestClass]
    public class UserSelectionLoadingTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FilePathIsNull_ThrowsArgumentException()
        {
            // Arrange
            var fileManagementServiceMock = new Mock<IFileManagementService>();
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(null);

            // Assert by attribute
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FilePathIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var fileManagementServiceMock = new Mock<IFileManagementService>();
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(string.Empty);

            // Assert by attribute
        }

        [TestMethod]
        public void AllFieldsFilledAndValid_FileIsLoaded()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(family, result.Family);
            Assert.AreEqual(model, result.Model);
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(comment, result.Comment);
            Assert.AreEqual(2, result.Indexes.Count);
            Assert.AreEqual(index1, result.Indexes[0]);
            Assert.AreEqual(index2, result.Indexes[1]);
        }

        [TestMethod]
        public void FileContentIsNotXml_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string fileContent = $"not an xml";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesFamilyElement_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesFamilyNameAttribute_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlFamilyNameAttributeIsEmpty_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesModelElement_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesModelNameAttribute_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlModelNameAttributeIsEmpty_FileIsLoaded()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(family, result.Family);
            Assert.AreEqual(model, result.Model);
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(comment, result.Comment);
            Assert.AreEqual(2, result.Indexes.Count);
            Assert.AreEqual(index1, result.Indexes[0]);
            Assert.AreEqual(index2, result.Indexes[1]);
        }

        [TestMethod]
        public void XmlMissesInformationElement_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesInformationNameAttribute_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesInformationCommentAttribute_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlInformationNameAttributeIsEmpty_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlInformationCommentAttributeIsEmpty_FileIsLoaded()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(family, result.Family);
            Assert.AreEqual(model, result.Model);
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(comment, result.Comment);
            Assert.AreEqual(2, result.Indexes.Count);
            Assert.AreEqual(index1, result.Indexes[0]);
            Assert.AreEqual(index2, result.Indexes[1]);
        }

        [TestMethod]
        public void XmlMissesFavouritesElement_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesParameterElement_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlMissesParameterIndexAttribute_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void XmlParameterIndexAttributeIsEmpty_ReturnsNull()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "";
            const string fileContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var fileManagementServiceMock = new Mock<IFileManagementService>();

            fileManagementServiceMock.Setup(m => m.FileExists(filePath)).Returns(true);
            fileManagementServiceMock.Setup(m => m.ReadAllText(filePath)).Returns(fileContent);
            var target = new UserSelectionFileManager(fileManagementServiceMock.Object);

            // Act
            UserSelection result = target.LoadFile(filePath);

            // Assert
            Assert.IsNull(result);
        }
    }
}
