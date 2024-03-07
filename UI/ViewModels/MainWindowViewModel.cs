using Microsoft.Win32;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Common.Validation;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.DTOs;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic;
using SysmacStudioParameterEditorUserSelectionFileMaker.UI.Common;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.UI.ViewModels
{
    internal class MainWindowViewModel : NotifyBase
    {
        private readonly UserSelectionFileCreator userSelectionFileCreator;
        private string family;
        private string model;
        private string title;
        private string comment;
        private string indexesInput;

        public string Family
        {
            get
            {
                return family;
            }

            set
            {
                if (value != family)
                {
                    family = value;
                    RaisePropertyChanged();
                    UpdateCanExecute();
                }
            }
        }

        public string Model
        {
            get
            {
                return model;
            }

            set
            {
                if (value != model)
                {
                    model = value;
                    RaisePropertyChanged();
                    UpdateCanExecute();
                }
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (value != title)
                {
                    title = value;
                    RaisePropertyChanged();
                    UpdateCanExecute();
                }
            }
        }

        public string Comment
        {
            get
            {
                return comment;
            }

            set
            {
                if (value != comment)
                {
                    comment = value;
                    RaisePropertyChanged();
                    UpdateCanExecute();
                }
            }
        }

        public string IndexesInput
        {
            get
            {
                return indexesInput;
            }

            set
            {
                if (value != indexesInput)
                {
                    indexesInput = value;
                    RaisePropertyChanged();
                    UpdateCanExecute();
                }
            }
        }

        public RelayCommand CreateFileCommand { get; private set; }

        public MainWindowViewModel(UserSelectionFileCreator userSelectionFileCreator)
        {
            ParameterChecker.IsNotNull(userSelectionFileCreator, nameof(userSelectionFileCreator));

            this.userSelectionFileCreator = userSelectionFileCreator;
            CreateFileCommand = new RelayCommand(o => ExecuteCreateFileCommand(), o => CanExecuteCreateFileCommand());

            Family = string.Empty;
            Model = string.Empty;
            Title = string.Empty;
            Comment = string.Empty;
            IndexesInput = string.Empty;
        }

        private bool CanExecuteCreateFileCommand()
        {
            return !string.IsNullOrWhiteSpace(Family) && !string.IsNullOrWhiteSpace(Title) && IndexesListIsValid();
        }

        private void ExecuteCreateFileCommand()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "User selection|*.usel";
            saveFileDialog.FileName = Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                var data = new UserSelection()
                {
                    Family = Family,
                    Model = Model,
                    Title = Title,
                    Comment = Comment,
                    Indexes = ParseIndexesList()
                };
                userSelectionFileCreator.CreateFile(data, saveFileDialog.FileName);
            }
        }

        private bool IndexesListIsValid()
        {
            return ParseIndexesList().Count() > 0;
        }

        private IList<string> ParseIndexesList()
        {
            var result = new List<string>();

            var lines = IndexesInput.Split('\n');

            foreach (var line in lines)
            {
                var indexes = line.Split(' ');

                foreach (var index in indexes)
                {
                    if (!string.IsNullOrWhiteSpace(index))
                    {
                        result.Add(index.Trim());
                    }
                }
            }

            return result;
        }

        private void UpdateCanExecute()
        {
            CreateFileCommand.RaiseCanExecuteChanged();
        }
    }
}
