using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Common.Validation;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Services;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.FileManagement
{
    public class FileManagementService : IFileManagementService
    {
        public string ReadAllText(string filePath)
        {
            ParameterChecker.IsNotNullOrEmpty(filePath, nameof(filePath));

            return File.ReadAllText(filePath);
        }

        public bool WriteAllText(string filePath, string content)
        {
            ParameterChecker.IsNotNullOrEmpty(filePath, nameof(filePath));
            ParameterChecker.IsNotNullOrEmpty(content, nameof(content));

            try
            {
                File.WriteAllText(filePath, content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreatePath(string path)
        {
            ParameterChecker.IsNotNullOrEmpty(path, nameof(path));

            Directory.CreateDirectory(path);
        }

        public bool FileExists(string path)
        {
            ParameterChecker.IsNotNullOrEmpty(path, nameof(path));

            return File.Exists(path);
        }
    }
}
