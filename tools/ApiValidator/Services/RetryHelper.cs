namespace ApiValidator.Services;

public static class RetryHelper
{
    public static async Task<T> ExecuteWithRetryAsync<T>(
        Func<Task<T>> action,
        int maxAttempts = 3,
        Action<int, Exception>? onRetry = null)
    {
        var delays = new[] { 500, 1000, 2000 };
        Exception? lastException = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                lastException = ex;

                if (attempt < maxAttempts)
                {
                    onRetry?.Invoke(attempt, ex);
                    var delayMs = delays[Math.Min(attempt - 1, delays.Length - 1)];
                    await Task.Delay(delayMs);
                }
            }
        }

        throw lastException ?? new Exception("Retry failed with no exception");
    }

    public static async Task<(T Result, int Attempts)> ExecuteWithRetryAndCountAsync<T>(
        Func<Task<T>> action,
        int maxAttempts = 3,
        Action<int, Exception>? onRetry = null)
    {
        var delays = new[] { 500, 1000, 2000 };
        Exception? lastException = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var result = await action();
                return (result, attempt);
            }
            catch (Exception ex)
            {
                lastException = ex;

                if (attempt < maxAttempts)
                {
                    onRetry?.Invoke(attempt, ex);
                    var delayMs = delays[Math.Min(attempt - 1, delays.Length - 1)];
                    await Task.Delay(delayMs);
                }
            }
        }

        throw lastException ?? new Exception("Retry failed with no exception");
    }
}
