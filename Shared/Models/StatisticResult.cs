namespace Shared.Models
{
    public class StatisticResult<T>
    {
        public StatisticStrategy StatisticBy { get; set; }

        public StatisticResultItem[] Details { get; set; }

        public double HighestIncome { get; set; }

        public double LowestIncome { get; set; }

        public StatisticDateResult HighestDate { get; set; }

        public StatisticDateResult LowestDate { get; set; }

        public StatisticResult(StatisticStrategy statisticStrategy)
        {
            StatisticBy = statisticStrategy;
        }
    }
}
