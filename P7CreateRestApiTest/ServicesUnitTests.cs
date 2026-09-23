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


namespace P7CreateRestApiTest
{

    /// <Summary>
    /// Creating an SQL datatabse context for all tests needing sql
    /// <Summary>
    public class DatabaseFixture
    {



        public class BidListUnitTests
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
                //bool result = customValidationAttribute.IsValid(bidListDto);
                Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                int bidListIdFound = iBidListService.GetAllBidListsDto().Result.Select(b => b.BidListId).Last();
                BidListDto foundBidListDto = iBidListService.GetBidListDtoById(bidListIdFound).Result.FirstOrDefault();
                bidListDto.BidListId = bidListIdFound;

                ///Assert
                //Xunit.Assert.True(result);
                Xunit.Assert.True(createBidList.IsCompletedSuccessfully);
                Xunit.Assert.Equivalent(bidListDto, foundBidListDto);

                await iBidListService.DeleteBidListById(bidListIdFound);

            }

            [Fact]
            public async Task IBidListService_CreateBidListWithWrongBidListDto_ShouldNotAdd_1BidList()
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
                Task createBidList = iBidListService.CreateBidListWithBidListDto(wrongBidListDto);
                bool foundBidListDto = (iBidListService.GetAllBidListsDto().Result.Select(b => b).Count() > 0);

                ///Assert
                Xunit.Assert.False(createBidList.IsCompletedSuccessfully);
                Xunit.Assert.True(createBidList.IsFaulted);
                Xunit.Assert.False(foundBidListDto);
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


                Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                int bidListIdFound = iBidListService.GetAllBidListsDto().Result.Select(b => b.BidListId).Last();

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
                await iBidListService.UpdateBidListWithBidListDto(modifiedBidListDto);
                BidListDto modifiedFoundBidListDto = iBidListService.GetBidListDtoById(bidListIdFound).Result.FirstOrDefault();
                bidListDto.BidListId = bidListIdFound;

                ///Assert
                Xunit.Assert.Equivalent(modifiedBidListDto, modifiedFoundBidListDto);

                await iBidListService.DeleteBidListById(bidListIdFound);

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

                Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                //int id = await bidListrepository.GetMaxBidListId();
                int bidListIdFound = iBidListService.GetAllBidListsDto().Result.Select(b => b.BidListId).Last();

                Xunit.Assert.True(createBidList.IsCompletedSuccessfully);
                ///Act
                Task deleteBidList = iBidListService.DeleteBidListById(bidListIdFound);
                ///Assert
                Xunit.Assert.True(iBidListService.GetBidListDtoById(bidListIdFound).Result.FirstOrDefault() == null);
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


                Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
                //int id = await bidListrepository.GetMaxBidListId();
                int bidListIdFound = iBidListService.GetAllBidListsDto().Result.Select(b => b.BidListId).Last();
                BidListDto FoundBidListDto = iBidListService.GetBidListDtoById(bidListIdFound).Result.FirstOrDefault();

                ///Act
                bidListDto.BidListId = bidListIdFound;
                Task getBidListById = bidListrepository.GetBidListById(bidListIdFound);

                ///Assert
                Xunit.Assert.Equivalent(bidListDto, FoundBidListDto);
                Xunit.Assert.True(createBidList.IsCompletedSuccessfully);
                Xunit.Assert.True(getBidListById.IsCompletedSuccessfully);

