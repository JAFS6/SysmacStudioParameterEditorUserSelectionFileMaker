using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.UI.Common
{
    internal class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
