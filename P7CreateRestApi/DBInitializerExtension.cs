using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.Data;
using System;





namespace P7CreateRestApi
{
    internal static class DbInitializerExtension
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<P7Referential>();
                bool p7ReferentialCanConnect = context.Database.CanConnect();
                var identityContext = services.GetRequiredService<ApplicationDbContext>();
                bool identityCanConnect = identityContext.Database.CanConnect();

                if (!identityCanConnect)
                {
                    identityContext.Database.Migrate();
                }

                if (!p7ReferentialCanConnect)
                {
                    context.Database.Migrate();
                }
                
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration the DB.");
            }


            try
            {
                //Migration for P7Referential only
                SeedData.SeedData.Initialize(services);
                //Migration for p7Identity done in Program await IdentitySeedData.EnsurePopulated(app);

            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }



            return app;
        }
    }

}
