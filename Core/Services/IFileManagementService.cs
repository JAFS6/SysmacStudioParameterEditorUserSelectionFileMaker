namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.Services
{
    public interface IFileManagementService
    {
        string ReadAllText(string filePath);
        void WriteAllText(string filePath, string content);
        void CreatePath(string path);
        bool FileExists(string path);
    }
}
