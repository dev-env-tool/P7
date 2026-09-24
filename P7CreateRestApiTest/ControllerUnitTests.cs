using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Testing.Platform.Logging;
using Moq;
using NUnit;
using NUnit.Framework.Internal;
using P7CreateRestApi;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.Filters;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Profiles;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web.Http.ModelBinding;
using System.Xml.Linq;
using Xunit;
using Xunit.Internal;
using static Duende.IdentityServer.Models.IdentityResources;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using User = P7CreateRestApi.Domain.User;


namespace P7CreateRestApiControllerUnitTests
{

    /// <Summary>
    /// Creating an SQL datatabse context for all tests needing sql
    /// <Summary>
    public class DatabaseFixture
    {



        public class BidListUnitTests
        {


            private P7Referential CreateInMemoryDbContext(string dbName = null)
            {
                var options = new DbContextOptionsBuilder<P7Referential>().UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString()).Options;
                return new P7Referential(options);
            }
            //private P7Referential GetInMemoryDbContext()
            //{
            //    var options = new DbContextOptionsBuilder<P7Referential>()
            //        .UseInMemoryDatabase(databaseName: "TestDatabaseBidListController")
            //        .Options;
            //    return new P7Referential(options);
            //}

            [Fact]
            public async Task IBidListController_CreateBidList_ShouldAdd_1BidList()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                using var context = CreateInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);
                BidListsController bidListsController = new BidListsController(bidListrepository, iBidListService);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                BidListDto bidListDto = new BidListDto
                {
                    Account = "test",
                    BidType = "string",
                    BidQuantity = "0,0001",
                    AskQuantity = "0,0001",
                    Bid = "0,0001",
                    Ask = "0,0001",
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
                var create = await bidListsController.CreateBidList(bidListDto);
                var bidListFound = await iBidListService.GetAllBidListsDto();
                int bidListIdFound = -1;
                if (bidListFound is IEnumerable<BidListDto> dtos0)
                {
                    bidListDto.BidListId = dtos0.ElementAt(0).BidListId;
                    bidListIdFound = dtos0.ElementAt(0).BidListId;
                    Xunit.Assert.Equivalent(dtos0.ElementAt(0), bidListDto);
                }
                var del = await bidListsController.DeleteBidListById(bidListIdFound);
            }

