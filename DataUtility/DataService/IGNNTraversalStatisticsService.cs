using DataUtility.DataModel;

namespace DataUtility.DataService
{
    public interface IGNNTraversalStatisticsService
    {
        public List<GNNLevelwiseNodeTraversalStatistics> GetGNNLevelwiseNodeTraversalStatisticsEnumerable(string filenameWithLocation, int maximumTraversalLevel, bool isGetSummerizedNodeTraversal);
        public void GetGNNLevelwiseNormalisedNodeTraversalStatisticsEnumerable();
    }
}
