namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.Common.Validation
{
    public class ParameterChecker
    {
        public static void IsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                LaunchArgumentNullExceptionBecauseOfNull(parameterName);
            }
        }

        public static void IsNotNullOrEmpty(string parameter, string parameterName)
        {
            IsNotNull(parameter, parameterName);

            if (string.IsNullOrWhiteSpace(parameter))
            {
                LaunchArgumentExceptionBecauseOfEmpty(parameterName);
            }
        }

        private static void LaunchArgumentNullExceptionBecauseOfNull(string parameterName)
        {
            throw new System.ArgumentNullException(parameterName, parameterName + " is null.");
        }

        private static void LaunchArgumentExceptionBecauseOfEmpty(string parameterName)
        {
            throw new System.ArgumentException(parameterName, parameterName + " is empty or only whitespaces.");
        }
    }
}
