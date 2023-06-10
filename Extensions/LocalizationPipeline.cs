using Microsoft.Extensions.Options;

namespace WebAPI_dapper.Extensions
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app, RequestLocalizationOptions options)
        {
            
            app.UseRequestLocalization(options);
        }
    }
}
