namespace DataUtility
{
    public interface ITextFileReadUoW
    {
        FileStream GetFullTextFile(string fileName);
        List<string> GetLinesFromTextFileList(string fileName);
    }
}