                await iBidListService.DeleteBidListById(bidListIdFound);
            }

            [Fact]
            public async Task IBidListService_GetAllBidLists_ShouldGet_2BidLists()
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


                Task createBidList1 = iBidListService.CreateBidListWithBidListDto(bidListDto1);
                Task createBidList2 = iBidListService.CreateBidListWithBidListDto(bidListDto2);
                //int id = await bidListrepository.GetMaxBidListId();
                List<int> BidListFoundids = iBidListService.GetAllBidListsDto().Result.Select(b => b.BidListId).ToList();


                ///Act

                var getAllBidLists = iBidListService.GetAllBidListsDto().Result.Where(b => b.BidListId > 0);

                ///Assert
                Xunit.Assert.True(createBidList1.IsCompletedSuccessfully);
                Xunit.Assert.True(createBidList2.IsCompletedSuccessfully);
                Xunit.Assert.True(getAllBidLists.Count() == 2);
                Xunit.Assert.True(getAllBidLists.ElementAt(0).BidListId == BidListFoundids[0]);
                Xunit.Assert.True(getAllBidLists.ElementAt(1).BidListId == BidListFoundids[1]);

                await iBidListService.DeleteBidListById(BidListFoundids[0]);
                await iBidListService.DeleteBidListById(BidListFoundids[1]);


            }


            [Fact]
            public async Task AsyncActionFilter_WithWrongBidListDto_ShouldReturn_8Errors()
            {
                BidListDto wrongBidListDto = new BidListDto
                {
                    BidListId = 0,
                    Account = "test2",
                    BidType = "abc",
                    BidQuantity = "abc",
                    AskQuantity = "abcd",
                    Bid = "abcde",
                    Ask = "abcde",
                    Benchmark = "string",
                    BidListDate = "erhk",
                    Commentary = "string",
                    BidSecurity = "string",
                    BidStatus = "string",
                    Trader = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "kjg",
                    RevisionName = "string",
                    RevisionDate = "ioj",
                    DealName = "string",
                    DealType = "string",
                    SourceListId = "string",
                    Side = "string"
                };


                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(wrongBidListDto);
                Validator.TryValidateObject(wrongBidListDto, validationContext, validationResults, validateAllProperties: true);


                var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
                foreach (var vr in validationResults)
                {
                    var key = vr.MemberNames.FirstOrDefault() ?? string.Empty;
                    modelState.AddModelError(key, vr.ErrorMessage);
                }

                var httpContext = new DefaultHttpContext();
                var routeData = new Microsoft.AspNetCore.Routing.RouteData();
                routeData.Values["controller"] = "MyController";
                routeData.Values["action"] = "Create";
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor(), modelState);
                var actionExecutingContext = new ActionExecutingContext(
                    actionContext,
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object>(),
                    controller: null
                );


                ActionExecutionDelegate next = () => Task.FromResult<ActionExecutedContext>(
                    new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller: null)
                );
                var filter = new AsyncActionFilter();
                await filter.OnActionExecutionAsync(actionExecutingContext, next);
                var error0 = modelState.ElementAt(0);
                var error1 = modelState.ElementAt(1);
                var error2 = modelState.ElementAt(2);
                var error3 = modelState.ElementAt(3);
                var error4 = modelState.ElementAt(4);
                var error5 = modelState.ElementAt(5);
                var error6 = modelState.ElementAt(6);

                // Assert
                Xunit.Assert.Equal("The Ask is not greater than 0 and smaller than 1.79 E+308.", error0.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The Ask is not a double.", error0.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The Bid is not greater than 0 and smaller than 1.79 E+308.", error1.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The Bid is not a double.", error1.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The AskQuantity is not greater than 0 and smaller than 1.79 E+308.", error2.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The AskQuantity is not a double.", error2.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The BidListDate does not suit the DateTime format.", error3.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The BidQuantity is not greater than 0 and smaller than 1.79 E+308.", error4.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The BidQuantity is not a double.", error4.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The CreationDate does not suit the DateTime format.", error5.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The RevisionDate does not suit the DateTime format.", error6.Value.Errors[0].ErrorMessage);
                Xunit.Assert.True(modelState.ErrorCount == 11);
            }


            //    [Fact]
            //    public async Task IBidListService_CreateBidListWithBidListDto_ShouldReturn_1Error()
            //    {
            //        /// Arrange
            //        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
            //        var config = new MapperConfiguration(cfg =>
            //        {
            //            cfg.AddProfile<DtoProfile>();
            //        }, loggerFactory);
            //        IMapper mapper = config.CreateMapper();

            //        var context = GetInMemoryDbContext();
            //        var bidListrepository = new BidListRepository(context);
            //        IBidListService iBidListService = new BidListService(bidListrepository, mapper);
            //        CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

            //        BidListDto bidListDto = new BidListDto
            //        {
            //            BidListId = 0,
            //            Account = "test",
            //            BidType = "string",
            //            BidQuantity = "abc",
            //            AskQuantity = "0,0001",
            //            Bid = "0,0001",
            //            Ask = "0,0001",
            //            Benchmark = "string",
            //            BidListDate = "11/08/2026 12:53:27",
            //            Commentary = "string",
            //            BidSecurity = "string",
            //            BidStatus = "string",
            //            Trader = "string",
            //            Book = "string",
            //            CreationName = "string",
            //            CreationDate = "11/08/2026 12:53:27",
            //            RevisionName = "string",
            //            RevisionDate = "11/08/2026 12:53:27",
            //            DealName = "string",
            //            DealType = "string",
            //            SourceListId = "string",
            //            Side = "string"
            //        };

            //        ///Act
            //        //bool result = customValidationAttribute.IsValid(bidListDto);
            //        Task createBidList = iBidListService.CreateBidListWithBidListDto(bidListDto);
            //        //Task filterModelState = customValidationAttribute.;
            //        int bidListIdFound = iBidListService.GetAllBidListsDto().Result.Select(b => b.BidListId).Last();
            //        BidListDto foundBidListDto = iBidListService.GetBidListDtoById(bidListIdFound).Result.FirstOrDefault();
            //        bidListDto.BidListId = bidListIdFound;

            //        ///Assert
            //        //Xunit.Assert.True(result);
            //        Xunit.Assert.True(createBidList.IsCompletedSuccessfully);
            //        Xunit.Assert.Equivalent(bidListDto, foundBidListDto);

            //        await iBidListService.DeleteBidListById(bidListIdFound);

            //    }
            //}

        }
        public class CurvePointUnitTests
        {
            private P7Referential GetInMemoryDbContext()
            {
                var options = new DbContextOptionsBuilder<P7Referential>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase2")
                    .Options;
                return new P7Referential(options);
            }



            [Fact]
            public async Task ICurvePointService_CreateCurvePointWithCurvePointDto_ShouldAdd_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "1",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };

                ///Act
                //bool result = customValidationAttribute.IsValid(curvePointDto);
                Task createCurvePoint = iCurvePointService.CreateCurvePointWithCurvePointDto(curvePointDto);
                int curvePointIdFound = iCurvePointService.GetAllCurvePointsDto().Result.Select(c => c.Id).Last();
                CurvePointDto foundCurvePointDto = iCurvePointService.GetCurvePointDtoById(curvePointIdFound).Result.FirstOrDefault();
                curvePointDto.Id = curvePointIdFound;

                ///Assert
                //Xunit.Assert.True(result);
                Xunit.Assert.True(createCurvePoint.IsCompletedSuccessfully);
                Xunit.Assert.Equivalent(curvePointDto, foundCurvePointDto);

                await iCurvePointService.DeleteCurvePointById(curvePointIdFound);

            }

            [Fact]
            public async Task IBidListService_CreateBidListWithWrongCurvePointDto_ShouldNotAdd_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var CurvePointRepository = new CurvePointRepository(context);
                ICurvePointService curvePointService = new CurvePointService(CurvePointRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                CurvePointDto wrongCurvePointDto = new CurvePointDto
                {
                    CurveId = "1",
                    AsOfDate = "f",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "g",
                };


                ///Act
                Task createCurvePoint = curvePointService.CreateCurvePointWithCurvePointDto(wrongCurvePointDto);
                bool foundCurvePointDto = (curvePointService.GetAllCurvePointsDto().Result.Select(c => c).Count() > 0);

                ///Assert
                Xunit.Assert.False(createCurvePoint.IsCompletedSuccessfully);
                Xunit.Assert.True(createCurvePoint.IsFaulted);
                Xunit.Assert.False(foundCurvePointDto);
            }

            [Fact]
            public async Task ICurvePointService_UpdateCurvePointWithCurvePointDto_Modify_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "1",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                Task createCurvePoint = iCurvePointService.CreateCurvePointWithCurvePointDto(curvePointDto);
                int curvePointIdFound = iCurvePointService.GetAllCurvePointsDto().Result.Select(c => c.Id).Last();

                CurvePointDto modifiedCurvePointDto = new CurvePointDto
                {
                    Id = curvePointIdFound,
                    CurveId = "1",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "3",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                ///Act
                await iCurvePointService.UpdateCurvePointWithCurvePointDto(modifiedCurvePointDto);
                CurvePointDto modifiedFoundCurvePointDto = iCurvePointService.GetCurvePointDtoById(curvePointIdFound).Result.FirstOrDefault();
                curvePointDto.Id = curvePointIdFound;

                ///Assert
                Xunit.Assert.Equivalent(modifiedCurvePointDto, modifiedFoundCurvePointDto);

                await iCurvePointService.DeleteCurvePointById(curvePointIdFound);

            }

            [Fact]
            public async Task ICurvePointService_DeleteCurvePointById_ShouldDelete_1CurvePoint()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "1",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };

                Task createCurvePoint = iCurvePointService.CreateCurvePointWithCurvePointDto(curvePointDto);
                //int id = await curvePointrepository.GetMaxCurvePointId();
                int productIdFound = iCurvePointService.GetAllCurvePointsDto().Result.Select(c => c.Id).Last();

                Xunit.Assert.True(createCurvePoint.IsCompletedSuccessfully);
                ///Act
                Task deleteCurvePoint = iCurvePointService.DeleteCurvePointById(productIdFound);
                ///Assert
                Xunit.Assert.True(iCurvePointService.GetCurvePointDtoById(productIdFound).Result.FirstOrDefault() == null);
                Xunit.Assert.True(deleteCurvePoint.IsCompletedSuccessfully);

            }

            [Fact]
            public async Task ICurvePointService_GetCurvePointDtoById_ShouldGet_1CurvePointById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);

                CurvePointDto curvePointDto = new CurvePointDto
                {
                    CurveId = "1",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                Task createCurvePoint = iCurvePointService.CreateCurvePointWithCurvePointDto(curvePointDto);
                //int id = await curvePointrepository.GetMaxCurvePointId();
                int curvePointIdFound = iCurvePointService.GetAllCurvePointsDto().Result.Select(c => c.Id).Last();
                CurvePointDto FoundCurvePointDto = iCurvePointService.GetCurvePointDtoById(curvePointIdFound).Result.FirstOrDefault();

                ///Act
                curvePointDto.Id = curvePointIdFound;
                Task getCurvePointById = curvePointrepository.GetCurvePointById(curvePointIdFound);

                ///Assert
                Xunit.Assert.Equivalent(curvePointDto, FoundCurvePointDto);
                Xunit.Assert.True(createCurvePoint.IsCompletedSuccessfully);
                Xunit.Assert.True(getCurvePointById.IsCompletedSuccessfully);

                await iCurvePointService.DeleteCurvePointById(curvePointIdFound);
            }

            [Fact]
            public async Task ICurvePointService_GetAllCurvePoints_ShouldGet_2CurvePoints()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var curvePointrepository = new CurvePointRepository(context);
                ICurvePointService iCurvePointService = new CurvePointService(curvePointrepository, mapper);

                CurvePointDto curvePointDto1 = new CurvePointDto
                {
                    CurveId = "1",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };


                CurvePointDto curvePointDto2 = new CurvePointDto
                {
                    Id = 0,
                    CurveId = "2",
                    AsOfDate = "11/08/2026 12:53:27",
                    Term = "0,0001",
                    CurvePointValue = "0,0001",
                    CreationDate = "11/08/2026 12:53:27",
                };



                Task createCurvePoint1 = iCurvePointService.CreateCurvePointWithCurvePointDto(curvePointDto1);
                Task createCurvePoint2 = iCurvePointService.CreateCurvePointWithCurvePointDto(curvePointDto2);
                //int id = await curvePointrepository.GetMaxCurvePointId();
                List<int> curvePointFoundids = iCurvePointService.GetAllCurvePointsDto().Result.Select(c => c.Id).ToList();


                ///Act

                var getAllCurvePoints = iCurvePointService.GetAllCurvePointsDto().Result.Where(c => c.Id > 0);

                ///Assert
                Xunit.Assert.True(createCurvePoint1.IsCompletedSuccessfully);
                Xunit.Assert.True(createCurvePoint2.IsCompletedSuccessfully);
                Xunit.Assert.True(getAllCurvePoints.Count() == 2);
                Xunit.Assert.True(getAllCurvePoints.ElementAt(0).Id == curvePointFoundids[0]);
                Xunit.Assert.True(getAllCurvePoints.ElementAt(1).Id == curvePointFoundids[1]);

                await iCurvePointService.DeleteCurvePointById(curvePointFoundids[0]);
                await iCurvePointService.DeleteCurvePointById(curvePointFoundids[1]);


            }

            [Fact]
            public async Task AsyncActionFilter_WithWrongCurvePointDto_ShouldReturn_8Errors()
            {
                CurvePointDto wrongCurvePointDto = new CurvePointDto
                {
                    CurveId = "2000",
                    AsOfDate = "ruioolk",
                    Term = "abc",
                    CurvePointValue = "abcd",
                    CreationDate = "kiuyjk",
                };


                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(wrongCurvePointDto);
                Validator.TryValidateObject(wrongCurvePointDto, validationContext, validationResults, validateAllProperties: true);


                var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
                foreach (var vr in validationResults)
                {
                    var key = vr.MemberNames.FirstOrDefault() ?? string.Empty;
                    modelState.AddModelError(key, vr.ErrorMessage);
                }

                var httpContext = new DefaultHttpContext();
                var routeData = new Microsoft.AspNetCore.Routing.RouteData();
                routeData.Values["controller"] = "MyController";
                routeData.Values["action"] = "Create";
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor(), modelState);
                var actionExecutingContext = new ActionExecutingContext(
                    actionContext,
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object>(),
                    controller: null
                );


                ActionExecutionDelegate next = () => Task.FromResult<ActionExecutedContext>(
                    new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller: null)
                );
                var filter = new AsyncActionFilter();
                await filter.OnActionExecutionAsync(actionExecutingContext, next);
                var error0 = modelState.ElementAt(0);
                var error1 = modelState.ElementAt(1);
                var error2 = modelState.ElementAt(2);
                var error3 = modelState.ElementAt(3);
                var error4 = modelState.ElementAt(4);
                int test = modelState.ErrorCount;
                //Assert
                Xunit.Assert.Equal("The Term is not greater than 0 and smaller than 1.79 E+308.", error0.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The Term is not a double.", error0.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The CurveId is not a byte as its value not equal or greater than 0 and smaller or equal to 256.", error1.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The CurveId is not a byte.", error1.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The AsOfDate does not suit the DateTime format.", error2.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The CreationDate does not suit the DateTime format.", error3.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The CurvePointValue is not greater than 0 and smaller than 1.79 E+308.", error4.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The CurvePointValue is not a double.", error4.Value.Errors[1].ErrorMessage);
                Xunit.Assert.True(modelState.ErrorCount == 8);
            }
        }

        public class RatingUnitTests
        {
            private P7Referential GetInMemoryDbContext()
            {
                var options = new DbContextOptionsBuilder<P7Referential>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase3")
                    .Options;
                return new P7Referential(options);
            }



            [Fact]
            public async Task IRatingService_CreateRatingWithRatingDto_ShouldAdd_1Rating()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };

                ///Act
                //bool result = customValidationAttribute.IsValid(ratingDto);
                Task createRating = iRatingService.CreateRatingWithRatingDto(ratingDto);
                int ratingIdFound = iRatingService.GetAllRatingsDto().Result.Select(r => r.Id).Last();
                RatingDto foundRatingDto = iRatingService.GetRatingDtoById(ratingIdFound).Result.FirstOrDefault();
                ratingDto.Id = ratingIdFound;

                ///Assert
                //Xunit.Assert.True(result);
                Xunit.Assert.True(createRating.IsCompletedSuccessfully);
                Xunit.Assert.Equivalent(ratingDto, foundRatingDto);

                await iRatingService.DeleteRatingById(ratingIdFound);

            }

            [Fact]
            public async Task IBidListService_CreateBidListWithWrongRatingDto_ShouldNotAdd_1Rating()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var RatingRepository = new RatingRepository(context);
                IRatingService ratingService = new RatingService(RatingRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                RatingDto wrongRatingDto = new RatingDto
                {
                    MoodysRating = "",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "6000",
                };


                ///Act
                Task createRating = ratingService.CreateRatingWithRatingDto(wrongRatingDto);
                bool foundRatingDto = (ratingService.GetAllRatingsDto().Result.Select(c => c).Count() > 0);

                ///Assert
                Xunit.Assert.False(createRating.IsCompletedSuccessfully);
                Xunit.Assert.True(createRating.IsFaulted);
                Xunit.Assert.False(foundRatingDto);
            }


            [Fact]
            public async Task IRatingService_UpdateRatingWithRatingDto_Should_Modify_1Rating()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                Task createRating = iRatingService.CreateRatingWithRatingDto(ratingDto);
                int ratingIdFound = iRatingService.GetAllRatingsDto().Result.Select(r => r.Id).Last();

                RatingDto modifiedRatingDto = new RatingDto
                {
                    Id = ratingIdFound,
                    MoodysRating = "BBB",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                ///Act
                await iRatingService.UpdateRatingWithRatingDto(modifiedRatingDto);
                RatingDto modifiedFoundRatingDto = iRatingService.GetRatingDtoById(ratingIdFound).Result.FirstOrDefault();
                ratingDto.Id = ratingIdFound;

                ///Assert
                Xunit.Assert.Equivalent(modifiedRatingDto, modifiedFoundRatingDto);

                await iRatingService.DeleteRatingById(ratingIdFound);

            }

            [Fact]
            public async Task IRatingService_DeleteRatingById_ShouldDelete_1Rating()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };

                Task createRating = iRatingService.CreateRatingWithRatingDto(ratingDto);
                //int id = await ratingrepository.GetMaxRatingId();
                int productIdFound = iRatingService.GetAllRatingsDto().Result.Select(r => r.Id).Last();

                Xunit.Assert.True(createRating.IsCompletedSuccessfully);
                ///Act
                Task deleteRating = iRatingService.DeleteRatingById(productIdFound);
                ///Assert
                Xunit.Assert.True(iRatingService.GetRatingDtoById(productIdFound).Result.FirstOrDefault() == null);
                Xunit.Assert.True(deleteRating.IsCompletedSuccessfully);

            }

            [Fact]
            public async Task IRatingService_GetRatingDtoById_ShouldGet_1RatingById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);

                RatingDto ratingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                Task createRating = iRatingService.CreateRatingWithRatingDto(ratingDto);
                //int id = await ratingrepository.GetMaxRatingId();
                int ratingIdFound = iRatingService.GetAllRatingsDto().Result.Select(r => r.Id).Last();
                RatingDto FoundRatingDto = iRatingService.GetRatingDtoById(ratingIdFound).Result.FirstOrDefault();

                ///Act
                ratingDto.Id = ratingIdFound;
                Task getRatingById = ratingrepository.GetRatingById(ratingIdFound);

                ///Assert
                Xunit.Assert.Equivalent(ratingDto, FoundRatingDto);
                Xunit.Assert.True(createRating.IsCompletedSuccessfully);
                Xunit.Assert.True(getRatingById.IsCompletedSuccessfully);

                await iRatingService.DeleteRatingById(ratingIdFound);
            }

            [Fact]
            public async Task IRatingService_GetAllRatings_ShouldGet_2Ratings()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ratingrepository = new RatingRepository(context);
                IRatingService iRatingService = new RatingService(ratingrepository, mapper);

                RatingDto ratingDto1 = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };


                RatingDto ratingDto2 = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "10",
                };



                Task createRating1 = iRatingService.CreateRatingWithRatingDto(ratingDto1);
                Task createRating2 = iRatingService.CreateRatingWithRatingDto(ratingDto2);
                //int id = await ratingrepository.GetMaxRatingId();
                List<int> ratingFoundids = iRatingService.GetAllRatingsDto().Result.Select(r => r.Id).ToList();


                ///Act

                var getAllRatings = iRatingService.GetAllRatingsDto().Result.Where(r => r.Id > 0);

                ///Assert
                Xunit.Assert.True(createRating1.IsCompletedSuccessfully);
                Xunit.Assert.True(createRating2.IsCompletedSuccessfully);
                Xunit.Assert.True(getAllRatings.Count() == 2);
                Xunit.Assert.True(getAllRatings.ElementAt(0).Id == ratingFoundids[0]);
                Xunit.Assert.True(getAllRatings.ElementAt(1).Id == ratingFoundids[1]);

                await iRatingService.DeleteRatingById(ratingFoundids[0]);
                await iRatingService.DeleteRatingById(ratingFoundids[1]);


            }

            [Fact]
            public async Task AsyncActionFilter_WithWrongRatingDto_ShouldReturn_8Errors()
            {
                RatingDto wrongRatingDto = new RatingDto
                {
                    MoodysRating = "AAA",
                    FitchRating = "AAA",
                    SandPRating = "AAA",
                    OrderNumber = "jfg",
                };


                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(wrongRatingDto);
                Validator.TryValidateObject(wrongRatingDto, validationContext, validationResults, validateAllProperties: true);


                var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
                foreach (var vr in validationResults)
                {
                    var key = vr.MemberNames.FirstOrDefault() ?? string.Empty;
                    modelState.AddModelError(key, vr.ErrorMessage);
                }

                var httpContext = new DefaultHttpContext();
                var routeData = new Microsoft.AspNetCore.Routing.RouteData();
                routeData.Values["controller"] = "MyController";
                routeData.Values["action"] = "Create";
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor(), modelState);
                var actionExecutingContext = new ActionExecutingContext(
                    actionContext,
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object>(),
                    controller: null
                );


                ActionExecutionDelegate next = () => Task.FromResult<ActionExecutedContext>(
                    new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller: null)
                );
                var filter = new AsyncActionFilter();
                await filter.OnActionExecutionAsync(actionExecutingContext, next);
                var error0 = modelState.ElementAt(0);
                int test = modelState.ErrorCount;
                //Assert
                Xunit.Assert.Equal("The OrderNumber is not a byte as its value not equal or greater than 0 and smaller or equal to 256.", error0.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The OrderNumber is not a byte.", error0.Value.Errors[1].ErrorMessage);
                Xunit.Assert.True(modelState.ErrorCount == 2);
            }
        }

        public class RuleNameUnitTests
        {
            private P7Referential GetInMemoryDbContext()
            {
                var options = new DbContextOptionsBuilder<P7Referential>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase4")
                    .Options;
                return new P7Referential(options);
            }



            [Fact]
            public async Task IRuleNameService_CreateRuleNameWithRuleNameDto_ShouldAdd_1RuleName()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ruleNamerepository = new RuleNameRepository(context);
                IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);
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
                //bool result = customValidationAttribute.IsValid(ruleNameDto);
                Task createRuleName = iRuleNameService.CreateRuleNameWithRuleNameDto(ruleNameDto);
                int ruleNameIdFound = iRuleNameService.GetAllRuleNamesDto().Result.Select(r => r.Id).Last();
                RuleNameDto foundRuleNameDto = iRuleNameService.GetRuleNameDtoById(ruleNameIdFound).Result.FirstOrDefault();
                ruleNameDto.Id = ruleNameIdFound;

                ///Assert
                //Xunit.Assert.True(result);
                Xunit.Assert.True(createRuleName.IsCompletedSuccessfully);
                Xunit.Assert.Equivalent(ruleNameDto, foundRuleNameDto);

                await iRuleNameService.DeleteRuleNameById(ruleNameIdFound);

            }

            [Fact]
            public async Task IRuleNameService_UpdateRuleNameWithRuleNameDto_Modify_1RuleName()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ruleNamerepository = new RuleNameRepository(context);
                IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);

                RuleNameDto ruleNameDto = new RuleNameDto
                {
                    Name = "test",
                    Description = "testdescription",
                    Json = "Json",
                    Template = "Template",
                    SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                };


                Task createRuleName = iRuleNameService.CreateRuleNameWithRuleNameDto(ruleNameDto);
                int ruleNameIdFound = iRuleNameService.GetAllRuleNamesDto().Result.Select(r => r.Id).Last();

                RuleNameDto modifiedRuleNameDto = new RuleNameDto
                {
                    Id = ruleNameIdFound,
                    Name = "test2mod",
                    Description = "testdescription",
                    Json = "Json",
                    Template = "Template",
                    SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                };


                ///Act
                await iRuleNameService.UpdateRuleNameWithRuleNameDto(modifiedRuleNameDto);
                RuleNameDto modifiedFoundRuleNameDto = iRuleNameService.GetRuleNameDtoById(ruleNameIdFound).Result.FirstOrDefault();
                ruleNameDto.Id = ruleNameIdFound;

                ///Assert
                Xunit.Assert.Equivalent(modifiedRuleNameDto, modifiedFoundRuleNameDto);

                await iRuleNameService.DeleteRuleNameById(ruleNameIdFound);

            }

            [Fact]
            public async Task IRuleNameService_DeleteRuleNameById_ShouldDelete_1RuleName()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ruleNamerepository = new RuleNameRepository(context);
                IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);

                RuleNameDto ruleNameDto = new RuleNameDto
                {
                    Name = "test",
                    Description = "testdescription",
                    Json = "Json",
                    Template = "Template",
                    SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                };

                Task createRuleName = iRuleNameService.CreateRuleNameWithRuleNameDto(ruleNameDto);
                //int id = await ruleNamerepository.GetMaxRuleNameId();
                int productIdFound = iRuleNameService.GetAllRuleNamesDto().Result.Select(r => r.Id).Last();

                Xunit.Assert.True(createRuleName.IsCompletedSuccessfully);
                ///Act
                Task deleteRuleName = iRuleNameService.DeleteRuleNameById(productIdFound);
                ///Assert
                Xunit.Assert.True(iRuleNameService.GetRuleNameDtoById(productIdFound).Result.FirstOrDefault() == null);
                Xunit.Assert.True(deleteRuleName.IsCompletedSuccessfully);

            }

            [Fact]
            public async Task IRuleNameService_GetRuleNameDtoById_ShouldGet_1RuleNameById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ruleNamerepository = new RuleNameRepository(context);
                IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);

                RuleNameDto ruleNameDto = new RuleNameDto
                {
                    Name = "test",
                    Description = "testdescription",
                    Json = "Json",
                    Template = "Template",
                    SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                    SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                };


                Task createRuleName = iRuleNameService.CreateRuleNameWithRuleNameDto(ruleNameDto);
                //int id = await ruleNamerepository.GetMaxRuleNameId();
                int ruleNameIdFound = iRuleNameService.GetAllRuleNamesDto().Result.Select(r => r.Id).Last();
                RuleNameDto FoundRuleNameDto = iRuleNameService.GetRuleNameDtoById(ruleNameIdFound).Result.FirstOrDefault();

                ///Act
                ruleNameDto.Id = ruleNameIdFound;
                Task getRuleNameById = ruleNamerepository.GetRuleNameById(ruleNameIdFound);

                ///Assert
                Xunit.Assert.Equivalent(ruleNameDto, FoundRuleNameDto);
                Xunit.Assert.True(createRuleName.IsCompletedSuccessfully);
                Xunit.Assert.True(getRuleNameById.IsCompletedSuccessfully);

                await iRuleNameService.DeleteRuleNameById(ruleNameIdFound);
            }

            [Fact]
            public async Task IRuleNameService_GetAllRuleNames_ShouldGet_2RuleNames()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var ruleNamerepository = new RuleNameRepository(context);
                IRuleNameService iRuleNameService = new RuleNameService(ruleNamerepository, mapper);

                RuleNameDto ruleNameDto1 = new RuleNameDto
                {
                    Name = "test1",
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



                Task createRuleName1 = iRuleNameService.CreateRuleNameWithRuleNameDto(ruleNameDto1);
                Task createRuleName2 = iRuleNameService.CreateRuleNameWithRuleNameDto(ruleNameDto2);
                //int id = await ruleNamerepository.GetMaxRuleNameId();
                List<int> ruleNameFoundids = iRuleNameService.GetAllRuleNamesDto().Result.Select(r => r.Id).ToList();


                ///Act

                var getAllRuleNames = iRuleNameService.GetAllRuleNamesDto().Result.Where(r => r.Id > 0);

                ///Assert
                Xunit.Assert.True(createRuleName1.IsCompletedSuccessfully);
                Xunit.Assert.True(createRuleName2.IsCompletedSuccessfully);
                Xunit.Assert.True(getAllRuleNames.Count() == 2);
                Xunit.Assert.True(getAllRuleNames.ElementAt(0).Id == ruleNameFoundids[0]);
                Xunit.Assert.True(getAllRuleNames.ElementAt(1).Id == ruleNameFoundids[1]);

                await iRuleNameService.DeleteRuleNameById(ruleNameFoundids[0]);
                await iRuleNameService.DeleteRuleNameById(ruleNameFoundids[1]);

            }


            [Fact]
            public async Task AsyncActionFilter_WithWrongRuleNameDto_ShouldReturn_0Error()
            {
                RuleNameDto wrongRuleNameDto = new RuleNameDto
                {
                    Name = "hgfekjg+6",
                    Description = "jfgk48/",
                    Json = "tgqdjk625",
                    Template = "12589",
                    SqlStr = "12359",
                    SqlPart = "2589",
                };


                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(wrongRuleNameDto);
                Validator.TryValidateObject(wrongRuleNameDto, validationContext, validationResults, validateAllProperties: true);


                var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
                foreach (var vr in validationResults)
                {
                    var key = vr.MemberNames.FirstOrDefault() ?? string.Empty;
                    modelState.AddModelError(key, vr.ErrorMessage);
                }

                var httpContext = new DefaultHttpContext();
                var routeData = new Microsoft.AspNetCore.Routing.RouteData();
                routeData.Values["controller"] = "MyController";
                routeData.Values["action"] = "Create";
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor(), modelState);
                var actionExecutingContext = new ActionExecutingContext(
                    actionContext,
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object>(),
                    controller: null
                );


                ActionExecutionDelegate next = () => Task.FromResult<ActionExecutedContext>(
                    new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller: null)
                );
                var filter = new AsyncActionFilter();
                await filter.OnActionExecutionAsync(actionExecutingContext, next);
                int test = modelState.ErrorCount;
                //Assert
                Xunit.Assert.True(modelState.ErrorCount == 0);
            }


        }


        public class TradeUnitTests
        {
            private P7Referential GetInMemoryDbContext()
            {
                var options = new DbContextOptionsBuilder<P7Referential>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase5")
                    .Options;
                return new P7Referential(options);
            }



            [Fact]
            public async Task ITradeService_CreateTradeWithTradeDto_ShouldAdd_1Trade()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var traderepository = new TradeRepository(context);
                ITradeService iTradeService = new TradeService(traderepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                TradeDto tradeDto = new TradeDto
                {
                    Account = "string",
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
                //bool result = customValidationAttribute.IsValid(tradeDto);
                Task createTrade = iTradeService.CreateTradeWithTradeDto(tradeDto);
                int tradeIdFound = iTradeService.GetAllTradesDto().Result.Select(t => t.TradeId).Last();
                TradeDto foundTradeDto = iTradeService.GetTradeDtoById(tradeIdFound).Result.FirstOrDefault();
                tradeDto.TradeId = tradeIdFound;

                ///Assert
                //Xunit.Assert.True(result);
                Xunit.Assert.True(createTrade.IsCompletedSuccessfully);
                Xunit.Assert.Equivalent(tradeDto, foundTradeDto);

                await iTradeService.DeleteTradeById(tradeIdFound);

            }

            [Fact]
            public async Task IBidListService_CreateBidListWithWrongTradeDto_ShouldNotAdd_1Trade()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var TradeRepository = new TradeRepository(context);
                ITradeService tradeService = new TradeService(TradeRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                TradeDto wrongTradeDto = new TradeDto
                {
                    Account = "string",
                    AccountType = "string",
                    BuyQuantity = "1",
                    SellQuantity = "1",
                    BuyPrice = "1",
                    SellPrice = "1",
                    TradeDate = "2",
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
                Task createTrade = tradeService.CreateTradeWithTradeDto(wrongTradeDto);
                bool foundTradeDto = (tradeService.GetAllTradesDto().Result.Select(c => c).Count() > 0);

                ///Assert
                Xunit.Assert.False(createTrade.IsCompletedSuccessfully);
                Xunit.Assert.True(createTrade.IsFaulted);
                Xunit.Assert.False(foundTradeDto);
            }



            [Fact]
            public async Task ITradeService_UpdateTradeWithTradeDto_Modify_1Trade()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var traderepository = new TradeRepository(context);
                ITradeService iTradeService = new TradeService(traderepository, mapper);

                TradeDto tradeDto = new TradeDto
                {
                    Account = "string",
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


                Task createTrade = iTradeService.CreateTradeWithTradeDto(tradeDto);
                int tradeIdFound = iTradeService.GetAllTradesDto().Result.Select(t => t.TradeId).Last();

                TradeDto modifiedTradeDto = new TradeDto
                {
                    TradeId = tradeIdFound,
                    Account = "stringmodified",
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
                await iTradeService.UpdateTradeWithTradeDto(modifiedTradeDto);
                TradeDto modifiedFoundTradeDto = iTradeService.GetTradeDtoById(tradeIdFound).Result.FirstOrDefault();
                tradeDto.TradeId = tradeIdFound;

                ///Assert
                Xunit.Assert.Equivalent(modifiedTradeDto, modifiedFoundTradeDto);

                await iTradeService.DeleteTradeById(tradeIdFound);

            }

            [Fact]
            public async Task ITradeService_DeleteTradeById_ShouldDelete_1Trade()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var traderepository = new TradeRepository(context);
                ITradeService iTradeService = new TradeService(traderepository, mapper);

                TradeDto tradeDto = new TradeDto
                {
                    Account = "string",
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

                Task createTrade = iTradeService.CreateTradeWithTradeDto(tradeDto);
                //int id = await traderepository.GetMaxTradeId();
                int productIdFound = iTradeService.GetAllTradesDto().Result.Select(t => t.TradeId).Last();

                Xunit.Assert.True(createTrade.IsCompletedSuccessfully);
                ///Act
                Task deleteTrade = iTradeService.DeleteTradeById(productIdFound);
                ///Assert
                Xunit.Assert.True(iTradeService.GetTradeDtoById(productIdFound).Result.FirstOrDefault() == null);
                Xunit.Assert.True(deleteTrade.IsCompletedSuccessfully);

            }

            [Fact]
            public async Task ITradeService_GetTradeDtoById_ShouldGet_1TradeById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var traderepository = new TradeRepository(context);
                ITradeService iTradeService = new TradeService(traderepository, mapper);

                TradeDto tradeDto = new TradeDto
                {
                    Account = "string",
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


                Task createTrade = iTradeService.CreateTradeWithTradeDto(tradeDto);
                //int id = await traderepository.GetMaxTradeId();
                int tradeIdFound = iTradeService.GetAllTradesDto().Result.Select(t => t.TradeId).Last();
                TradeDto FoundTradeDto = iTradeService.GetTradeDtoById(tradeIdFound).Result.FirstOrDefault();

                ///Act
                tradeDto.TradeId = tradeIdFound;
                Task getTradeById = traderepository.GetTradeById(tradeIdFound);

                ///Assert
                Xunit.Assert.Equivalent(tradeDto, FoundTradeDto);
                Xunit.Assert.True(createTrade.IsCompletedSuccessfully);
                Xunit.Assert.True(getTradeById.IsCompletedSuccessfully);

                await iTradeService.DeleteTradeById(tradeIdFound);
            }

            [Fact]
            public async Task ITradeService_GetAllTrades_ShouldGet_2Trades()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var traderepository = new TradeRepository(context);
                ITradeService iTradeService = new TradeService(traderepository, mapper);

                TradeDto tradeDto1 = new TradeDto
                {
                    Account = "string",
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



                Task createTrade1 = iTradeService.CreateTradeWithTradeDto(tradeDto1);
                Task createTrade2 = iTradeService.CreateTradeWithTradeDto(tradeDto2);
                //int id = await traderepository.GetMaxTradeId();
                List<int> tradeFoundids = iTradeService.GetAllTradesDto().Result.Select(t => t.TradeId).ToList();


                ///Act

                var getAllTrades = iTradeService.GetAllTradesDto().Result.Where(t => t.TradeId > 0);

                ///Assert
                Xunit.Assert.True(createTrade1.IsCompletedSuccessfully);
                Xunit.Assert.True(createTrade2.IsCompletedSuccessfully);
                Xunit.Assert.True(getAllTrades.Count() == 2);
                Xunit.Assert.True(getAllTrades.ElementAt(0).TradeId == tradeFoundids[0]);
                Xunit.Assert.True(getAllTrades.ElementAt(1).TradeId == tradeFoundids[1]);

                await iTradeService.DeleteTradeById(tradeFoundids[0]);
                await iTradeService.DeleteTradeById(tradeFoundids[1]);


            }

            [Fact]
            public async Task AsyncActionFilter_WithWrongTradeDto_ShouldReturn_11Errors()
            {
                TradeDto wrongTradeDto = new TradeDto
                {
                    Account = "string2test",
                    AccountType = "string",
                    BuyQuantity = "abc",
                    SellQuantity = "abcd",
                    BuyPrice = "abcde",
                    SellPrice = "abcdef",
                    TradeDate = "jh",
                    TradeSecurity = "string",
                    TradeStatus = "string",
                    Trader = "string",
                    Benchmark = "string",
                    Book = "string",
                    CreationName = "string",
                    CreationDate = "ytj",
                    RevisionName = "string",
                    RevisionDate = "juytj",
                    DealName = "string"
                };


                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(wrongTradeDto);
                Validator.TryValidateObject(wrongTradeDto, validationContext, validationResults, validateAllProperties: true);


                var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
                foreach (var vr in validationResults)
                {
                    var key = vr.MemberNames.FirstOrDefault() ?? string.Empty;
                    modelState.AddModelError(key, vr.ErrorMessage);
                }

                var httpContext = new DefaultHttpContext();
                var routeData = new Microsoft.AspNetCore.Routing.RouteData();
                routeData.Values["controller"] = "MyController";
                routeData.Values["action"] = "Create";
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor(), modelState);
                var actionExecutingContext = new ActionExecutingContext(
                    actionContext,
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object>(),
                    controller: null
                );


                ActionExecutionDelegate next = () => Task.FromResult<ActionExecutedContext>(
                    new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller: null)
                );
                var filter = new AsyncActionFilter();
                await filter.OnActionExecutionAsync(actionExecutingContext, next);
                var error0 = modelState.ElementAt(0);
                var error1 = modelState.ElementAt(1);
                var error2 = modelState.ElementAt(2);
                var error3 = modelState.ElementAt(3);
                var error4 = modelState.ElementAt(4);
                var error5 = modelState.ElementAt(5);
                var error6 = modelState.ElementAt(6);

                // Assert
                Xunit.Assert.Equal("The BuyPrice is not greater than 0 and smaller than 1.79 E+308.", error0.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The BuyPrice is not a double.", error0.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The SellPrice is not greater than 0 and smaller than 1.79 E+308.", error1.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The SellPrice is not a double.", error1.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The TradeDate does not suit the DateTime format.", error2.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The BuyQuantity is not greater than 0 and smaller than 1.79 E+308.", error3.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The BuyQuantity is not a double.", error3.Value.Errors[1].ErrorMessage);
                Xunit.Assert.Equal("The CreationDate does not suit the DateTime format.", error4.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The RevisionDate does not suit the DateTime format.", error5.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The SellQuantity is not greater than 0 and smaller than 1.79 E+308.", error6.Value.Errors[0].ErrorMessage);
                Xunit.Assert.Equal("The SellQuantity is not a double.", error6.Value.Errors[1].ErrorMessage);
                Xunit.Assert.True(modelState.ErrorCount == 11);
            }

        }


        public class UserUnitTests
        {
            private ApplicationDbContext GetInMemoryDbContext()
            {
                var configuration = new ConfigurationBuilder();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                return new ApplicationDbContext(options, configuration);
            }


            [Fact]
            public async Task IUserService_CreateUserWithRegisterModel_ShouldAdd_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel",
                    Email = "registerModel@gmail.com",
                    Password = "test123.Pass",
                    Role = "",
                };

                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                UserDto userDtoFound = iUserService.GetUserDtoByEmail(registerModel.Email).Result.Select(u => u).Last();

                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.Equivalent(registerModel.UserName, userDtoFound.UserName);
                Xunit.Assert.Equivalent("Member", userDtoFound.Role);
                await iUserService.DeleteUserByEmail(registerModel.Email);
            }



            [Fact]
            public async Task IUserService_DeleteUserByEmail_ShouldDelete_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel",
                    Email = "registerModel@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };


                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                UserDto userDtoFound = iUserService.GetUserDtoByEmail(registerModel.Email).Result.Select(u => u).Last();

                ///Act
                var deleteUser = await iUserService.DeleteUserByEmail(registerModel.Email);
                int userDtoFoundafterDeletetionCount = iUserService.GetUserDtoByEmail(registerModel.Email).Result.Select(u => u).Count();

                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.Equivalent(registerModel.UserName, userDtoFound.UserName);
                Xunit.Assert.Equivalent(registerModel.Role, userDtoFound.Role);
                Xunit.Assert.True(deleteUser.Succeeded);
                Xunit.Assert.Equal(0, userDtoFoundafterDeletetionCount);

            }


            [Fact]
            public async Task IUserService_GetAllUsersDto_ShouldGet_2Users()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                //var userValidator = new List<IUserValidator<User>>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new IPasswordValidator<User>[0];
                var validator = new PasswordValidator<User>();
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel1",
                    Email = "registerModel1@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                P7CreateRestApi.Models.RegisterModel registerModel2 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel2",
                    Email = "registerModel2@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                ///Act
                var createUser1 = await iUserService.CreateUserWithRegisterModel(registerModel1);
                var createUser2 = await iUserService.CreateUserWithRegisterModel(registerModel2);
                IList<UserDto> userDtoFound = iUserService.GetAllUsersDto().Result.Select(u => u).ToList();
                int userDtoFoundafterDeletetionCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();

                ///Assert
                Xunit.Assert.True(createUser1.Succeeded);
                Xunit.Assert.True(createUser2.Succeeded);
                Xunit.Assert.Equivalent(registerModel1.UserName, userDtoFound[0].UserName);
                Xunit.Assert.Equivalent(registerModel1.Role, userDtoFound[0].Role);
                Xunit.Assert.Equivalent(registerModel2.UserName, userDtoFound[1].UserName);
                Xunit.Assert.Equivalent(registerModel2.Role, userDtoFound[1].Role);
                Xunit.Assert.Equal(2, userDtoFoundafterDeletetionCount);
                await iUserService.DeleteUserByEmail(registerModel1.Email);
                await iUserService.DeleteUserByEmail(registerModel2.Email);
            }


            [Fact]
            public async Task IBidListService_CreateBidListWithBidListDto_ShouldGet_1UserById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel1",
                    Email = "test123456789abcd@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                P7CreateRestApi.Models.RegisterModel registerModel2 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel2",
                    Email = "test1abcd@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                ///Act
                var createUser1 = await iUserService.CreateUserWithRegisterModel(registerModel1);
                var createUser2 = await iUserService.CreateUserWithRegisterModel(registerModel2);
                UserDto userDtoFound1 = iUserService.GetUserDtoByEmail(registerModel1.Email).Result.Select(u => u).First();




                ///Assert
                Xunit.Assert.True(createUser1.Succeeded);
                Xunit.Assert.True(createUser2.Succeeded);
                Xunit.Assert.Equivalent(registerModel1.UserName, userDtoFound1.UserName);
                Xunit.Assert.Equivalent(registerModel1.Role, userDtoFound1.Role);
                await iUserService.DeleteUserByEmail(registerModel1.Email);
                await iUserService.DeleteUserByEmail(registerModel2.Email);
            }

            [Fact]
            public async Task IBidListService_UpdateUserWithUpdateGeneralInfosModel_ShouldUpdate_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "register",
                    Password = "passW1.ordtest",
                    Role = "Member",
                };

                P7CreateRestApi.Models.UpdateGeneralInfosModel updateGeneralInfosModelForUpdate = new P7CreateRestApi.Models.UpdateGeneralInfosModel
                {
                    UserName = "testnewname",
                    Role = "Admin",
                };

                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                var updateUser = await iUserService.UpdateUserWithUpdateGeneralInfosModel(registerModel.Email, updateGeneralInfosModelForUpdate);
                // Persistency = staus before/during/after the action. _userManager.UpdateAsync does not tell the new database instance about the updates in test environement.
                // We must use context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email); to retrieve the updated user
                var userDtoFoundwithoutPertisency = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email);


                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.True(updateUser.Succeeded);
                Xunit.Assert.Equivalent(updateGeneralInfosModelForUpdate.UserName, userDtoFoundwithoutPertisency.UserName);
                Xunit.Assert.Equivalent(updateGeneralInfosModelForUpdate.Role, userDtoFoundwithoutPertisency.Role);
                var test1 = await userManager.GetUsersInRoleAsync("Member");
                var test2 = await userManager.GetUsersInRoleAsync("Admin");
                Xunit.Assert.True(test2.ElementAt(0).UserName == userDtoFoundwithoutPertisency.UserName);

                await iUserService.DeleteUserByEmail(registerModel.Email);

            }


            [Fact]
            public async Task IBidListService_UpdateUserWithUpdatePasswordModel_ShouldUpdate_1PasswordUser()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "register",
                    Password = "passW1.ordtest",
                    Role = "Member",
                };

                P7CreateRestApi.Models.UpdatePasswordModel updatePasswordModelForUpdate = new P7CreateRestApi.Models.UpdatePasswordModel
                {
                    CurrentPassword = "passW1.ordtest",
                    NewPassword = "NewPassword123.",
                };

                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                var updateUser = await iUserService.UpdateUserPasswordWithUpdatePasswordModel(registerModel.Email, updatePasswordModelForUpdate);
                // Persistency = staus before/during/after the action. _userManager.UpdateAsync does not tell the new database instance about the updates in test environement.
                // We must use context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email); to retrieve the updated user
                var userDtoFoundwithoutPertisency = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email);


                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.True(updateUser.Succeeded);
                Xunit.Assert.Equivalent(updatePasswordModelForUpdate.NewPassword, userDtoFoundwithoutPertisency.Password);

                await iUserService.DeleteUserByEmail(registerModel.Email);

            }


            [Fact]
            public async Task IBidListService_CreateUserWithRegisterModel_ShouldNotCreate_2IdenticalUsers()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "test@gmail.com",
                    Password = "passwordTest1.",
                    Role = "Member",
                };

                P7CreateRestApi.Models.RegisterModel registerModel2 = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "test@gmail.com",
                    Password = "passwordTest1.",
                    Role = "",
                };
                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
                var createUser2 = await iUserService.CreateUserWithRegisterModel(registerModel2);
                UserDto userDtoFound1 = iUserService.GetUserDtoByEmail(registerModel1.Email).Result.Select(u => u).First();
                int userDtoFoundCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();



                ///Assert
                Xunit.Assert.Equivalent(registerModel1.Email, userDtoFound1.Email);
                Xunit.Assert.Equivalent(registerModel1.UserName, userDtoFound1.UserName);
                Xunit.Assert.Equivalent(registerModel1.Role, userDtoFound1.Role);
                Xunit.Assert.Equal(1, userDtoFoundCount);
                Xunit.Assert.False(createUser2.Succeeded);
                await iUserService.DeleteUserByEmail(registerModel1.Email);

            }


            [Fact]
            public async Task IBidListService_CreateUserWithRegisterModel_ShouldNotCreate_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "test@gmail.com",
                    Password = "",
                    Role = "Member",
                };


                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
                //UserDto userDtoFound1 = iUserService.GetUserDtoByEmail(registerModel1.Email).Result.Select(u => u).First();
                int userDtoFoundCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();



                ///Assert
                Xunit.Assert.Equal(0, userDtoFoundCount);


            }


            [Fact]
            public async Task IBidListService_CreateBidListWithBidListDto_ShouldReturn_4Errors()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel1",
                    Email = "registerModel1@gmail.com",
                    Password = "pp",
                    Role = "Member",
                };



                ///Act
                var createUser1 = await iUserService.CreateUserWithRegisterModel(registerModel1);
                int userDtoFoundCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();
                string err1 = createUser1.Errors.ElementAt(0).Description;
                string err2 = createUser1.Errors.ElementAt(1).Description;
                string err3 = createUser1.Errors.ElementAt(2).Description;
                string err4 = createUser1.Errors.ElementAt(3).Description;


                ///Assert
                Xunit.Assert.Equal(0, userDtoFoundCount);
                Xunit.Assert.Equal(4, createUser1.Errors.Count());
                Xunit.Assert.Contains(err1, "Passwords must be at least 10 characters.");
                Xunit.Assert.Contains(err2, "Passwords must have at least one non alphanumeric character.");
                Xunit.Assert.Contains(err3, "Passwords must have at least one digit ('0'-'9').");
                Xunit.Assert.Contains(err4, "Passwords must have at least one uppercase ('A'-'Z').");
                await iUserService.DeleteUserByEmail(registerModel1.Email);
            }

        }
        //public class LoginUnitTests
        //{
        //    private ApplicationDbContext GetInMemoryDbContext()
        //    {
        //        var configuration = new ConfigurationBuilder();
        //        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //            .UseInMemoryDatabase(databaseName: "TestDatabase6")
        //            .Options;
        //        return new ApplicationDbContext(options, configuration);
        //    }

        //    [Fact]
        //    public async Task IBidLoginService_Login_ShouldLogin_1User()
        //    {

        //        var services = new ServiceCollection();
        //        services.AddLogging();
        //        services.AddDbContext<ApplicationDbContext>(opts =>
        //            opts.UseInMemoryDatabase("TestDb_" + Guid.NewGuid()));
        //        services.AddIdentity<User, IdentityRole>(options =>
        //        {
        //            options.Password.RequireDigit = true;
        //            options.Password.RequireLowercase = true;
        //            options.Password.RequireNonAlphanumeric = true;
        //            options.Password.RequireUppercase = true;
        //            options.Password.RequiredLength = 10;
        //        })
        //        .AddEntityFrameworkStores<ApplicationDbContext>()
        //        .AddDefaultTokenProviders();
        //        services.AddHttpContextAccessor();

        //        var configuration = new ConfigurationBuilder()
        //            .AddInMemoryCollection(new Dictionary<string, string>
        //            {
        //                { "ConnectionStrings:P7Identity", "Server=(localdb)\\mssqllocaldb;Database=P7Test;Trusted_Connection=True;" }
        //            })
        //            .Build();

        //        services.AddSingleton<IConfiguration>(configuration);


        //        var sp = services.BuildServiceProvider();

        //        using var scope = sp.CreateScope();
        //        var provider = scope.ServiceProvider;
        //        var db = provider.GetRequiredService<ApplicationDbContext>();
        //        var userManager = provider.GetRequiredService<UserManager<User>>();
        //        var signInManager = provider.GetRequiredService<SignInManager<User>>();
        //        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        //        var loginServiceconfiguration = new Mock<IConfiguration>().Object;
        //        ILoginService iLoginService = new LoginService(userManager, signInManager, loginServiceconfiguration);
        //        await db.Database.EnsureCreatedAsync();

        //        var user = new User
        //        {
        //            UserName = "registerModel1",
        //            Email = "registerModel1@gmail.com",
        //            Password = "Password.10K",
        //            Role = "Member",
        //            EmailConfirmed = true,
        //        };

        //        P7CreateRestApi.Models.LoginModel loginModel1 = new P7CreateRestApi.Models.LoginModel
        //        {
        //            Email = "registerModel1@gmail.com",
        //            Password = "Password.10K",
        //        };
        //        var createResult = await userManager.CreateAsync(user, user.Password);

        //        var roles = new[] { "Admin", "Member" };
        //        foreach (var role in roles)
        //        {

        //            if (!await roleManager.RoleExistsAsync(role))
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(role));
        //            }
        //        }
        //        Xunit.Assert.True(createResult.Succeeded);

        //        //var roleName = "Admin";
        //        //var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        //        //Xunit.Assert.True(roleResult.Succeeded || roleResult.Errors.Any(e => e.Code == "DuplicateRoleName"));
        //        //var addRoleResult = await userManager.AddToRoleAsync(user, roleName);
        //        //Xunit.Assert.True(addRoleResult.Succeeded);

        //        var httpContext = new DefaultHttpContext { RequestServices = provider };
        //        httpContextAccessor.HttpContext = httpContext;

        //        var principal = await signInManager.CreateUserPrincipalAsync(user);
        //        httpContext.User = principal;


        //        //Act
        //        var login = await iLoginService.Login(loginModel1);


        //        //Assert
        //        var claims = httpContext.User.Claims.ToList();
        //        Xunit.Assert.Contains(claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id);
        //        Xunit.Assert.Contains(claims, c => c.Type == ClaimTypes.Name && c.Value == user.UserName);
        //        Xunit.Assert.Contains(claims, c => c.Type == ClaimTypes.Email && c.Value == user.Email);
        //        Xunit.Assert.True(login.Succeeded);
        //        Xunit.Assert.False(login.IsNotAllowed);
        //        Xunit.Assert.False(login.IsLockedOut);




        //        ///// Arrange
        //        //Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
        //        //var config = new MapperConfiguration(cfg =>
        //        //{
        //        //    cfg.AddProfile<DtoProfile>();
        //        //}, loggerFactory);
        //        //IMapper mapper = config.CreateMapper();

        //        //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        //    .UseInMemoryDatabase(databaseName: "TestDatabase6")
        //        //    .Options;


        //        //var configuration = new ConfigurationBuilder()
        //        //    .AddInMemoryCollection(new Dictionary<string, string> { { "Jwt:Key", "value" } })
        //        //    .Build();
        //        //var services = new ServiceCollection();
        //        //services.AddSingleton<IConfiguration>(configuration);
        //        //services.AddLogging();
        //        //services.AddDbContext<ApplicationDbContext>(opts =>
        //        //    opts.UseInMemoryDatabase("TestDb"));

        //        //services.AddIdentity<User, IdentityRole>(options =>
        //        //{
        //        //    options.SignIn.RequireConfirmedEmail = false;


        //        //    options.Password.RequireDigit = true;
        //        //    options.Password.RequireLowercase = true;
        //        //    options.Password.RequireNonAlphanumeric = true;
        //        //    options.Password.RequireUppercase = true;
        //        //    options.Password.RequiredLength = 10;
        //        //    options.Password.RequiredUniqueChars = 1;

        //        //    options.SignIn.RequireConfirmedEmail = false;
        //        //    options.SignIn.RequireConfirmedAccount = false;
        //        //    options.SignIn.RequireConfirmedPhoneNumber = false;


        //        //    options.Lockout.AllowedForNewUsers = true;
        //        //    //User = new UserOptions
        //        //    //{
        //        //    //    RequireUniqueEmail = true,
        //        //    //},

        //        //})
        //        //.AddEntityFrameworkStores<ApplicationDbContext>()
        //        //.AddDefaultTokenProviders();


        //        //services.AddAuthentication(IdentityConstants.ApplicationScheme);
        //        //        //.AddCookie(IdentityConstants.ApplicationScheme);


        //        //services.AddHttpContextAccessor();

        //        //var provider = services.BuildServiceProvider();

        //        //using var scope = provider.CreateScope();
        //        //var sp = scope.ServiceProvider;
        //        //var context = sp.GetRequiredService<ApplicationDbContext>();


        //        //var userManager = sp.GetRequiredService<UserManager<User>>();
        //        //var signInManager = sp.GetRequiredService<SignInManager<User>>();
        //        //var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
        //        //var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        //        //var httpContext = new DefaultHttpContext { RequestServices = sp };
        //        //httpContextAccessor.HttpContext = httpContext;




        //        //IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
        //        //IUserService iUserService = new UserService(iUserRepository, mapper);
        //        //var loginServiceconfiguration = new Mock<IConfiguration>().Object;
        //        //ILoginService iLoginService = new LoginService(userManager, signInManager, loginServiceconfiguration);

        //        //var factory = sp.GetRequiredService<IUserClaimsPrincipalFactory<User>>();

        //        //var roles = new[] { "Admin", "Member" };
        //        //foreach (var role in roles)
        //        //{

        //        //    if (!await roleManager.RoleExistsAsync(role))
        //        //    {
        //        //        await roleManager.CreateAsync(new IdentityRole(role));
        //        //    }
        //        //}

        //        //P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
        //        //{
        //        //    UserName = "registerModel1",
        //        //    Email = "registerModel1@gmail.com",
        //        //    Password = "Password.10K",
        //        //    Role = "Member",
        //        //};
        //        //P7CreateRestApi.Domain.User user1 = new P7CreateRestApi.Domain.User
        //        //{
        //        //    UserName = "registerModel1",
        //        //    Email = "registerModel1@gmail.com",
        //        //    Password = "Password.10K",
        //        //    Role = "Member",
        //        //};

        //        //P7CreateRestApi.Models.LoginModel loginModel1 = new P7CreateRestApi.Models.LoginModel
        //        //{
        //        //    Email = "registerModel1@gmail.com",
        //        //    Password = "Password.10K",
        //        //};


        //        //var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
        //        //var t1 = await userManager.GetUserIdAsync(user1);
        //        //var t2 = await userManager.GetUserNameAsync(user1);
        //        //var t3 = await userManager.GetClaimsAsync(user1);
        //        //var t4 = await userManager.GetRolesAsync(user1);
        //        /////Act
        //        ////var login = await iLoginService.Login(loginModel1);
        //        //var principal = await factory.CreateAsync(user1);
        //        //httpContext.SignInAsync(principal);

        //        /////Assert
        //        //Xunit.Assert.True(createUser.Succeeded);
        //        ////Xunit.Assert.False(login.IsNotAllowed);
        //        //await iUserService.DeleteUserByEmail(registerModel1.Email);
        //    }
        //    //[Fact]
        //    //public async Task IBidLoginService_Login_ShouldLogin_1User()
        //    //{
        //    //    /// Arrange
        //    //    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
        //    //    var config = new MapperConfiguration(cfg =>
        //    //    {
        //    //        cfg.AddProfile<DtoProfile>();
        //    //    }, loggerFactory);
        //    //    IMapper mapper = config.CreateMapper();

        //    //    var context = GetInMemoryDbContext();
        //    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    //        .UseInMemoryDatabase(databaseName: "TestDatabase6")
        //    //        .Options;
        //    //    var configuration = new ConfigurationBuilder();
        //    //    var db = new ApplicationDbContext(options, configuration);
        //    //    var userstore = new UserStore<User>(db);
        //    //    var roleStore = new RoleStore<IdentityRole>(db);
        //    //    //var optionsDb = Options.Create(new IdentityOptions());
        //    //    //var optionsDb = Options.Create(new IdentityOptions
        //    //    //{
        //    //    //    Password = new PasswordOptions
        //    //    //    {
        //    //    //        RequireDigit = true,
        //    //    //        RequireLowercase = true,
        //    //    //        RequireNonAlphanumeric = true,
        //    //    //        RequireUppercase = true,
        //    //    //        RequiredLength = 10,
        //    //    //        RequiredUniqueChars = 1
        //    //    //    },
        //    //    //    User = new UserOptions
        //    //    //    {
        //    //    //        RequireUniqueEmail = true,
        //    //    //    },

        //    //    //});

        //    //    var optionsDb = Options.Create(new IdentityOptions
        //    //    {
        //    //        Password = new PasswordOptions
        //    //        {
        //    //            RequireDigit = true,
        //    //            RequireLowercase = true,
        //    //            RequireNonAlphanumeric = true,
        //    //            RequireUppercase = true,
        //    //            RequiredLength = 10,
        //    //            RequiredUniqueChars = 1
        //    //        },
        //    //        User = new UserOptions
        //    //        {
        //    //            RequireUniqueEmail = true,
        //    //        },
        //    //        SignIn = new SignInOptions
        //    //        {
        //    //            RequireConfirmedEmail = false,
        //    //            RequireConfirmedAccount = false,
        //    //            RequireConfirmedPhoneNumber = false,
        //    //        },
        //    //        Lockout = new LockoutOptions
        //    //        {
        //    //            AllowedForNewUsers = true,

        //    //        },


        //    //    });
        //    //    var passwordHasher = new PasswordHasher<User>();
        //    //    var userValidator = new List<IUserValidator<User>>();
        //    //    var userValidatorItem = new UserValidator<User>();
        //    //    userValidator.Add(userValidatorItem);
        //    //    int count = userValidator.Count();
        //    //    var passwordValidator = new List<IPasswordValidator<User>>();
        //    //    var passwordValidatorItem = new PasswordValidator<User>();
        //    //    passwordValidator.Add(passwordValidatorItem);
        //    //    var lookupNormalizer = new UpperInvariantLookupNormalizer();
        //    //    var identityErrorDescriber = new IdentityErrorDescriber();
        //    //    var iServiceProvider = new Mock<IServiceProvider>().Object;
        //    //    var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
        //    //    var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
        //    //    var roleValidator = new List<IRoleValidator<IdentityRole>>();
        //    //    var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
        //    //    var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
        //    //    IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
        //    //    IUserService iUserService = new UserService(iUserRepository, mapper);
        //    //    var httpContext = new DefaultHttpContext();
        //    //    //httpContext.Request.Headers["Authorization"] = "Bearer token123";
        //    //    var httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    //    httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext); 
        //    //    var userClaimsPrincipalFactory = new UserClaimsPrincipalFactory<User>(userManager, optionsDb);
        //    //    var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<SignInManager<User>>>().Object;
        //    //    var authOptions = new Mock<IAuthenticationSchemeProvider>().Object;
        //    //    var userConfirmation = new Mock<IUserConfirmation<User>>().Object;
        //    //    var iAuthService = new Mock<IAuthenticationService>();
        //    //    iAuthService.Setup(a => a.SignInAsync(It.IsAny<HttpContext>(),It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);

        //    //    var services = new ServiceCollection();
        //    //    services.AddSingleton(authOptions);
        //    //    services.AddSingleton<IUserClaimsPrincipalFactory<User>>(userClaimsPrincipalFactory);
        //    //    services.AddSingleton<IAuthenticationService>(iAuthService.Object);
        //    //    var provider = services.BuildServiceProvider();
        //    //    httpContext.RequestServices = provider;

        //    //    var signInManager = new SignInManager<P7CreateRestApi.Domain.User>(userManager, httpContextAccessor.Object, userClaimsPrincipalFactory, optionsDb, loggerMock, authOptions, userConfirmation);
        //    //    var loginServiceconfiguration = new Mock<IConfiguration>().Object;
        //    //    ILoginService iLoginService = new LoginService(userManager, signInManager, loginServiceconfiguration);
        //    //    CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

        //    //    var roles = new[] { "Admin", "Member" };
        //    //    foreach (var role in roles)
        //    //    {

        //    //        if (!await roleManager.RoleExistsAsync(role))
        //    //        {
        //    //            await roleManager.CreateAsync(new IdentityRole(role));
        //    //        }
        //    //    }

        //    //    P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
        //    //    {
        //    //        UserName = "registerModel1",
        //    //        Email = "registerModel1@gmail.com",
        //    //        Password = "Password.10K",
        //    //        Role = "Member",
        //    //    };
        //    //    P7CreateRestApi.Domain.User user1 = new P7CreateRestApi.Domain.User
        //    //    {
        //    //        UserName = "registerModel1",
        //    //        Email = "registerModel1@gmail.com",
        //    //        Password = "Password.10K",
        //    //        Role = "Member",
        //    //    };

        //    //    P7CreateRestApi.Models.LoginModel loginModel1 = new P7CreateRestApi.Models.LoginModel
        //    //    {
        //    //        Email = "registerModel1@gmail.com",
        //    //        Password = "Password.10K",
        //    //    };

        //    //    var principal = await userClaimsPrincipalFactory.CreateAsync(user1);
        //    //    var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
        //    //    var t1 = await userManager.GetUserIdAsync(user1);
        //    //    var t2 = await userManager.GetUserNameAsync(user1);
        //    //    var t3 = await userManager.GetClaimsAsync(user1);
        //    //    var t4 = await userManager.GetRolesAsync(user1);
        //    //    ///Act
        //    //    var login = await iLoginService.Login(loginModel1);

        //    //    ///Assert
        //    //    Xunit.Assert.True(createUser.Succeeded);
        //    //    Xunit.Assert.False(login.IsNotAllowed);
        //    //    await iUserService.DeleteUserByEmail(registerModel1.Email);
        //    //}


        //    [Fact]
        //    public async Task IBidLoginService_Login_ShouldNotLogin_1User()
        //    {
        //        /// Arrange
        //        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
        //        var config = new MapperConfiguration(cfg =>
        //        {
        //            cfg.AddProfile<DtoProfile>();
        //        }, loggerFactory);
        //        IMapper mapper = config.CreateMapper();

        //        var context = GetInMemoryDbContext();
        //        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //            .UseInMemoryDatabase(databaseName: "TestDatabase6")
        //            .Options;
        //        var configuration = new ConfigurationBuilder();
        //        var db = new ApplicationDbContext(options, configuration);
        //        var userstore = new UserStore<User>(db);
        //        var roleStore = new RoleStore<IdentityRole>(db);
        //        //var optionsDb = Options.Create(new IdentityOptions());
        //        var optionsDb = Options.Create(new IdentityOptions
        //        {
        //            Password = new PasswordOptions
        //            {
        //                RequireDigit = true,
        //                RequireLowercase = true,
        //                RequireNonAlphanumeric = true,
        //                RequireUppercase = true,
        //                RequiredLength = 10,
        //                RequiredUniqueChars = 1
        //            },
        //            User = new UserOptions
        //            {
        //                RequireUniqueEmail = true,
        //            },

        //        });
        //        var passwordHasher = new PasswordHasher<User>();
        //        var userValidator = new List<IUserValidator<User>>();
        //        var userValidatorItem = new UserValidator<User>();
        //        userValidator.Add(userValidatorItem);
        //        int count = userValidator.Count();
        //        var passwordValidator = new List<IPasswordValidator<User>>();
        //        var passwordValidatorItem = new PasswordValidator<User>();
        //        passwordValidator.Add(passwordValidatorItem);
        //        var lookupNormalizer = new UpperInvariantLookupNormalizer();
        //        var identityErrorDescriber = new IdentityErrorDescriber();
        //        var iServiceProvider = new Mock<IServiceProvider>().Object;
        //        var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
        //        var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
        //        var roleValidator = new List<IRoleValidator<IdentityRole>>();
        //        var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
        //        var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
        //        IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
        //        IUserService iUserService = new UserService(iUserRepository, mapper);
        //        var httpContextAccessor = new HttpContextAccessor();
        //        var userClaimsPrincipalFactory = new UserClaimsPrincipalFactory<User>(userManager, optionsDb);
        //        var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<SignInManager<User>>>().Object;
        //        var authOptions = new Mock<IAuthenticationSchemeProvider>().Object;
        //        var userConfirmation = new Mock<IUserConfirmation<User>>().Object;


        //        var signInManager = new SignInManager<P7CreateRestApi.Domain.User>(userManager, httpContextAccessor, userClaimsPrincipalFactory, optionsDb, loggerMock, authOptions, userConfirmation);
        //        var loginServiceconfiguration = new Mock<IConfiguration>().Object;
        //        ILoginService iLoginService = new LoginService(userManager, signInManager, loginServiceconfiguration);
        //        CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

        //        var roles = new[] { "Admin", "Member" };
        //        foreach (var role in roles)
        //        {

        //            if (!await roleManager.RoleExistsAsync(role))
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(role));
        //            }
        //        }

        //        P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
        //        {
        //            UserName = "registerModel1",
        //            Email = "registerModel1@gmail.com",
        //            Password = "Password.10K",
        //            Role = "Member",
        //        };

        //        P7CreateRestApi.Models.LoginModel loginModel1 = new P7CreateRestApi.Models.LoginModel
        //        {
        //            Email = "registerModel1@gmail.com",
        //            Password = "Pass",
        //        };

        //        var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
        //        ///Act
        //        var login1 = await iLoginService.Login(loginModel1);

        //        ///Assert
        //        Xunit.Assert.True(createUser.Succeeded);
        //        Xunit.Assert.True(login1.IsNotAllowed);
        //        Xunit.Assert.False(login1.Succeeded);
        //        await iUserService.DeleteUserByEmail(registerModel1.Email);
        //    }

        //    [Fact]
        //    public async Task IBidLoginService_Login_ShouldLogout_1User()
        //    {
        //        /// Arrange
        //        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
        //        var config = new MapperConfiguration(cfg =>
        //        {
        //            cfg.AddProfile<DtoProfile>();
        //        }, loggerFactory);
        //        IMapper mapper = config.CreateMapper();

        //        var context = GetInMemoryDbContext();
        //        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //            .UseInMemoryDatabase(databaseName: "TestDatabase6")
        //            .Options;
        //        var configuration = new ConfigurationBuilder();
        //        var db = new ApplicationDbContext(options, configuration);
        //        var userstore = new UserStore<User>(db);
        //        var roleStore = new RoleStore<IdentityRole>(db);
        //        //var optionsDb = Options.Create(new IdentityOptions());
        //        var optionsDb = Options.Create(new IdentityOptions
        //        {
        //            Password = new PasswordOptions
        //            {
        //                RequireDigit = true,
        //                RequireLowercase = true,
        //                RequireNonAlphanumeric = true,
        //                RequireUppercase = true,
        //                RequiredLength = 10,
        //                RequiredUniqueChars = 1
        //            },
        //            User = new UserOptions
        //            {
        //                RequireUniqueEmail = true,
        //            },

        //        });
        //        var passwordHasher = new PasswordHasher<User>();
        //        var userValidator = new List<IUserValidator<User>>();
        //        var userValidatorItem = new UserValidator<User>();
        //        userValidator.Add(userValidatorItem);
        //        int count = userValidator.Count();
        //        var passwordValidator = new List<IPasswordValidator<User>>();
        //        var passwordValidatorItem = new PasswordValidator<User>();
        //        passwordValidator.Add(passwordValidatorItem);
        //        var lookupNormalizer = new UpperInvariantLookupNormalizer();
        //        var identityErrorDescriber = new IdentityErrorDescriber();
        //        var iServiceProvider = new Mock<IServiceProvider>().Object;
        //        var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
        //        var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
        //        var roleValidator = new List<IRoleValidator<IdentityRole>>();
        //        var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
        //        var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
        //        IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
        //        IUserService iUserService = new UserService(iUserRepository, mapper);
        //        var httpContextAccessor = new HttpContextAccessor();
        //        var userClaimsPrincipalFactory = new UserClaimsPrincipalFactory<User>(userManager, optionsDb);
        //        var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<SignInManager<User>>>().Object;
        //        var authOptions = new Mock<IAuthenticationSchemeProvider>().Object;
        //        var userConfirmation = new Mock<IUserConfirmation<User>>().Object;


        //        var signInManager = new SignInManager<P7CreateRestApi.Domain.User>(userManager, httpContextAccessor, userClaimsPrincipalFactory, optionsDb, loggerMock, authOptions, userConfirmation);
        //        var loginServiceconfiguration = new Mock<IConfiguration>().Object;
        //        ILoginService iLoginService = new LoginService(userManager, signInManager, loginServiceconfiguration);
        //        CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

        //        var roles = new[] { "Admin", "Member" };
        //        foreach (var role in roles)
        //        {

        //            if (!await roleManager.RoleExistsAsync(role))
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(role));
        //            }
        //        }

        //        P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
        //        {
        //            UserName = "registerModel1",
        //            Email = "registerModel1@gmail.com",
        //            Password = "Password.10K",
        //            Role = "Member",
        //        };

        //        P7CreateRestApi.Models.LoginModel loginModel1 = new P7CreateRestApi.Models.LoginModel
        //        {
        //            Email = "registerModel1@gmail.com",
        //            Password = "Password.10K",
        //        };

        //        var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
        //        Task login = iLoginService.Login(loginModel1);

        //        ///Act
        //        Task logout = iLoginService.Logout();

        //        ///Assert
        //        Xunit.Assert.True(createUser.Succeeded);
        //        Xunit.Assert.True(login.IsCompletedSuccessfully);
        //        Xunit.Assert.True(logout.IsCompletedSuccessfully);
        //        await iUserService.DeleteUserByEmail(registerModel1.Email);
        //    }

        //    //public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        //    //{
        //    //    var store = new Mock<IUserStore<TUser>>();
        //    //    var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        //    //    mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        //    //    mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        //    //    mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        //    //    mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
        //    //    mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

        //    //        return mgr;




        //    //}
        //    ////private _userManager = MockUserManager<ApplicationUser>().Object; 

        //    //private readonly List<P7CreateRestApi.Domain.User> _users = new List<P7CreateRestApi.Domain.User>
        //    //    {
        //    //        new P7CreateRestApi.Domain.User { Email = "memberuser@test.com", Password = "Passmembertest15.", Role = "Member" },
        //    //        new P7CreateRestApi.Domain.User { Email = "adminuser@test.com", Password = "Passadmintest15.", Role = "Admin" }
        //    //    };
    }










    
}















