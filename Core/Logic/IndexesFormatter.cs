namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic
{
    public static class IndexesFormatter
    {
        private static char[] IndexesSeparators = [' ', ',', ';'];

        public static IList<string> ToList(string input)
        {
            var result = new List<string>();

            var lines = input.Split('\n');

            foreach (var line in lines)
            {
                var indexes = line.Split(IndexesSeparators, StringSplitOptions.RemoveEmptyEntries);

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

        public static string ToString(IList<string> input)
        {
            string result = string.Empty;

            foreach (var item in input)
            {
                result += item + Environment.NewLine;
            }

            return result;
        }
    }
}
