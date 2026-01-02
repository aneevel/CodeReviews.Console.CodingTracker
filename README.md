# Coding Tracker Application

Console application for tracking the time that you spend coding in a session.

Follows the requirements as set out by [C# Academy](https://www.thecsharpacademy.com/project/13/coding-tracker).

# How to Use

Run Project in preferred IDE/Terminal with `dotnet run` and follow on-screen prompts.

# Features

- Provides easy to use console interface
- Builds and interacts with Sqlite Database
- Insert, View, Update, and Delete Coding Sessions
- Sessions contain start time, end time, and duration fields
- Insert an already completed session, or start a running session you're currently working on
- Sort Sessions by all fields, whether ascending or descending
- Unit Tests for CodingSessionService

# Challenges

I implemented all challenges provided, save for the Desktop build and AI inclusion.

# Difficulties

- Understanding the issues with querying records from the database was difficult, particularly understanding how to parse the TimeSpan value; the sections on SqlMapper
are a bit lacking on the Dapper website, I had to really dig in order to find the relevant information for this.
- Figuring out the Stopwatch functionality required a good amount of experimentation. I ended up enjoying my use of Spectre.Console's Live features, but it is not without its flaws
- Designing in a way that considers testing up front is always interesting and an exercise that only seems to improve the architecture.

# What I learned

- A deeper understanding of the intricacies involved with reading/writing to databases and how many ORMs I've used have abstracted over those intricacies in the past.
- It's good to do some design up front, even if you will likely be wrong about some decisions! This helps you think through potential pitfalls right at the start, especially if you diagram relationships.
- Moq is a very simple framework to use!

# Areas to improve

- I don't like the architecture I set up for rendering; the View solution made sense at first, but if I considered the requirements for each View initially, I would have seen
the need for different implementations. My idea was to provide an interface to Views so that I could provide the CodingTracker application itself with a list of generic Views, which would
make adding new Views simple. Since the requirements differed between the Views though, my attempts to make a generic interface ended up being way too convoluted to be of help.
It also made the implementation of pagination for View Sessions break the abstraction; the View shouldn't be worrying about input, but under this architecture it does.
- The Live feature "stutters" whenever a non-"s" key is held down. I need to keep experimenting with ways of implementing the Live features combined with user input, but thinking
about combining blocking and non-blocking features has been tricky for me.

# Technologies used

- Dapper (2.1.66)
- Microsoft.Extensions.Configuration (10.0.1)
- Microsoft.Extensions.Configuration.Json (10.0.1)
- Microsoft.Extensions.Configuration.FileExtensions (10.0.1)
- Spectre.Console (0.54.0)
- System.Data.SQLite.Core(1.0.119)
- Serilog (4.3.0)
- Serilog.Sinks.Console (6.1.1)
- Serilog.Sinks.File (7.0.0)

# Reference materials used

[Spectre Console Documentation](https://spectreconsole.net/console)
[Dapper Documentation](https://www.learndapper.com/)
[Serilog Documentation](https://github.com/serilog/serilog/wiki/)
[Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/fundamentals/)
[Unit Testing Guide by Zied Rebhi](https://dev.to/zrebhi/the-ultimate-guide-to-unit-testing-in-net-c-using-xunit-and-moq-without-the-boring-examples-28ad)
