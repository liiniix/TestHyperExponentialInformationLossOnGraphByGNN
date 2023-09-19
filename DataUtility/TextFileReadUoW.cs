using System.Text;

namespace DataUtility
{
    public class TextFileReadUoW : ITextFileReadUoW
    {
        #region Fields

        #endregion

        #region CTor
        public TextFileReadUoW()
        {
        }
        #endregion

        #region Methods
        public FileStream GetFullTextFile(string fileName)
        {
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            return fileStream;
        }

        public List<string> GetLinesFromTextFileList(string fileName)
        {
            IEnumerable<string> fileLinesList = File.ReadAllLines(fileName, Encoding.UTF8);

            return fileLinesList.ToList();
        }
        #endregion
    }
}
