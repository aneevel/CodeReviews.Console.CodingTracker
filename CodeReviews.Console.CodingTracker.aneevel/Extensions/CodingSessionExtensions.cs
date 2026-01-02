using CodeReviews.Console.CodingTracker.aneevel.Models;
using CodeReviews.Console.CodingTracker.aneevel.Views;

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
        if (direction == SortingDirection.ASC)
        {
            return [.. sessions.OrderBy(codingSession => codingSession.Id)];
        }
        else
        {
            return [.. sessions.OrderByDescending(codingSession => codingSession.Id)];
        }
    }

    private List<CodingSession> SortByStartTime(SortingDirection direction)
    {
        if (direction == SortingDirection.ASC)
        {
            return [.. sessions.OrderBy(codingSession => codingSession.StartTime)];
        }
        else
        {
            return
            [
                .. sessions.OrderByDescending(codingSession => codingSession.StartTime),
            ];
        }
    }

    private List<CodingSession> SortByEndTime(SortingDirection direction)
    {
        if (direction == SortingDirection.ASC)
        {
            return [.. sessions.OrderBy(codingSession => codingSession.EndTime)];
        }
        else
        {
            return [.. sessions.OrderByDescending(codingSession => codingSession.EndTime)];
        }
    }

    private List<CodingSession> SortByDuration(SortingDirection direction)
    {
        if (direction == SortingDirection.ASC)
        {
            return [.. sessions.OrderBy(codingSession => codingSession.Duration)];
        }
        else
        {
            return [.. sessions.OrderByDescending(codingSession => codingSession.Duration)];
        }
    }

    internal List<CodingSession> TakeElementsAtIndex(int skipSize, int takeSize)
    {
        return sessions.Skip(skipSize).Take(takeSize).ToList();
    }
} 

}