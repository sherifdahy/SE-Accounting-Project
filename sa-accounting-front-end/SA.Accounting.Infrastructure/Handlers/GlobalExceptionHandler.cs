using Refit;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace SA.Accounting.Infrastructure.Handlers;

public static class GlobalExceptionHandler
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Handle exceptions without UI dependencies (Infrastructure layer)
    /// </summary>
    public static void Handle(Exception? exception)
    {
        if (exception is null) return;

        // Flatten aggregate exceptions
        var ex = exception is AggregateException agg
            ? agg.Flatten().InnerExceptions.First()
            : exception;

        Debug.WriteLine($"[ERROR] {ex}");
        HandleApiException(ex);
    }

    private static void HandleApiException(Exception ex)
    {
        if (ex is ApiException apiEx)
        {
            Debug.WriteLine($"[API ERROR] Status: {apiEx.StatusCode}, Content: {apiEx.Content}");

            switch (apiEx.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    Debug.WriteLine($"[BAD REQUEST] {apiEx.Content}");
                    break;

                case HttpStatusCode.Unauthorized:
                    Debug.WriteLine($"[UNAUTHORIZED] User needs to login again");
                    break;

                case HttpStatusCode.Forbidden:
                    Debug.WriteLine($"[FORBIDDEN] User does not have permission");
                    break;

                case HttpStatusCode.NotFound:
                    Debug.WriteLine($"[NOT FOUND] Resource not found");
                    break;

                case HttpStatusCode.InternalServerError:
                    Debug.WriteLine($"[SERVER ERROR] Internal server error");
                    break;

                default:
                    Debug.WriteLine($"[API ERROR] {apiEx.StatusCode}: {apiEx.Content}");
                    break;
            }
        }
        else if (ex is HttpRequestException)
        {
            Debug.WriteLine($"[HTTP ERROR] No internet connection or server unavailable");
        }
        else if (ex is TaskCanceledException)
        {
            Debug.WriteLine($"[TIMEOUT] Request timed out");
        }
        else
        {
            Debug.WriteLine($"[UNEXPECTED ERROR] {ex.Message}");
        }
    }
}

