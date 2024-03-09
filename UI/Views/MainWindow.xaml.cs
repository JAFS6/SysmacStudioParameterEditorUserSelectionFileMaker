using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveFileButton_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var button = sender as Button;
            var command = button.Command as ICommand;

            if (button != null && command != null && command.CanExecute(null))
            {
                e.Handled = true; // Prevents the tooltip from showing when the button is enabled
                return;
            }
        }
    }
}
