using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

using Microsoft.Win32;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Common.Validation;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.DTOs;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic;
using SysmacStudioParameterEditorUserSelectionFileMaker.UI.Common;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.UI.ViewModels
{
    internal class MainWindowViewModel : NotifyBase
    {
        private const string ButtonTooltipMessage = "Family, Title and Indexes are required fields.";

        private readonly UserSelectionFileManager userSelectionFileManager;
        private string family;
        private string model;
        private string title;
        private string comment;
        private string indexesInput;
        private string buttonTooltip;
        private string versionLabel;

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

        public string ButtonTooltip
        {
            get
            {
                return buttonTooltip;
            }

            set
            {
                if (value != buttonTooltip)
                {
                    buttonTooltip = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string VersionLabel
        {
            get
            {
                return versionLabel;
            }

            set
            {
                if (value != versionLabel)
                {
                    versionLabel = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand LoadFileCommand { get; private set; }
        public RelayCommand SaveFileCommand { get; private set; }
        public RelayCommand ClearFormCommand { get; private set; }

        public MainWindowViewModel(UserSelectionFileManager userSelectionFileManager)
        {
            ParameterChecker.IsNotNull(userSelectionFileManager, nameof(userSelectionFileManager));

            this.userSelectionFileManager = userSelectionFileManager;
            LoadFileCommand = new RelayCommand(o => ExecuteLoadFileCommand());
            SaveFileCommand = new RelayCommand(o => ExecuteSaveFileCommand(), o => CanExecuteSaveFileCommand());
            ClearFormCommand = new RelayCommand(o => ExecuteClearFormCommand());

            VersionLabel = GetAssemblyVersion();

            InitializeForm();
        }

        private void InitializeForm()
        {
            Family = string.Empty;
            Model = string.Empty;
            Title = string.Empty;
            Comment = string.Empty;
            IndexesInput = string.Empty;
            ButtonTooltip = ButtonTooltipMessage;
        }

        private void ExecuteLoadFileCommand()
        {
            var loadFileDialog = new OpenFileDialog();
            loadFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            loadFileDialog.Filter = "User selection|*.usel";

            if (loadFileDialog.ShowDialog() == true)
            {
                var data = userSelectionFileManager.LoadFile(loadFileDialog.FileName);

                if (data == null)
                {
                    MessageBox.Show("File couldn't be loaded! Check format.", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Family = data.Family;
                    Model = data.Model;
                    Title = data.Title;
                    Comment = data.Comment;

                    foreach (var item in data.Indexes)
                    {
                        IndexesInput = item + Environment.NewLine;
                    }
                }
            }
        }

        private bool CanExecuteSaveFileCommand()
        {
            var fieldsAreValid = FieldsAreValid();
            UpdateButtonTooltip(fieldsAreValid);
            return fieldsAreValid;
        }

        private void ExecuteSaveFileCommand()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "User selection|*.usel";
            saveFileDialog.FileName = Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                if (Directory.Exists(folderPath))
                {
                    var data = new UserSelection()
                    {
                        Family = Family,
                        Model = Model,
                        Title = Title,
                        Comment = Comment,
                        Indexes = ParseIndexesList()
                    };
                    userSelectionFileManager.CreateFile(data, saveFileDialog.FileName);

                    OpenSavingFolder(folderPath);
                }
                else
                {
                    MessageBox.Show("Location invalid!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteClearFormCommand()
        {
            InitializeForm();
        }

        private bool FieldsAreValid()
        {
            return !string.IsNullOrWhiteSpace(Family) && !string.IsNullOrWhiteSpace(Title) && IndexesListIsValid();
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
            SaveFileCommand.RaiseCanExecuteChanged();
        }

        private void UpdateButtonTooltip(bool hideTooltip)
        {
            ButtonTooltip = hideTooltip ? string.Empty : ButtonTooltipMessage;
        }

        private void OpenSavingFolder(string folderPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = folderPath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        private string GetAssemblyVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            return $"v{version.Major}.{version.Minor}.{version.Build}";
        }
    }
}
