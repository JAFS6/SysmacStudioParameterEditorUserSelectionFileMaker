using System.Windows;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic;
using SysmacStudioParameterEditorUserSelectionFileMaker.FileManagement;
using SysmacStudioParameterEditorUserSelectionFileMaker.UI.Common;
using SysmacStudioParameterEditorUserSelectionFileMaker.UI.Runtime;
using SysmacStudioParameterEditorUserSelectionFileMaker.UI.ViewModels;
using SysmacStudioParameterEditorUserSelectionFileMaker.UI.Views;

namespace UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var singleInstanceGuardian = new SingleInstanceGuardian();
            singleInstanceGuardian.ForceSingleInstance();
            UIBehaviour.SetAllTextBoxesFirstClickSelectsAllText();
            base.OnStartup(e);

            var fileManagementService = new FileManagementService();
            var userSelectionFileManager = new UserSelectionFileManager(fileManagementService);
            var mainWindowViewModel = new MainWindowViewModel(userSelectionFileManager);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();
        }
    }

}
