using CodeReviews.Console.CodingTracker.aneevel.Models;
using CodeReviews.Console.CodingTracker.aneevel.Enums;

namespace CodeReviews.Console.CodingTracker.aneevel.Extensions;

internal static class CodingSessionExtensions
{
  extension(List<CodingSession> sessions)
{
    internal List<CodingSession> SortBy(SortingField field, SortingDirection direction)
    {
        return field switch
        {
            SortingField.Id => sessions.SortById(direction),
            SortingField.StartTime => sessions.SortByStartTime(direction),
            SortingField.EndTime => sessions.SortByEndTime(direction),
            SortingField.Duration => sessions.SortByDuration(direction),
            _ => throw new NotImplementedException("Unhandled sorting field provided!")
        };
    }

    private List<CodingSession> SortById(SortingDirection direction)
    {
        return direction switch
        {
            SortingDirection.Asc => [.. sessions.OrderBy(codingSession => codingSession.Id)],
            SortingDirection.Desc => [.. sessions.OrderByDescending(codingSession => codingSession.Id)],
            _ => throw new InvalidOperationException("Unhandled sorting direction provided!")
        };
    }

    private List<CodingSession> SortByStartTime(SortingDirection direction)
    {
        return direction switch
        {
            SortingDirection.Asc => [.. sessions.OrderBy(codingSession => codingSession.StartTime)],
            SortingDirection.Desc => [.. sessions.OrderByDescending(codingSession => codingSession.StartTime),],
            _ => throw new InvalidOperationException("Unhandled sorting direction provided!")
        };
    }

    private List<CodingSession> SortByEndTime(SortingDirection direction)
    {
        return direction switch
        {
            SortingDirection.Asc => [.. sessions.OrderBy(codingSession => codingSession.EndTime)],
            SortingDirection.Desc => [.. sessions.OrderByDescending(codingSession => codingSession.EndTime)],
            _ => throw new InvalidOperationException("Unhandled sorting direction provided!")
        };
    }

    private List<CodingSession> SortByDuration(SortingDirection direction)
    {
        return direction switch
        {
            SortingDirection.Asc => [.. sessions.OrderBy(codingSession => codingSession.Duration)],
            SortingDirection.Desc => [.. sessions.OrderByDescending(codingSession => codingSession.Duration)],
            _ => throw new InvalidOperationException("Unhandled sorting direction provided!")
        };
    }

    internal List<CodingSession> TakeElementsAtIndex(int skipSize, int takeSize)
    {
        return sessions.Skip(skipSize).Take(takeSize).ToList();
    }
} 

}