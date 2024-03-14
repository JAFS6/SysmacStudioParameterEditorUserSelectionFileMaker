namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.Services
{
    public interface IFileManagementService
    {
        string ReadAllText(string filePath);
        bool WriteAllText(string filePath, string content);
        void CreatePath(string path);
        bool FileExists(string path);
    }
}
