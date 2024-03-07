using Moq;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.DTOs;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Services;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.Tests
{
    [TestClass]
    public class UserSelectionFileCreationTests
    {
        [TestMethod]
        public void AllFieldsFilledAndValid_FileIsWritten()
        {
            // Arrange
            const string filePath = "testPath";
            const string family = "Family";
            const string model = "Model";
            const string title = "Title";
            const string comment = "Comment";
            const string index1 = "Index1";
            const string index2 = "Index2";
            const string expectedContent = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                $"<userselectionlist Version=\"1.0\">\r\n" +
                $"  <Family Name=\"{family}\" />\r\n" +
                $"  <Model Name=\"{model}\" />\r\n" +
                $"  <information name=\"{title}\" comment=\"{comment}\" />\r\n" +
                $"  <favourites>\r\n" +
                $"    <parameter index=\"{index1}\" />\r\n" +
                $"    <parameter index=\"{index2}\" />\r\n" +
                $"  </favourites>\r\n" +
                $"</userselectionlist>";

            var indexes = new List<string>() { index1, index2 };
            var userSelection = new UserSelection()
            {
                Family = family,
                Model = model,
                Title = title,
                Comment = comment,
                Indexes = indexes
            };
            var fileManagementServiceMock = new Mock<IFileManagementService>();
            var target = new UserSelectionFileCreator(fileManagementServiceMock.Object);

            fileManagementServiceMock.Setup(m => m.WriteAllText(filePath, expectedContent)).Verifiable();

            // Act
            target.CreateFile(userSelection, filePath);

            // Assert
            fileManagementServiceMock.Verify();
        }
    }
}
