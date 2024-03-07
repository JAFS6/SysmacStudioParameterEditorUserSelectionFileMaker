using System.Windows;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.UI.Runtime
{
    internal class SingleInstanceGuardian
    {
        private static Mutex _mutex = null;
        private const string AppName = "App";

        public void ForceSingleInstance()
        {
            bool createdNew;

            _mutex = new Mutex(true, AppName, out createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application
                Application.Current.Shutdown();
            }
        }
    }
}
