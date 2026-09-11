using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Testing.Platform.Logging;
using Moq;
using NUnit;
using P7CreateRestApi;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Profiles;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Xml.Linq;
using Xunit;
using Xunit.Internal;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace P7CreateRestApiTest
{

    /// <Summary>
    /// Creating an SQL datatabse context for all tests needing sql
    /// <Summary>
    public class DatabaseFixture 
    {



        public class UnitTests
        {
            private P7Referential GetInMemoryDbContext()
            {
                var options = new DbContextOptionsBuilder<P7Referential>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;
                return new P7Referential(options);
            }



            [Fact]
            public async Task IBidListService_CreateBidListWithBidListDto_ShouldAdd_1BidList()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);

                BidListDto bidListDto = new BidListDto
                {
                    BidListId = 0,
                    Account = "test",
                    BidType = "string",
                    BidQuantity = 0.0001,
                    AskQuantity = 0.0001,
                    Bid = 0.0001,
                    Ask = 0.0001,
                    Benchmark = "string",
                    BidListDate = "11/08/2026 12:53:27",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "11/08/2026 12:53:27",
                    RevisionName = "string",
                    RevisionDate = "11/08/2026 12:53:27",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };

                ///Act
                Task CreateBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                int id = await bidListrepository.GetMaxBidListId();
                BidListDto foundBidListDto = iBidListService.GetBidListDtoById(id).Result.FirstOrDefault();


                ///Assert
                Xunit.Assert.True(CreateBidList.IsCompletedSuccessfully);
                bidListDto.BidListId = id;
                Xunit.Assert.Equivalent(bidListDto, foundBidListDto);

                await iBidListService.DeleteBidListById(id);

            }

            [Fact]
            public async Task IBidListService_UpdateBidListWithBidListDto_Modify_1BidList()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);

                BidListDto bidListDto = new BidListDto
                {
                    BidListId = 0,
                    Account = "test",
                    BidType = "string",
                    BidQuantity = 0.0001,
                    AskQuantity = 0.0001,
                    Bid = 0.0001,
                    Ask = 0.0001,
                    Benchmark = "string",
                    BidListDate = "11/08/2026 12:53:27",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "11/08/2026 12:53:27",
                    RevisionName = "string",
                    RevisionDate = "11/08/2026 12:53:27",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };


                Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                int id = createBidList.Id;
                //int id = await bidListrepository.GetMaxBidListId();
                BidListDto modifiedBidListDto = new BidListDto
                {
                    BidListId = 0,
                    Account = "test2string",
                    BidType = "string",
                    BidQuantity = 0.0001,
                    AskQuantity = 0.0001,
                    Bid = 1,
                    Ask = 1,
                    Benchmark = "string",
                    BidListDate = "11/08/2026 13:53:27",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "11/08/2026 13:53:27",
                    RevisionName = "teststring",
                    RevisionDate = "11/08/2026 13:53:27",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };


                ///Act
                BidListDto foundBidListDto = iBidListService.GetBidListDtoById(id).Result.FirstOrDefault();
                await iBidListService.UpdateBidListWithBidListDto(modifiedBidListDto);
                BidListDto modifiedFoundBidListDto = iBidListService.GetBidListDtoById(id).Result.FirstOrDefault();

                ///Assert
                Xunit.Assert.True(createBidList.IsCompletedSuccessfully);
                bidListDto.BidListId = id;
                Xunit.Assert.Equivalent(modifiedBidListDto, modifiedFoundBidListDto);
                await iBidListService.DeleteBidListById(id);

            }

            [Fact]
            public async Task IBidListService_DeleteBidListById_ShouldDelete_1BidList()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);

                BidListDto bidListDto = new BidListDto
                {
                    BidListId = 0,
                    Account = "test",
                    BidType = "string",
                    BidQuantity = 0.0001,
                    AskQuantity = 0.0001,
                    Bid = 0.0001,
                    Ask = 0.0001,
                    Benchmark = "string",
                    BidListDate = "11/08/2026 12:53:27",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "11/08/2026 12:53:27",
                    RevisionName = "string",
                    RevisionDate = "11/08/2026 12:53:27",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };

                Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                int id = await bidListrepository.GetMaxBidListId();

                Xunit.Assert.True(createBidList.IsCompletedSuccessfully);
                ///Act
                Task deleteBidList = iBidListService.DeleteBidListById(id);
                ///Assert
                Xunit.Assert.True(iBidListService.GetBidListDtoById(id).Result.FirstOrDefault() == null);
                Xunit.Assert.True(deleteBidList.IsCompletedSuccessfully);

            }

            [Fact]
            public async Task IBidListService_GetBidListDtoById_ShouldGet_1BidListById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);

                BidListDto bidListDto = new BidListDto
                {
                    BidListId = 0,
                    Account = "test",
                    BidType = "string",
                    BidQuantity = 0.0001,
                    AskQuantity = 0.0001,
                    Bid = 0.0001,
                    Ask = 0.0001,
                    Benchmark = "string",
                    BidListDate = "11/08/2026 12:53:27",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "11/08/2026 12:53:27",
                    RevisionName = "string",
                    RevisionDate = "11/08/2026 12:53:27",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };


                Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                int id = await bidListrepository.GetMaxBidListId();
                BidListDto FoundBidListDto = iBidListService.GetBidListDtoById(id).Result.FirstOrDefault();

                ///Act
                bidListDto.BidListId = id;
                Task getBidListById = bidListrepository.GetBidListById(id);

                ///Assert
                Xunit.Assert.Equivalent(bidListDto, FoundBidListDto);
                Xunit.Assert.True(createBidList.IsCompletedSuccessfully);
                Xunit.Assert.True(getBidListById.IsCompletedSuccessfully);

                await iBidListService.DeleteBidListById(id);
            }

            [Fact]
            public async Task IBidListService_GetAllBidLists_ShouldGet_AllBidListById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);

                BidListDto bidListDto1 = new BidListDto
                {
                    BidListId = 0,
                    Account = "test",
                    BidType = "string",
                    BidQuantity = 0.0001,
                    AskQuantity = 0.0001,
                    Bid = 0.0001,
                    Ask = 0.0001,
                    Benchmark = "string",
                    BidListDate = "11/08/2026 12:53:27",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "11/08/2026 12:53:27",
                    RevisionName = "string",
                    RevisionDate = "11/08/2026 12:53:27",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };


                BidListDto bidListDto2 = new BidListDto
                {
                    BidListId = 0,
                    Account = "test2",
                    BidType = "string",
                    BidQuantity = 0.0001,
                    AskQuantity = 0.0001,
                    Bid = 0.0001,
                    Ask = 0.0001,
                    Benchmark = "string",
                    BidListDate = "11/08/2026 12:53:27",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "11/08/2026 12:53:27",
                    RevisionName = "string",
                    RevisionDate = "11/08/2026 12:53:27",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };


                Task createBidList1 = iBidListService.CreateBidListWithBidListDto(bidListDto1);
                Task createBidList2 = iBidListService.CreateBidListWithBidListDto(bidListDto2);
                int id = await bidListrepository.GetMaxBidListId();
                BidListDto FoundBidListDto = iBidListService.GetBidListDtoById(id).Result.FirstOrDefault();

                ///Act

                var getAllBidLists = iBidListService.GetAllBidListsDto().Result.Where(b => b.BidListId > 0);

                ///Assert
                Xunit.Assert.True(createBidList1.IsCompletedSuccessfully);
                Xunit.Assert.True(createBidList2.IsCompletedSuccessfully);
                Xunit.Assert.True(getAllBidLists.Count() == 2);
                Xunit.Assert.True(getAllBidLists.ElementAt(0).BidListId == id - 1);
                Xunit.Assert.True(getAllBidLists.ElementAt(1).BidListId == id);

                await iBidListService.DeleteBidListById(id - 1);
                await iBidListService.DeleteBidListById(id);


            }


            //[Fact]
            //public async Task IBidLoginService_Login_ShouldLog_1User()
            //{

            //}    //Arrange

            //public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
            //    {
            //        var store = new Mock<IUserStore<TUser>>();
            //        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            //        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            //        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            //        mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            //        mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            //        mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            //        return mgr;
            //    }


            //    private readonly List<P7CreateRestApi.Domain.User> _users = new List<P7CreateRestApi.Domain.User>
            //    {
            //        new P7CreateRestApi.Domain.User { Email = "memberuser@test.com", Password = "Passmembertest15.", Role = "Member" },
            //        new P7CreateRestApi.Domain.User { Email = "adminuser@test.com", Password = "Passadmintest15.", Role = "Admin" }
            //    };

        }
    }
}

    //private _userManager = MockUserManager<ApplicationUser>().Object; 



                



            






