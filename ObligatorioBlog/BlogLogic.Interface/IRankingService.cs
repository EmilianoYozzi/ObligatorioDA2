using BlogDomain;

namespace BlogServicesInterfaces
{
    public interface IRankingService
    {
        UserScore[] GetUserActivityRanking(DateRange range);
        UserScore[] GetUserOffensesRanking(DateRange range);
    } 
}