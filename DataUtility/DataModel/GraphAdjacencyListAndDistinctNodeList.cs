namespace DataUtility.DataModel
{
    public class GraphAdjacencyListAndDistinctNodeList
    {
        public Dictionary<int, List<int>> AdjacencyListDict { get; set; } = new Dictionary<int, List<int>>();
        public HashSet<int> DistinctNodeList = new HashSet<int>();
    }
}
