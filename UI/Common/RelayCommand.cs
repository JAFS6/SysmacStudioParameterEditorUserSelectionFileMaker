using System.Windows.Input;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Common.Validation;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.UI.Common
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _execute = null;
        private readonly Func<object, bool> _canExecute = null;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            ParameterChecker.IsNotNull(execute, nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
