using Refit;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace SA.Accounting.WPF.Handlers;

public static class GlobalExceptionHandler
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // ═══════════════════════════════════════════
    //  Register — في App.xaml.cs
    // ═══════════════════════════════════════════
    public static void Register(this Application app)
    {
        app.DispatcherUnhandledException += (_, e) =>
        {
            Handle(e.Exception);
            e.Handled = true;
        };

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            Handle(e.ExceptionObject as Exception);
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            Handle(e.Exception);
            e.SetObserved();
        };
    }

    // ═══════════════════════════════════════════
    //  Handle — public عشان الـ ViewModels تقدر تنادييه
    // ═══════════════════════════════════════════
    public static void Handle(Exception? exception)
    {
        if (exception is null) return;

        // فك الـ AggregateException لو موجود
        var ex = exception is AggregateException agg
            ? agg.Flatten().InnerExceptions.First()
            : exception;

        Debug.WriteLine($"[ERROR] {ex}");

        Application.Current.Dispatcher.Invoke(() =>
        {
            switch (ex)
            {
                case ApiException apiEx:
                    HandleApiException(apiEx);
                    break;

                case FluentValidation.ValidationException validationEx:
                    HandleValidationException(validationEx);
                    break;

                case HttpRequestException:
                    Show("لا يوجد اتصال بالإنترنت أو السيرفر غير متاح",
                         "خطأ في الاتصال", MessageBoxImage.Error);
                    break;

                case TaskCanceledException:
                    Show("انتهت مهلة الاتصال، حاول مرة أخرى",
                         "انتهت المهلة", MessageBoxImage.Warning);
                    break;

                default:
                    Show($"حدث خطأ غير متوقع:\n{ex.Message}",
                         "خطأ", MessageBoxImage.Error);
                    break;
            }
        });
    }

    // ═══════════════════════════════════════════
    //  API Errors
    // ═══════════════════════════════════════════
    private static void HandleApiException(ApiException ex)
    {
        switch (ex.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                HandleBadRequest(ex.Content);
                break;

            case HttpStatusCode.Unauthorized:
                Show("برجاء تسجيل الدخول مرة أخرى",
                     "غير مصرح", MessageBoxImage.Warning);
                break;

            case HttpStatusCode.Forbidden:
                Show("ليس لديك صلاحية للوصول",
                     "غير مسموح", MessageBoxImage.Warning);
                break;

            case HttpStatusCode.NotFound:
                Show("البيانات غير موجودة",
                     "غير موجود", MessageBoxImage.Information);
                break;

            case HttpStatusCode.InternalServerError:
                HandleServerError(ex.Content);
                break;

            default:
                Show($"خطأ من السيرفر: {(int)ex.StatusCode}",
                     "خطأ", MessageBoxImage.Error);
                break;
        }
    }

    private static void HandleBadRequest(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            Show("خطأ في البيانات المرسلة", "خطأ", MessageBoxImage.Warning);
            return;
        }

        try
        {
            var errors = JsonSerializer
                .Deserialize<List<ValidationError>>(content, _jsonOptions);

            if (errors is { Count: > 0 })
            {
                var message = string.Join("\n",
                    errors.Select(e => $"• {e.Property}: {e.Message}"));
                Show(message, "خطأ في البيانات", MessageBoxImage.Warning);
                return;
            }
        }
        catch { }

        Show(content, "خطأ في البيانات", MessageBoxImage.Warning);
    }

    private static void HandleServerError(string? content)
    {
        var title = "حدث خطأ في السيرفر";
        try
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var problem = JsonSerializer
                    .Deserialize<ProblemDetails>(content, _jsonOptions);
                title = problem?.Title ?? title;
            }
        }
        catch { }

        Show(title, "خطأ في السيرفر", MessageBoxImage.Error);
    }

    private static void HandleValidationException(
        FluentValidation.ValidationException ex)
    {
        var message = string.Join("\n",
            ex.Errors.Select(e => $"• {e.PropertyName}: {e.ErrorMessage}"));
        Show(message, "خطأ في البيانات", MessageBoxImage.Warning);
    }

    // ═══════════════════════════════════════════
    //  Helper
    // ═══════════════════════════════════════════
    private static void Show(string msg, string title, MessageBoxImage icon)
        => MessageBox.Show(msg, title, MessageBoxButton.OK, icon);
}

// DTOs
public class ValidationError
{
    public string Property { get; set; } = "";
    public string Message { get; set; } = "";
}

public class ProblemDetails
{
    public int? Status { get; set; }
    public string? Title { get; set; }
    public string? Type { get; set; }
}