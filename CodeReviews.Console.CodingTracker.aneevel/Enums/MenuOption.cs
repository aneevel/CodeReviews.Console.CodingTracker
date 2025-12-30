using System.ComponentModel.DataAnnotations;

namespace CodeReviews.Console.CodingTracker.aneevel.Enums
{
    public enum MenuOption
    {
        [Display(Name = "View all sessions")]
        ViewAllSessions,

        [Display(Name = "Start a running session")]
        StartRunningSession,

        [Display(Name = "Insert a completed session")]
        InsertSession,

        [Display(Name = "Update an existing session")]
        UpdateSession,

        [Display(Name = "Delete an existing session")]
        DeleteSession,

        [Display(Name = "Exit Coding Tracker")]
        ExitApplication,
    }
}
