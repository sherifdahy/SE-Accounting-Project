using Microsoft.Playwright;
using SA.Accounting.Core.Contracts.Account.Responses;
using SA.Accounting.Core.Contracts.Selector.Responses;
using SA.Accounting.Core.Enums;
using SA.Accounting.Core.Interfaces;

public sealed class PlaywrightAccountAutomationService : IAccountAutomationService
{
    private IPlaywright? _playwright;
    private IBrowserContext? _context;
    private string? _profilePath;

    public async Task OpenAsync(
        AccountResponse account,
        CancellationToken cancellationToken = default)
    {
        ValidateAccount(account);

        _profilePath = CreateProfilePath();

        _playwright = await Playwright.CreateAsync();

        _context = await _playwright.Chromium.LaunchPersistentContextAsync(
            _profilePath,
            new BrowserTypeLaunchPersistentContextOptions
            {
                Headless = false,
                Channel = "chrome",
                ViewportSize = null,
                Args = new[]
                {
                    "--start-maximized",
                    "--no-first-run",
                    "--no-default-browser-check",

                    "--disable-save-password-bubble",
                    "--disable-password-generation",
                    "--disable-features=AutofillServerCommunication,PasswordManagerOnboarding,EnablePasswordsAccountStorage",

                    "--disable-sync",
                    "--disable-notifications"
                }
            });

        var page = _context.Pages.FirstOrDefault()
                   ?? await _context.NewPageAsync();

        await page.GotoAsync(account.Platform.Url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 60000
        });

        await ExecuteSelectorsAsync(
            page,
            account,
            cancellationToken);
    }

    public async Task CloseAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_context is not null)
                await _context.CloseAsync();
        }
        catch
        {
            // log if you want, but don't log credentials
        }

        try
        {
            _playwright?.Dispose();
        }
        catch
        {
            // ignore
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(_profilePath) &&
                Directory.Exists(_profilePath))
            {
                Directory.Delete(_profilePath, recursive: true);
            }
        }
        catch
        {
            // ممكن تعمل cleanup later
        }

        _context = null;
        _playwright = null;
        _profilePath = null;
    }

    private static void ValidateAccount(AccountResponse account)
    {
        if (account is null)
            throw new ArgumentNullException(nameof(account));

        if (string.IsNullOrWhiteSpace(account.Email))
            throw new InvalidOperationException("Account email is empty.");

        if (string.IsNullOrWhiteSpace(account.Password))
            throw new InvalidOperationException("Account password is empty.");

        if (account.Platform is null)
            throw new InvalidOperationException("Platform is required.");

        if (account.Platform.IsDeleted)
            throw new InvalidOperationException("Platform is deleted.");

        if (string.IsNullOrWhiteSpace(account.Platform.Url))
            throw new InvalidOperationException("Platform URL is empty.");

        if (account.Platform.Selectors is null || account.Platform.Selectors.Count == 0)
            throw new InvalidOperationException("Platform selectors are empty.");
    }

    private static string CreateProfilePath()
    {
        var path = Path.Combine(
            Path.GetTempPath(),
            "AccountingApp_Playwright_" + Guid.NewGuid());

        Directory.CreateDirectory(path);

        return path;
    }

    private static async Task ExecuteSelectorsAsync(
        IPage page,
        AccountResponse account,
        CancellationToken cancellationToken)
    {
        var selectors = account.Platform.Selectors
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .ToList();

        var dataQueue = new Queue<string>();

        dataQueue.Enqueue(account.Email);
        dataQueue.Enqueue(account.Password);

        foreach (var selector in selectors)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var locator = BuildLocator(page, selector).First;

            switch (selector.ContentType)
            {
                case SelectorContentType.Data:
                    if (dataQueue.Count == 0)
                    {
                        throw new InvalidOperationException(
                            $"No data value available for selector Id: {selector.Id}");
                    }

                    var value = dataQueue.Dequeue();

                    await FillAsync(locator, value, selector);
                    break;

                case SelectorContentType.Action:
                    await ClickAsync(page, locator, selector);
                    break;

                default:
                    throw new NotSupportedException(
                        $"Unsupported selector content type: {selector.ContentType}");
            }
        }
    }

    private static ILocator BuildLocator(
        IPage page,
        SelectorResponse selector)
    {
        if (string.IsNullOrWhiteSpace(selector.Value))
            throw new InvalidOperationException($"Selector value is empty. Id: {selector.Id}");

        return selector.Type switch
        {
            SelectorType.Id =>
                page.Locator($"#{EscapeCssIdentifier(selector.Value)}"),

            SelectorType.Name =>
                page.Locator($"[name=\"{EscapeCssAttributeValue(selector.Value)}\"]"),

            SelectorType.Class =>
                page.Locator($".{BuildClassSelector(selector.Value)}"),

            _ => throw new NotSupportedException(
                $"Unsupported selector type: {selector.Type}")
        };
    }

    private static async Task FillAsync(
        ILocator locator,
        string value,
        SelectorResponse selector)
    {
        try
        {
            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 15000
            });

            await locator.FillAsync(value);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to fill selector. SelectorId: {selector.Id}, SelectorValue: {selector.Value}",
                ex);
        }
    }

    private static async Task ClickAsync(
        IPage page,
        ILocator locator,
        SelectorResponse selector)
    {
        try
        {
            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 15000
            });

            await locator.ClickAsync();

            try
            {
                await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new()
                {
                    Timeout = 5000
                });
            }
            catch
            {
                // طبيعي في مواقع SPA
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to click selector. SelectorId: {selector.Id}, SelectorValue: {selector.Value}",
                ex);
        }
    }

    private static string EscapeCssAttributeValue(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"");
    }

    private static string EscapeCssIdentifier(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace(":", "\\:")
            .Replace(".", "\\.")
            .Replace("#", "\\#");
    }

    private static string BuildClassSelector(string value)
    {
        // لو Value = "btn login-btn"
        // النتيجة = "btn.login-btn"
        return string.Join(
            ".",
            value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                 .Select(x => EscapeCssIdentifier(x.Trim())));
    }
}