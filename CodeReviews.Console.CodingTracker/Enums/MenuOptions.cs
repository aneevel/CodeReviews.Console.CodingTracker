using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeReviews.Console.CodingTracker.aneevel.Enums
{
    internal enum MenuOptions
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
