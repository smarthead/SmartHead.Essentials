using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SmartHead.Essentials.Extensions
{
    public static class HealthCheckExtensions
    {
        public static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(
                new JObject(
                    new JProperty("status", result.Status.ToString()),
                    new JProperty("results", new JObject(result.Entries.Select(pair =>
                        new JProperty(pair.Key, new JObject(
                            new JProperty("status", pair.Value.Status.ToString()),
                            new JProperty("description", pair.Value.Description))))))).ToString(Formatting.Indented));
        }
    }
}