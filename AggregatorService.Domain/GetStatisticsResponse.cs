namespace AggregatorService.Domain;

public class GetStatisticsResponse
{
    public IEnumerable<ApiPeformance> Apis { get; set; }
}

public class ApiPeformance
{
    public string ApiName { get; set; }
    public double AverageResponseMilliSeconds { get; set; }
    public int TotalNumOfRequestsFast { get; set; }
    public int TotalNumOfRequestsAverage { get; set; }
    public int TotalNumOfRequestsSlow { get; set; }
}
