using Lms.Data.Data;

namespace Lms.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var LmsDbContext = serviceProvider.GetRequiredService<LmsDbContext>();
                await SeedData.InitAsync(LmsDbContext);
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(string.Join(" ", ex.Message));
                //throw ex;
            }

        }

        return app;
    }
}
