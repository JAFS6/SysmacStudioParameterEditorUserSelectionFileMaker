using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.Tests
{
    [TestClass]
    public class IndexesFormatterTests
    {
        [TestMethod]
        public void ToList_EmptyString_ReturnsEmptyList()
        {
            var result = IndexesFormatter.ToList(string.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ToList_SingleIndex_ReturnsListWithIndex()
        {
            const string Expected = "Index1";
            var result = IndexesFormatter.ToList(Expected);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(Expected, result[0]);
        }

        [TestMethod]
        public void ToList_IndexesSeparatedByWhiteSpaces_ReturnsListWithIndexes()
        {
            const string Index1 = "Index1";
            const string Index2 = "Index2";
            var result = IndexesFormatter.ToList($"{Index1}  {Index2}");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(Index1, result[0]);
            Assert.AreEqual(Index2, result[1]);
        }

        [TestMethod]
        public void ToList_IndexesSeparatedByComma_ReturnsListWithIndexes()
        {
            const string Index1 = "Index1";
            const string Index2 = "Index2";
            var result = IndexesFormatter.ToList($"{Index1},{Index2}");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(Index1, result[0]);
            Assert.AreEqual(Index2, result[1]);
        }

        [TestMethod]
        public void ToList_IndexesSeparatedBySemicolon_ReturnsListWithIndexes()
        {
            const string Index1 = "Index1";
            const string Index2 = "Index2";
            var result = IndexesFormatter.ToList($"{Index1};{Index2}");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(Index1, result[0]);
            Assert.AreEqual(Index2, result[1]);
        }

        [TestMethod]
        public void ToList_IndexesSeparatedByMixOfCommaSemicolonAndWhitespaces_ReturnsListWithIndexes()
        {
            const string Index1 = "Index1";
            const string Index2 = "Index2";
            const string Index3 = "Index3";
            const string Index4 = "Index4";
            var result = IndexesFormatter.ToList($"{Index1}; {Index2}, {Index3} {Index4}");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(Index1, result[0]);
            Assert.AreEqual(Index2, result[1]);
            Assert.AreEqual(Index3, result[2]);
            Assert.AreEqual(Index4, result[3]);
        }
    }
}
