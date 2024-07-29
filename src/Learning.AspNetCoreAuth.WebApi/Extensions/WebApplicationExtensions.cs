using Serilog;

namespace Learning.AspNetCoreAuth.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseWebApiMiddlewares(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseExceptionHandler();
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseReDoc();
        }

        return app;
    }
}