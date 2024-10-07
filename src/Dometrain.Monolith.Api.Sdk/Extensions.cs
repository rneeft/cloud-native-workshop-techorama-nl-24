using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;

namespace Dometrain.Monolith.Api.Sdk;

public static class Extensions
{
    public static IServiceCollection AddDometrainApi(this IServiceCollection services,
        string baseUrl, string apiKey)
    {
        services.AddRefitClient<IStudentsApiClient>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("x-api-key", apiKey);
            }).AddStandardResilienceHandler();

        services.AddRefitClient<ICoursesApiClient>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("x-api-key", apiKey);
            }).AddResilienceHandler("default", builder =>
            {
                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    Delay = TimeSpan.FromSeconds(2),
                    UseJitter = true,
                    BackoffType = DelayBackoffType.Exponential,
                    MaxRetryAttempts = 2,
                    ShouldHandle = static args => ValueTask.FromResult(args is
                    {
                        Outcome.Result.StatusCode:
                        HttpStatusCode.RequestTimeout or
                        HttpStatusCode.TooManyRequests
                    })
                });
            });
        return services;
    }
}
