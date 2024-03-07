namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.DTOs
{
    public class UserSelection
    {
        public string Family { get; set; }
        public string Model { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public IList<string> Indexes { get; set; }
    }
}
