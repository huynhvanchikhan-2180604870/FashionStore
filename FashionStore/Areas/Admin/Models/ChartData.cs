namespace FashionStore.Areas.Admin.Models
{
    public class ChartData
    {
        public string Name { get; set; }
        public List<int> Data { get; set; }
    }

    public class ChartResponse
    {
        public List<string> Categories { get; set; }
        public List<ChartData> Series { get; set; }
    }
}