            [Fact]
            public async Task IBidListController_CreateBidList_ShouldNotAdd_1BidList()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);
                BidListsController bidListsController = new BidListsController(bidListrepository, iBidListService);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                BidListDto wrongBidListDto = new BidListDto
                {
                    Account = "test",
                    BidType = "string",
                    BidQuantity = "abc",
                    AskQuantity = "0,0001",
                    Bid = "0,0001",
                    Ask = "0,0001",
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
                bool result = bidListsController.CreateBidList(wrongBidListDto).IsFaulted;
                ///Assert
                Xunit.Assert.True(result);

            }

            [Fact]
            public async Task IBidListController_UpdateBidListById_Should_Modify_1BidList()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);
                BidListsController bidListsController = new BidListsController(bidListrepository, iBidListService);

                BidListDto bidListDto = new BidListDto
                {
                    Account = "test",
                    BidType = "string",
                    BidQuantity = "0,0001",
                    AskQuantity = "0,0001",
                    Bid = "0,0001",
                    Ask = "0,0001",
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


                var createBidList = await bidListsController.CreateBidList(bidListDto);
                int bidListIdFound = 0;
                var bidListFound = await iBidListService.GetAllBidListsDto();
                if (bidListFound is IEnumerable<BidListDto> dtos0)
                {
                    bidListIdFound = dtos0.ElementAt(0).BidListId;
                }

                BidListDto modifiedBidListDto = new BidListDto
                {
                    BidListId = bidListIdFound,
                    Account = "test2string",
                    BidType = "string",
                    BidQuantity = "0,0001",
                    AskQuantity = "0,0001",
                    Bid = "0,0001",
                    Ask = "0,0001",
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
                var update = await bidListsController.UpdateBidListById(modifiedBidListDto);
                var bidListFoundAfterModifications = await iBidListService.GetBidListDtoById(modifiedBidListDto.BidListId);
                if (bidListFoundAfterModifications is IEnumerable<BidListDto> dtos1)
                {
                    bidListDto.BidListId = dtos1.ElementAt(0).BidListId;
                    Xunit.Assert.Equivalent(modifiedBidListDto, dtos1.ElementAt(0));
                }
                var del = await bidListsController.DeleteBidListById(bidListIdFound);
            }

            [Fact]
            public async Task IBidListController_DeleteBidListById_ShouldDelete_1BidList()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);
                BidListsController bidListsController = new BidListsController(bidListrepository, iBidListService);

                BidListDto bidListDto = new BidListDto
                {
                    Account = "test",
                    BidType = "string",
                    BidQuantity = "2",
                    AskQuantity = "2",
                    Bid = "2",
                    Ask = "2",
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

                var create = await bidListsController.CreateBidList(bidListDto);
                var bidListFound = await iBidListService.GetAllBidListsDto();
                int bidListIdFound = 0;
                if (bidListFound is IEnumerable<BidListDto> dtos0)
                {
                    bidListIdFound = dtos0.ElementAt(0).BidListId;
                }
                ///Act
                var deleteBidList = await bidListsController.DeleteBidListById(bidListIdFound);
                ///Assert
                var bidListFoundAfterDelete = await iBidListService.GetBidListDtoById(bidListIdFound);
                if (bidListFoundAfterDelete is IEnumerable<BidListDto> dtos1)
                {
                    Xunit.Assert.True(bidListFoundAfterDelete.Count() == 0);
                }


            }

            [Fact]
            public async Task IBidListController_GetBidListDtoById_ShouldGet_1BidListById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);
                BidListsController bidListsController = new BidListsController(bidListrepository, iBidListService);

                BidListDto bidListDto = new BidListDto
                {
                    Account = "test",
                    BidType = "string",
                    BidQuantity = "2",
                    AskQuantity = "2",
                    Bid = "2",
                    Ask = "2",
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


                var createBidList = await bidListsController.CreateBidList(bidListDto);
                int bidListIdFound = 0;
                var bidListFound = await iBidListService.GetAllBidListsDto();
                if (bidListFound is IEnumerable<BidListDto> dtos0)
                {
                    bidListIdFound = dtos0.ElementAt(0).BidListId;
                }


                ///Act

                var getBidListById = await bidListsController.GetBidListById(bidListIdFound);
                if (getBidListById is IEnumerable<BidListDto> dtos1)
                {
                    bidListIdFound = dtos1.ElementAt(0).BidListId;
                }


                bidListDto.BidListId = bidListIdFound;
                ///Assert

                if (getBidListById  is IEnumerable<BidListDto> dtos2)
                {
                    Xunit.Assert.True(dtos2.Count() == 1);
                    Xunit.Assert.True(dtos2.ElementAt(0).Account == bidListDto.Account);
                }
                var del = await bidListsController.DeleteBidListById(bidListIdFound);
            }

            [Fact]
            public async Task IBidListController_GetAllBidLists_ShouldGet_2BidLists()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var bidListrepository = new BidListRepository(context);
                IBidListService iBidListService = new BidListService(bidListrepository, mapper);
                BidListsController bidListsController = new BidListsController(bidListrepository, iBidListService);

                BidListDto bidListDto1 = new BidListDto
                {
                    Account = "test",
                    BidType = "string",
                    BidQuantity = "2",
                    AskQuantity = "2",
                    Bid = "2",
                    Ask = "2",
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
                    Account = "test2",
                    BidType = "string",
                    BidQuantity = "3",
                    AskQuantity = "3",
                    Bid = "3",
                    Ask = "3",
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


                var c1 = await bidListsController.CreateBidList(bidListDto1);
                var c2 = await bidListsController.CreateBidList(bidListDto2);


                ///Act
                var BidListFound = await bidListsController.GetAllBidLists();



                ///Assert
                if (BidListFound is IEnumerable<BidListDto> dtos)
                {
                    int test = dtos.Count();
                    Xunit.Assert.Equal(2, dtos.Count());
                    Xunit.Assert.True(dtos.ElementAt(0).Account == bidListDto1.Account);
                    Xunit.Assert.True(dtos.ElementAt(1).Account == bidListDto2.Account);
                    var del = await bidListsController.DeleteBidListById(dtos.ElementAt(0).BidListId);
                    var del2 = await bidListsController.DeleteBidListById(dtos.ElementAt(1).BidListId);
                }



            }
        }

        public class CurvePointUnitTests
        {
            private P7Referential CreateInMemoryDbContext(string dbName = null)
            {
                var options = new DbContextOptionsBuilder<P7Referential>().UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString()).Options;
                return new P7Referential(options);
            }


            [Fact]
            public async Task ICurvePointController_CreateCurvePoint_ShouldAdd_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                using var context = CreateInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);
                CurvePointsController curvePointsController = new CurvePointsController(curvePointrepository, iCurvePointService);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "2",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };

                ///Act
                var create = await curvePointsController.CreateCurvePoint(curvePointDto);
                var curvePointFound = await iCurvePointService.GetAllCurvePointsDto();
                int curvePointIdFound = 0;
                if (curvePointFound is IEnumerable<CurvePointDto> dtos0)
                {
                    curvePointDto.Id = dtos0.ElementAt(0).Id;
                    curvePointIdFound = dtos0.ElementAt(0).Id;
                    Xunit.Assert.Equivalent(dtos0.ElementAt(0), curvePointDto);
                }
                var del = await curvePointsController.DeleteCurvePointById(curvePointIdFound);
            }

            [Fact]
            public async Task ICurvePointController_CreateCurvePoint_ShouldNotAdd_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);
                CurvePointsController curvePointsController = new CurvePointsController(curvePointrepository, iCurvePointService);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                CurvePointDto wrongCurvePointDto = new CurvePointDto
                {
                    CurveId = "2",
                    AsOfDate = "1",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };
                ///Act
                bool result = curvePointsController.CreateCurvePoint(wrongCurvePointDto).IsFaulted;
                ///Assert
                Xunit.Assert.True(result);

            }

            [Fact]
            public async Task ICurvePointController_UpdateCurvePointById_Should_Modify_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);
                CurvePointsController curvePointsController = new CurvePointsController(curvePointrepository, iCurvePointService);

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "2",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                var createCurvePoint = await curvePointsController.CreateCurvePoint(curvePointDto);
                int curvePointIdFound = 0;
                var curvePointFound = await iCurvePointService.GetAllCurvePointsDto();
                if (curvePointFound is IEnumerable<CurvePointDto> dtos0)
                {
                    curvePointIdFound = dtos0.ElementAt(0).Id;
                }

                CurvePointDto modifiedCurvePointDto = new CurvePointDto
                {
                    Id = curvePointIdFound,
                    CurveId = "3",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                ///Act
                var update = await curvePointsController.UpdateCurvePointById(modifiedCurvePointDto);
                var curvePointFoundAfterModifications = await iCurvePointService.GetCurvePointDtoById(modifiedCurvePointDto.Id);
                if (curvePointFoundAfterModifications is IEnumerable<CurvePointDto> dtos1)
                {
                    curvePointDto.Id = dtos1.ElementAt(0).Id;
                    Xunit.Assert.Equivalent(modifiedCurvePointDto, dtos1.ElementAt(0));
                }
                var del = await curvePointsController.DeleteCurvePointById(curvePointIdFound);
            }

            [Fact]
            public async Task ICurvePointController_DeleteCurvePointById_ShouldDelete_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);
                CurvePointsController curvePointsController = new CurvePointsController(curvePointrepository, iCurvePointService);

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "2",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };

                var create = await curvePointsController.CreateCurvePoint(curvePointDto);
                var curvePointFound = await iCurvePointService.GetAllCurvePointsDto();
                int curvePointIdFound = 0;
                if (curvePointFound is IEnumerable<CurvePointDto> dtos0)
                {
                    curvePointIdFound = dtos0.ElementAt(0).Id;
                }
                ///Act
                var deleteCurvePoint = await curvePointsController.DeleteCurvePointById(curvePointIdFound);
                ///Assert
                var curvePointFoundAfterDelete = await iCurvePointService.GetCurvePointDtoById(curvePointIdFound);
                if (curvePointFoundAfterDelete is OkObjectResult ok1 && ok1.Value is IEnumerable<CurvePointDto> dtos1)
                {
                    Xunit.Assert.True(curvePointFoundAfterDelete == null);
                }


            }

            [Fact]
            public async Task ICurvePointController_GetCurvePointDtoById_ShouldGet_1CurvePointById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);
                CurvePointsController curvePointsController = new CurvePointsController(curvePointrepository, iCurvePointService);

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "2",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                var createCurvePoint = await curvePointsController.CreateCurvePoint(curvePointDto);
                int curvePointIdFound = 0;
                var curvePointFound = await iCurvePointService.GetAllCurvePointsDto();
                if (curvePointFound is IEnumerable<CurvePointDto> dtos0)
                {
                    curvePointIdFound = dtos0.ElementAt(0).Id;
                }


                ///Act

                var getCurvePointById = await curvePointsController.GetCurvePointById(curvePointIdFound);
                if (getCurvePointById is OkObjectResult ok1 && ok1.Value is IEnumerable<CurvePointDto> dtos1)
                {
                    curvePointIdFound = dtos1.ElementAt(0).Id;
                }


                curvePointDto.Id = curvePointIdFound;
                ///Assert

                if (getCurvePointById is IEnumerable<CurvePointDto> dtos2)
                {
                    Xunit.Assert.True(dtos2.Count() == 1);
                    Xunit.Assert.True(dtos2.ElementAt(0).AsOfDate == curvePointDto.AsOfDate);
                }
                var del = await curvePointsController.DeleteCurvePointById(curvePointIdFound);
            }

            [Fact]
            public async Task ICurvePointController_GetAllCurvePoints_ShouldGet_2CurvePoints()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);
                CurvePointsController curvePointsController = new CurvePointsController(curvePointrepository, iCurvePointService);

                CurvePointDto curvePointDto1 = new CurvePointDto
                {
                    CurveId = "2",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                CurvePointDto curvePointDto2 = new CurvePointDto
                {
                    CurveId = "2",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                var c1 = await curvePointsController.CreateCurvePoint(curvePointDto1);
                var c2 = await curvePointsController.CreateCurvePoint(curvePointDto2);


                ///Act
                var CurvePointFound = await curvePointsController.GetAllCurvePoints();



                ///Assert
                if (CurvePointFound is IEnumerable<CurvePointDto> dtos)
                {
                    int test = dtos.Count();
                    Xunit.Assert.Equal(2, dtos.Count());
                    Xunit.Assert.True(dtos.ElementAt(0).AsOfDate == curvePointDto1.AsOfDate);
                    Xunit.Assert.True(dtos.ElementAt(1).AsOfDate == curvePointDto2.AsOfDate);
                    var del = await curvePointsController.DeleteCurvePointById(dtos.ElementAt(0).Id);
                    var del2 = await curvePointsController.DeleteCurvePointById(dtos.ElementAt(1).Id);
                }



            
            }
        }


        public class RatingUnitTests
        {
            private P7Referential CreateInMemoryDbContext(string dbName = null)
            {
                var options = new DbContextOptionsBuilder<P7Referential>().UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString()).Options;
                return new P7Referential(options);
            }
            [Fact]
            public async Task IRatingController_CreateRating_ShouldAdd_1Rating()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                using var context = CreateInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);
                RatingsController ratingsController = new RatingsController(ratingrepository, iRatingService);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };

                ///Act
                var create = await ratingsController.CreateRating(ratingDto);
                var ratingFound = await iRatingService.GetAllRatingsDto();
                int ratingIdFound = 0;
                if (ratingFound is IEnumerable<RatingDto> dtos0)
                {
                    ratingDto.Id = dtos0.ElementAt(0).Id;
                    ratingIdFound = dtos0.ElementAt(0).Id;
                    Xunit.Assert.Equivalent(dtos0.ElementAt(0), ratingDto);
                }
                var del = await ratingsController.DeleteRatingById(ratingIdFound);
            }

          

            [Fact]
            public async Task IRatingController_UpdateRatingById_Should_Modify_1Rating()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);
                RatingsController ratingsController = new RatingsController(ratingrepository, iRatingService);

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                var createRating = await ratingsController.CreateRating(ratingDto);
                int ratingIdFound = 0;
                var ratingFound = await iRatingService.GetAllRatingsDto();
                if (ratingFound is IEnumerable<RatingDto> dtos0)
                {
                    ratingIdFound = dtos0.ElementAt(0).Id;
                }

                RatingDto modifiedRatingDto = new RatingDto
                {
                    Id = ratingIdFound,
                    MoodysRating = "AAA",
                    FitchRating = "BBB",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                ///Act
                var update = await ratingsController.UpdateRatingById(modifiedRatingDto);
                var ratingFoundAfterModifications = await iRatingService.GetRatingDtoById(modifiedRatingDto.Id);
                if (ratingFoundAfterModifications is IEnumerable<RatingDto> dtos1)
                {
                    ratingDto.Id = dtos1.ElementAt(0).Id;
                    Xunit.Assert.Equivalent(modifiedRatingDto, dtos1.ElementAt(0));
                }
                var del = await ratingsController.DeleteRatingById(ratingIdFound);
            }

            [Fact]
            public async Task IRatingController_DeleteRatingById_ShouldDelete_1Rating()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);
                RatingsController ratingsController = new RatingsController(ratingrepository, iRatingService);

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };

                var create = await ratingsController.CreateRating(ratingDto);
                var ratingFound = await iRatingService.GetAllRatingsDto();
                int ratingIdFound = 0;
                if (ratingFound is IEnumerable<RatingDto> dtos0)
                {
                    ratingIdFound = dtos0.ElementAt(0).Id;
                }
                ///Act
                var deleteRating = await ratingsController.DeleteRatingById(ratingIdFound);
                ///Assert
                var ratingFoundAfterDelete = await iRatingService.GetRatingDtoById(ratingIdFound);
                if (ratingFoundAfterDelete is OkObjectResult ok1 && ok1.Value is IEnumerable<RatingDto> dtos1)
                {
                    Xunit.Assert.True(ratingFoundAfterDelete == null);
                }


            }

            [Fact]
            public async Task IRatingController_GetRatingDtoById_ShouldGet_1RatingById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);
                RatingsController ratingsController = new RatingsController(ratingrepository, iRatingService);

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                var createRating = await ratingsController.CreateRating(ratingDto);
                int ratingIdFound = 0;
                var ratingFound = await iRatingService.GetAllRatingsDto();
                if (ratingFound is IEnumerable<RatingDto> dtos0)
                {
                    ratingIdFound = dtos0.ElementAt(0).Id;
                }


                ///Act

                var getRatingById = await ratingsController.GetRatingById(ratingIdFound);
                if (getRatingById is OkObjectResult ok1 && ok1.Value is IEnumerable<RatingDto> dtos1)
                {
                    ratingIdFound = dtos1.ElementAt(0).Id;
                }


                ratingDto.Id = ratingIdFound;
                ///Assert

                if (getRatingById  is IEnumerable<RatingDto> dtos2)
                {
                    Xunit.Assert.True(dtos2.Count() == 1);
                    Xunit.Assert.True(dtos2.ElementAt(0).Id == ratingDto.Id);
                }
                var del = await ratingsController.DeleteRatingById(ratingIdFound);
            }

            [Fact]
            public async Task IRatingController_GetAllRatings_ShouldGet_2Ratings()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = CreateInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);
                RatingsController ratingsController = new RatingsController(ratingrepository, iRatingService);

                RatingDto ratingDto1 = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                RatingDto ratingDto2 = new RatingDto
                {
                    MoodysRating = "BBB",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                var c1 = await ratingsController.CreateRating(ratingDto1);
                var c2 = await ratingsController.CreateRating(ratingDto2);


                ///Act
                var RatingFound = await ratingsController.GetAllRatings();



                ///Assert
                if (RatingFound is IEnumerable<RatingDto> dtos)
                {
                    int test = dtos.Count();
                    Xunit.Assert.Equal(2, dtos.Count());
                    Xunit.Assert.True(dtos.ElementAt(0).MoodysRating == ratingDto1.MoodysRating);
                    Xunit.Assert.True(dtos.ElementAt(1).MoodysRating == ratingDto2.MoodysRating);
                    var del = await ratingsController.DeleteRatingById(dtos.ElementAt(0).Id);
                    var del2 = await ratingsController.DeleteRatingById(dtos.ElementAt(1).Id);
                }



            }

            public class RuleNameUnitTests
            {
                private P7Referential CreateInMemoryDbContext(string dbName = null)
                {
                    var options = new DbContextOptionsBuilder<P7Referential>().UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString()).Options;
                    return new P7Referential(options);
                }
                [Fact]
                public async Task IRuleNameController_CreateRuleName_ShouldAdd_1RuleName()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    using var context = CreateInMemoryDbContext();
                    var ruleNamerepository = new RuleNameRepository(context);
                    IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);
                    RuleNamesController ruleNamesController = new RuleNamesController(ruleNamerepository, iRuleNameService);
                    CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                    RuleNameDto ruleNameDto = new RuleNameDto
                    {
                        Name = "test",
                        Description = "testdescription",
                        Json = "Json",
                        Template = "Template",
                        SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                        SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    };

                    ///Act
                    var create = await ruleNamesController.CreateRuleName(ruleNameDto);
                    var ruleNameFound = await iRuleNameService.GetAllRuleNamesDto();
                    int ruleNameIdFound = 0;
                    if (ruleNameFound is IEnumerable<RuleNameDto> dtos0)
                    {
                        ruleNameDto.Id = dtos0.ElementAt(0).Id;
                        ruleNameIdFound = dtos0.ElementAt(0).Id;
                        Xunit.Assert.Equivalent(dtos0.ElementAt(0), ruleNameDto);
                    }
                    var del = await ruleNamesController.DeleteRuleNameById(ruleNameIdFound);
                }



                [Fact]
                public async Task IRuleNameController_UpdateRuleNameById_Should_Modify_1RuleName()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var ruleNamerepository = new RuleNameRepository(context);
                    IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);
                    RuleNamesController ruleNamesController = new RuleNamesController(ruleNamerepository, iRuleNameService);

                    RuleNameDto ruleNameDto = new RuleNameDto
                    {
                        Name = "test",
                        Description = "testdescription",
                        Json = "Json",
                        Template = "Template",
                        SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                        SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    };


                    var createRuleName = await ruleNamesController.CreateRuleName(ruleNameDto);
                    int ruleNameIdFound = 0;
                    var ruleNameFound = await iRuleNameService.GetAllRuleNamesDto();
                    if (ruleNameFound is IEnumerable<RuleNameDto> dtos0)
                    {
                        ruleNameIdFound = dtos0.ElementAt(0).Id;
                    }

                    RuleNameDto modifiedRuleNameDto = new RuleNameDto
                    {
                        Id = ruleNameIdFound,
                        Name = "test23",
                        Description = "testdescription",
                        Json = "Json",
                        Template = "Template",
                        SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                        SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    };


                    ///Act
                    var update = await ruleNamesController.UpdateRuleNameById(modifiedRuleNameDto);
                    var ruleNameFoundAfterModifications = await iRuleNameService.GetRuleNameDtoById(modifiedRuleNameDto.Id);
                    if (ruleNameFoundAfterModifications is IEnumerable<RuleNameDto> dtos1)
                    {
                        ruleNameDto.Id = dtos1.ElementAt(0).Id;
                        Xunit.Assert.Equivalent(modifiedRuleNameDto, dtos1.ElementAt(0));
                    }
                    var del = await ruleNamesController.DeleteRuleNameById(ruleNameIdFound);
                }

                [Fact]
                public async Task IRuleNameController_DeleteRuleNameById_ShouldDelete_1RuleName()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var ruleNamerepository = new RuleNameRepository(context);
                    IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);
                    RuleNamesController ruleNamesController = new RuleNamesController(ruleNamerepository, iRuleNameService);

                    RuleNameDto ruleNameDto = new RuleNameDto
                    {
                        Name = "test",
                        Description = "testdescription",
                        Json = "Json",
                        Template = "Template",
                        SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                        SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    };

                    var create = await ruleNamesController.CreateRuleName(ruleNameDto);
                    var ruleNameFound = await iRuleNameService.GetAllRuleNamesDto();
                    int ruleNameIdFound = 0;
                    if (ruleNameFound is IEnumerable<RuleNameDto> dtos0)
                    {
                        ruleNameIdFound = dtos0.ElementAt(0).Id;
                    }
                    ///Act
                    var deleteRuleName = await ruleNamesController.DeleteRuleNameById(ruleNameIdFound);
                    ///Assert
                    var ruleNameFoundAfterDelete = await iRuleNameService.GetRuleNameDtoById(ruleNameIdFound);
                    if (ruleNameFoundAfterDelete is OkObjectResult ok1 && ok1.Value is IEnumerable<RuleNameDto> dtos1)
                    {
                        Xunit.Assert.True(ruleNameFoundAfterDelete == null);
                    }


                }

                [Fact]
                public async Task IRuleNameController_GetRuleNameDtoById_ShouldGet_1RuleNameById()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var ruleNamerepository = new RuleNameRepository(context);
                    IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);
                    RuleNamesController ruleNamesController = new RuleNamesController(ruleNamerepository, iRuleNameService);

                    RuleNameDto ruleNameDto = new RuleNameDto
                    {
                        Name = "test",
                        Description = "testdescription",
                        Json = "Json",
                        Template = "Template",
                        SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                        SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    };


                    var createRuleName = await ruleNamesController.CreateRuleName(ruleNameDto);
                    int ruleNameIdFound = 0;
                    var ruleNameFound = await iRuleNameService.GetAllRuleNamesDto();
                    if (ruleNameFound is IEnumerable<RuleNameDto> dtos0)
                    {
                        ruleNameIdFound = dtos0.ElementAt(0).Id;
                    }


                    ///Act

                    var getRuleNameById = await ruleNamesController.GetRuleNameById(ruleNameIdFound);
                    if (getRuleNameById is OkObjectResult ok1 && ok1.Value is IEnumerable<RuleNameDto> dtos1)
                    {
                        ruleNameIdFound = dtos1.ElementAt(0).Id;
                    }


                    ruleNameDto.Id = ruleNameIdFound;
                    ///Assert

                    if (getRuleNameById is IEnumerable<RuleNameDto> dtos2)
                    {
                        Xunit.Assert.True(dtos2.Count() == 1);
                        Xunit.Assert.True(dtos2.ElementAt(0).Id == ruleNameDto.Id);
                    }
                    var del = await ruleNamesController.DeleteRuleNameById(ruleNameIdFound);
                }

                [Fact]
                public async Task IRuleNameController_GetAllRuleNames_ShouldGet_2RuleNames()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var ruleNamerepository = new RuleNameRepository(context);
                    IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);
                    RuleNamesController ruleNamesController = new RuleNamesController(ruleNamerepository, iRuleNameService);

                    RuleNameDto ruleNameDto1 = new RuleNameDto
                    {
                        Name = "test",
                        Description = "testdescription",
                        Json = "Json",
                        Template = "Template",
                        SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                        SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    };


                    RuleNameDto ruleNameDto2 = new RuleNameDto
                    {
                        Name = "test2",
                        Description = "testdescription",
                        Json = "Json",
                        Template = "Template",
                        SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                        SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    };


                    var c1 = await ruleNamesController.CreateRuleName(ruleNameDto1);
                    var c2 = await ruleNamesController.CreateRuleName(ruleNameDto2);


                    ///Act
                    var RuleNameFound = await ruleNamesController.GetAllRuleNames();



                    ///Assert
                    if (RuleNameFound is IEnumerable<RuleNameDto> dtos)
                    {
                        int test = dtos.Count();
                        Xunit.Assert.Equal(2, dtos.Count());
                        Xunit.Assert.True(dtos.ElementAt(0).Name == ruleNameDto1.Name);
                        Xunit.Assert.True(dtos.ElementAt(1).Name == ruleNameDto2.Name);
                        var del = await ruleNamesController.DeleteRuleNameById(dtos.ElementAt(0).Id);
                        var del2 = await ruleNamesController.DeleteRuleNameById(dtos.ElementAt(1).Id);
                    }



                }

            }


















            public class TradeUnitTests
            {
                private P7Referential CreateInMemoryDbContext(string dbName = null)
                {
                    var options = new DbContextOptionsBuilder<P7Referential>().UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString()).Options;
                    return new P7Referential(options);
                }
                [Fact]
                public async Task ITradeController_CreateTrade_ShouldAdd_1Trade()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    using var context = CreateInMemoryDbContext();
                    var traderepository = new TradeRepository(context);
                    ITradeService iTradeService = new TradeService(traderepository, mapper);
                    TradesController tradesController = new TradesController(traderepository, iTradeService);
                    CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                    TradeDto tradeDto = new TradeDto
                    {
                        Account = "string2test",
                        AccountType = "string",
                        BuyQuantity = "1",
                        SellQuantity = "1",
                        BuyPrice = "1",
                        SellPrice = "1",
                        TradeDate = "12/09/2026 09:53:52",
                        TradeSecurity = "string",
                        TradeStatus = "string",
                        Trader = "string",
                        Benchmark = "string",
                        Book = "string",
                        CreationName = "string",
                        CreationDate = "12/09/2026 09:53:52",
                        RevisionName = "string",
                        RevisionDate = "12/09/2026 09:53:52",
                        DealName = "string"
                    };

                    ///Act
                    var create = await tradesController.CreateTrade(tradeDto);
                    var tradeFound = await iTradeService.GetAllTradesDto();
                    int tradeIdFound = 0;
                    if (tradeFound is IEnumerable<TradeDto> dtos0)
                    {
                        tradeDto.TradeId = dtos0.ElementAt(0).TradeId;
                        tradeIdFound = dtos0.ElementAt(0).TradeId;
                        Xunit.Assert.Equivalent(dtos0.ElementAt(0), tradeDto);
                    }
                    var del = await tradesController.DeleteTradeById(tradeIdFound);
                }



                [Fact]
                public async Task ITradeController_UpdateTradeById_Should_Modify_1Trade()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var traderepository = new TradeRepository(context);
                    ITradeService iTradeService = new TradeService(traderepository, mapper);
                    TradesController tradesController = new TradesController(traderepository, iTradeService);

                    TradeDto tradeDto = new TradeDto
                    {
                        Account = "string2test",
                        AccountType = "string",
                        BuyQuantity = "1",
                        SellQuantity = "1",
                        BuyPrice = "1",
                        SellPrice = "1",
                        TradeDate = "12/09/2026 09:53:52",
                        TradeSecurity = "string",
                        TradeStatus = "string",
                        Trader = "string",
                        Benchmark = "string",
                        Book = "string",
                        CreationName = "string",
                        CreationDate = "12/09/2026 09:53:52",
                        RevisionName = "string",
                        RevisionDate = "12/09/2026 09:53:52",
                        DealName = "string"
                    };


                    var createTrade = await tradesController.CreateTrade(tradeDto);
                    int tradeIdFound = 0;
                    var tradeFound = await iTradeService.GetAllTradesDto();
                    if (tradeFound is IEnumerable<TradeDto> dtos0)
                    {
                        tradeIdFound = dtos0.ElementAt(0).TradeId;
                    }

                    TradeDto modifiedTradeDto = new TradeDto
                    {
                        TradeId = tradeIdFound,
                        Account = "string2test2",
                        AccountType = "string",
                        BuyQuantity = "1",
                        SellQuantity = "1",
                        BuyPrice = "1",
                        SellPrice = "1",
                        TradeDate = "12/09/2026 09:53:52",
                        TradeSecurity = "string",
                        TradeStatus = "string",
                        Trader = "string",
                        Benchmark = "string",
                        Book = "string",
                        CreationName = "string",
                        CreationDate = "12/09/2026 09:53:52",
                        RevisionName = "string",
                        RevisionDate = "12/09/2026 09:53:52",
                        DealName = "string"
                    };


                    ///Act
                    var update = await tradesController.UpdateTradeById(modifiedTradeDto);
                    var tradeFoundAfterModifications = await iTradeService.GetTradeDtoById(modifiedTradeDto.TradeId);
                    if (tradeFoundAfterModifications is IEnumerable<TradeDto> dtos1)
                    {
                        tradeDto.TradeId = dtos1.ElementAt(0).TradeId;
                        Xunit.Assert.Equivalent(modifiedTradeDto, dtos1.ElementAt(0));
                    }
                    var del = await tradesController.DeleteTradeById(tradeIdFound);
                }

                [Fact]
                public async Task ITradeController_DeleteTradeById_ShouldDelete_1Trade()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var traderepository = new TradeRepository(context);
                    ITradeService iTradeService = new TradeService(traderepository, mapper);
                    TradesController tradesController = new TradesController(traderepository, iTradeService);

                    TradeDto tradeDto = new TradeDto
                    {
                        Account = "string2test",
                        AccountType = "string",
                        BuyQuantity = "1",
                        SellQuantity = "1",
                        BuyPrice = "1",
                        SellPrice = "1",
                        TradeDate = "12/09/2026 09:53:52",
                        TradeSecurity = "string",
                        TradeStatus = "string",
                        Trader = "string",
                        Benchmark = "string",
                        Book = "string",
                        CreationName = "string",
                        CreationDate = "12/09/2026 09:53:52",
                        RevisionName = "string",
                        RevisionDate = "12/09/2026 09:53:52",
                        DealName = "string"
                    };

                    var create = await tradesController.CreateTrade(tradeDto);
                    var tradeFound = await iTradeService.GetAllTradesDto();
                    int tradeIdFound = 0;
                    if (tradeFound is IEnumerable<TradeDto> dtos0)
                    {
                        tradeIdFound = dtos0.ElementAt(0).TradeId;
                    }
                    ///Act
                    var deleteTrade = await tradesController.DeleteTradeById(tradeIdFound);
                    ///Assert
                    var tradeFoundAfterDelete = await iTradeService.GetTradeDtoById(tradeIdFound);
                    if (tradeFoundAfterDelete is OkObjectResult ok1 && ok1.Value is IEnumerable<TradeDto> dtos1)
                    {
                        Xunit.Assert.True(tradeFoundAfterDelete == null);
                    }


                }

                [Fact]
                public async Task ITradeController_GetTradeDtoById_ShouldGet_1TradeById()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var traderepository = new TradeRepository(context);
                    ITradeService iTradeService = new TradeService(traderepository, mapper);
                    TradesController tradesController = new TradesController(traderepository, iTradeService);

                    TradeDto tradeDto = new TradeDto
                    {
                        Account = "string2test",
                        AccountType = "string",
                        BuyQuantity = "1",
                        SellQuantity = "1",
                        BuyPrice = "1",
                        SellPrice = "1",
                        TradeDate = "12/09/2026 09:53:52",
                        TradeSecurity = "string",
                        TradeStatus = "string",
                        Trader = "string",
                        Benchmark = "string",
                        Book = "string",
                        CreationName = "string",
                        CreationDate = "12/09/2026 09:53:52",
                        RevisionName = "string",
                        RevisionDate = "12/09/2026 09:53:52",
                        DealName = "string"
                    };


                    var createTrade = await tradesController.CreateTrade(tradeDto);
                    int tradeIdFound = 0;
                    var tradeFound = await iTradeService.GetAllTradesDto();
                    if (tradeFound is IEnumerable<TradeDto> dtos0)
                    {
                        tradeIdFound = dtos0.ElementAt(0).TradeId;
                    }


                    ///Act

                    var getTradeById = await tradesController.GetTradeById(tradeIdFound);
                    if (getTradeById is OkObjectResult ok1 && ok1.Value is IEnumerable<TradeDto> dtos1)
                    {
                        tradeIdFound = dtos1.ElementAt(0).TradeId;
                    }


                    tradeDto.TradeId = tradeIdFound;
                    ///Assert

                    if (getTradeById is IEnumerable<TradeDto> dtos2)
                    {
                        Xunit.Assert.True(dtos2.Count() == 1);
                        Xunit.Assert.True(dtos2.ElementAt(0).TradeId == tradeDto.TradeId);
                    }
                    var del = await tradesController.DeleteTradeById(tradeIdFound);
                }

                [Fact]
                public async Task ITradeController_GetAllTrades_ShouldGet_2Trades()
                {
                    /// Arrange
                    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<DtoProfile>();
                    }, loggerFactory);
                    IMapper mapper = config.CreateMapper();

                    var context = CreateInMemoryDbContext();
                    var traderepository = new TradeRepository(context);
                    ITradeService iTradeService = new TradeService(traderepository, mapper);
                    TradesController tradesController = new TradesController(traderepository, iTradeService);

                    TradeDto tradeDto1 = new TradeDto
                    {
                        Account = "string2test",
                        AccountType = "string",
                        BuyQuantity = "1",
                        SellQuantity = "1",
                        BuyPrice = "1",
                        SellPrice = "1",
                        TradeDate = "12/09/2026 09:53:52",
                        TradeSecurity = "string",
                        TradeStatus = "string",
                        Trader = "string",
                        Benchmark = "string",
                        Book = "string",
                        CreationName = "string",
                        CreationDate = "12/09/2026 09:53:52",
                        RevisionName = "string",
                        RevisionDate = "12/09/2026 09:53:52",
                        DealName = "string"
                    };


                    TradeDto tradeDto2 = new TradeDto
                    {
                        Account = "string2test",
                        AccountType = "string2",
                        BuyQuantity = "1",
                        SellQuantity = "1",
                        BuyPrice = "1",
                        SellPrice = "1",
                        TradeDate = "12/09/2026 09:53:52",
                        TradeSecurity = "string",
                        TradeStatus = "string",
                        Trader = "string",
                        Benchmark = "string",
                        Book = "string",
                        CreationName = "string",
                        CreationDate = "12/09/2026 09:53:52",
                        RevisionName = "string",
                        RevisionDate = "12/09/2026 09:53:52",
                        DealName = "string"
                    };


                    var c1 = await tradesController.CreateTrade(tradeDto1);
                    var c2 = await tradesController.CreateTrade(tradeDto2);


                    ///Act
                    var TradeFound = await tradesController.GetAllTrades();



                    ///Assert
                    if (TradeFound is IEnumerable<TradeDto> dtos)
                    {
                        int test = dtos.Count();
                        Xunit.Assert.Equal(2, dtos.Count());
                        Xunit.Assert.True(dtos.ElementAt(0).AccountType == tradeDto1.AccountType);
                        Xunit.Assert.True(dtos.ElementAt(1).AccountType == tradeDto2.AccountType);
                        var del = await tradesController.DeleteTradeById(dtos.ElementAt(0).TradeId);
                        var del2 = await tradesController.DeleteTradeById(dtos.ElementAt(1).TradeId);
                    }



                }

            }

        }

    }
}



















