using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NUnit;
using P7CreateRestApi;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Data;
using Xunit;
using Moq;
using P7CreateRestApi.IServices;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;


namespace P7CreateRestApiTest
{

    /// <Summary>
    /// Creating an SQL datatabse context for all tests needing sql
    /// <Summary>
    public class DatabaseFixture : IDisposable
    {

        public P7Referential Context { get; private set; }

        public DatabaseFixture()
        {


        /// <summary>
        /// Build a new configuration path and name of the sql connection string folder.
        /// </summary >
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            /// <summary>
            /// Choose the database.
            /// </summary >
            var connectionString = configuration.GetConnectionString("P7ReferentialForTests");

            /// <summary>
            /// Allows for multiple sql connection attempts.
            /// </summary >
            var options = new DbContextOptionsBuilder<P7Referential>()
            .UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure())
            .Options;


            /// <summary>
            /// Preparing options to match DbContextOptions type.
            /// </summary >
            DbContextOptions<P7Referential> optionsForInjection = options;

            /// <summary>
            /// Instanciate the new context using data injection.
            /// </summary >
            //Context = new P7Referential(optionsForInjection, configuration);
            Context = new P7Referential(optionsForInjection);


        }

        public void Dispose()
        {
            /// <summary>
            /// Opens the database for use.
            /// </summary >
            Context.Dispose();
        }


        public class DatabaseTests : IClassFixture<DatabaseFixture>
        {
            private readonly P7Referential _context;

            /// <summary>
            /// Polymorphism : P3ReferentialForTests _context is derivated in each test (below) using the public _context.
            /// </summary >
            public DatabaseTests(DatabaseFixture fixture)
            {
                _context = fixture.Context;
            }


            [Fact]
            public void CheckProductModelErrors_ShouldReturn_MissingName()
            {
                /// Arrange

                /// <summary>
                /// Use of Moq to replicate I(Name)Service.
                /// No need to buildup dependances like intermediary Interfaces or even SQL.
                /// </summary >

                var mockedIBidListRepository = new Mock<IBidListRepository>();
                var mockedIBidListService = new Mock<IBidListService>();
                var mockedBidListController = new Mock<BidListsController>(mockedIBidListRepository.Object, mockedIBidListService.Object);
  


                //var service = new ServiceCollection();
                //service.AddLogging();



                BidListDto bidListDto = new BidListDto
                {
                  BidListId = 0,
                  Account = "string",
                  BidType = "string",
                  BidQuantity = 0.0001,
                  AskQuantity = 0.0001,
                  Bid = 0.0001,
                  Ask = 0.0001,
                  Benchmark = "string",
                  BidListDate = "2026-09-04T15:24:23.650Z",
                  Commentary = "string",
                  BidSecurity = "string",
                  BidStatus = "string",
                  Trader = "string",
                  Book = "string",
                  CreationName = "string",
                  CreationDate = "2026-09-04T15:24:23.650Z",
                  RevisionName = "string",
                  RevisionDate = "2026-09-04T15:24:23.650Z",
                  DealName = "string",
                  DealType = "string",
                  SourceListId = "string",
                  Side = "string"
                };

                ///// <summary>
                ///// Creating a temporary dictionnary to store results 
                ///// </summary >
                Dictionary<string, string> errorTempDictionary = new Dictionary<string, string>();


                ///Act
                Task CreateBidList = mockedBidListController.Object.CreateBidList(bidListDto);

                ///Assert
                Xunit.Assert.True(errorTempDictionary.Count == 0);
                Xunit.Assert.True(CreateBidList.IsCompletedSuccessfully);
                //Assert.True(errorTempD.ictionary.ContainsKey("MissingName"));


            }

        }
        //public class Tests
        //{
        //    [SetUp]
        //    public void Setup()
        //    {
        //    }

        //    [Test]
        //    public void Test1()
        //    {
        //        Assert.Pass();
        //    }
        //}
    }
}