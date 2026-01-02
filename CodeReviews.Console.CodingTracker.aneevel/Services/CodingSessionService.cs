using CodeReviews.Console.CodingTracker.aneevel.Database.Repositories;
using CodeReviews.Console.CodingTracker.aneevel.Models;

namespace CodeReviews.Console.CodingTracker.aneevel.Services;

internal class CodingSessionService(ICodingSessionRepository repository) : ICodingSessionService
{
    public List<CodingSession> GetCodingSessions()
    {
        return repository.GetCodingSessions();
    }
    
    public int InsertCodingSession(CodingSession codingSession)
    {
        return repository.InsertCodingSession(codingSession);
    }

    public int UpdateCodingSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration)
    {
       return repository.UpdateCodingSession(id, startTime, endTime, duration); 
    }

    public int DeleteCodingSession(int id)
    {
        return repository.DeleteCodingSession(id);
    }
}