using System.Runtime.InteropServices.JavaScript;
using CodeReviews.Console.CodingTracker.aneevel.Database.Repositories;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using CodeReviews.Console.CodingTracker.aneevel.Services;
using Moq;
using Xunit;

namespace CodeReviews.Console.CodingTracker.aneevel.Tests;

public class CodingSessionServiceTests
{
   private readonly Mock<ICodingSessionRepository> _mockCodingSessionRepository;
   private readonly ICodingSessionService _codingSessionService;

   public CodingSessionServiceTests()
   {
      _mockCodingSessionRepository = new Mock<ICodingSessionRepository>();
      _codingSessionService = new CodingSessionService(_mockCodingSessionRepository.Object);
   }

   [Fact]
   public void GetCodingSessions_ShouldReturnAListOfCodingSessions_WhenListOfCodingSessionsExist()
   {
      var start = DateTime.Now;
      var end = start + new TimeSpan(2, 0, 0);

      var expected = new List<CodingSession>
      {
         new()
         {
            Id = 1,
            StartTime = start,
            EndTime = end,
            Duration = end - start
         },
         new()
         {
            Id = 2,
            StartTime = start,
            EndTime = end,
            Duration = end - start
         },
         new()
         {
            Id = 3,
            StartTime = start,
            EndTime = end,
            Duration = end - start
         }
      };
      
      _mockCodingSessionRepository.Setup(repository => repository.GetCodingSessions())
         .Returns(expected);

      var result = _codingSessionService.GetCodingSessions();

      Assert.NotNull(result);
      Assert.Equal(expected.Count, result.Count); 
      Assert.NotNull(result[0]);
   }
   
   [Fact]
   public void GetCodingSessions_ShouldReturnAnEmptyList_WhenProvidedNoCodingSessions()
   {
      var expected = new List<CodingSession>();
      
      _mockCodingSessionRepository.Setup(repository => repository.GetCodingSessions())
         .Returns(expected);
      
      var result = _codingSessionService.GetCodingSessions();
      
      Assert.NotNull(result);
      Assert.Empty(result);
   }
   
   [Fact]
   public void InsertCodingSession_ShouldReturnA0_WhenSuccessful()
   {
      const int expected = 0;
      _mockCodingSessionRepository.Setup(repository => repository.InsertCodingSession(It.IsAny<CodingSession>()))
         .Returns(expected);
      
      var result = _codingSessionService.InsertCodingSession(new CodingSession());
      
      Assert.Equal(expected, result);
   }

   [Fact]
   public void InsertCodingSession_ShouldReturnA1_WhenFailed()
   {
      const int expected = 1;
      _mockCodingSessionRepository.Setup(repository => repository.InsertCodingSession(It.IsAny<CodingSession>()))
         .Returns(expected);
      
      var result = _codingSessionService.InsertCodingSession(new CodingSession());
      
      Assert.Equal(expected, result);
   }

   [Fact]
   public void UpdateCodingSession_ShouldReturnA0_WhenSuccessful()
   {
      const int expected = 0;
      _mockCodingSessionRepository.Setup(repository =>
         repository.UpdateCodingSession(It.IsAny<int>(), 
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>(),
            It.IsAny<TimeSpan>()))
         .Returns(expected);
      
      var result = _codingSessionService.UpdateCodingSession(1, DateTime.Now, DateTime.Now, TimeSpan.MaxValue);
      
      Assert.Equal(expected, result);
   }

   [Fact]
   public void UpdateCodingSession_ShouldReturnA1_WhenFailed()
   {
      const int expected = 1;
      _mockCodingSessionRepository.Setup(repository => 
         repository.UpdateCodingSession(It.IsAny<int>(),
         It.IsAny<DateTime>(),
         It.IsAny<DateTime>(),
         It.IsAny<TimeSpan>()))
         .Returns(expected);
      
      var result = _codingSessionService.UpdateCodingSession(1, 
         DateTime.Now, 
         DateTime.Now,
         TimeSpan.MaxValue);
      
      Assert.Equal(expected, result);
   }

   [Fact]
   public void DeleteCodingSession_ShouldReturnA0_WhenSuccessful()
   {
      const int expected = 0;
      _mockCodingSessionRepository.Setup(repository => 
         repository.DeleteCodingSession(It.IsAny<int>())).Returns(expected);
      
      var result = _codingSessionService.DeleteCodingSession(1);
      
      Assert.Equal(expected, result);
   }

   [Fact]
   public void DeleteCodingSession_ShouldReturnA1_WhenFailed()
   {
      const int expected = 1;
      _mockCodingSessionRepository.Setup(repository =>
         repository.DeleteCodingSession(It.IsAny<int>())).Returns(expected);
      
      var result = _codingSessionService.DeleteCodingSession(1);
      
      Assert.Equal(expected, result);
   }
}