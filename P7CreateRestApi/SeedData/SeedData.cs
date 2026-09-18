using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using System;
using System.Linq;








namespace P7CreateRestApi.SeedData
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new P7Referential(
               serviceProvider.GetRequiredService<DbContextOptions<P7Referential>>());

            Console.WriteLine("hereseeddata");

            //---------------------------------------------------------
            //---------------------------------------------------------
            if (context.BidLists.Any())
            {
                return;
            }
            var bidList1 = new BidList 
            {
                Account = "test",
                BidType = "string",
                BidQuantity = 0.0001,
                AskQuantity = 0.0001,
                Bid = 0.0001,
                Ask = 0.0001,
                Benchmark = "string",
                BidListDate = DateTime.Now,
                Commentary = "string",
                BidSecurity = "string",
                BidStatus = "string",
                Trader = "string",
                Book = "string",
                CreationName = "string",
                CreationDate = DateTime.Now,
                RevisionName = "string",
                RevisionDate = DateTime.Now,
                DealName = "string",
                DealType = "string",
                SourceListId = "string",
                Side = "string"
            };
            

            context.BidLists.AddRange(bidList1);
            context.SaveChanges();



            //---------------------------------------------------------
            //---------------------------------------------------------
            if (context.CurvePoints.Any())
            {
                return;
            }
            var curvePoint1 = new CurvePoint
            {
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 0.0001,
                CurvePointValue = 0.0001,
                CreationDate = DateTime.Now,
            };


            context.CurvePoints.AddRange(curvePoint1);
            context.SaveChanges();




            //---------------------------------------------------------
            //---------------------------------------------------------
            if (context.Ratings.Any())
            {
                return;
            }
            var rating1 = new Rating
            {
                MoodysRating = "AAA",
                FitchRating = "AAA",
                SandPRating = "AAA",
                OrderNumber = 10,
            };


            context.Ratings.AddRange(rating1);
            context.SaveChanges();



            //---------------------------------------------------------
            //---------------------------------------------------------
            if (context.RuleNames.Any())
            {
                return;
            }
            var ruleName1 = new RuleName
            {
                Name = "test",
                Description = "testdescription",
                Json = "Json",
                Template = "Template",
                SqlStr = "testsqlstringfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
                SqlPart = "testsqlpartfhhhhhhhhhhhhhhhhhhhhhhhhh/fffffffffffffff/ffffffffff",
            };


            context.RuleNames.AddRange(ruleName1);
            context.SaveChanges();



            //---------------------------------------------------------
            //---------------------------------------------------------
            if (context.Trades.Any())
            {
                return;
            }
            var trade1 = new Trade
            {
                Account = "string",
                AccountType = "string",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                TradeDate = DateTime.Now,
                TradeSecurity = "string",
                TradeStatus = "string",
                Trader = "string",
                Benchmark = "string",
                Book = "string",
                CreationName = "string",
                CreationDate = DateTime.Now,
                RevisionName = "string",
                RevisionDate = DateTime.Now,
                DealName = "string"
            };


            context.Trades.AddRange(trade1);
            context.SaveChanges();





        }
    }
}
