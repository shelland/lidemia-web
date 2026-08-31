// Created on 8/2/2024 10:14 by Laserson

namespace Lidemia.Core.Extensions;

public static class TaskExtensions
{
    public static void FireAndForget(
        this Task task,
        Action<Exception>? errorHandler = null)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted && errorHandler != null)
                errorHandler(t.Exception);
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    public static async Task<TResult?> Retry<TResult>(this Func<Task<TResult>> taskFactory, int maxRetries,
        TimeSpan delay)
    {
        for (var i = 0; i < maxRetries; i++)
        {
            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch
            {
                if (i == maxRetries - 1)
                    throw;
                await Task.Delay(delay).ConfigureAwait(false);
            }
        }

        return default;
    }

    public static async Task<TResult?> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        var delayTask = Task.Delay(timeout);
        var completedTask = await Task.WhenAny(task, delayTask).ConfigureAwait(false);
        if (completedTask == delayTask)
            throw new TimeoutException();

        return await task;
    }

    public static async Task<TResult> Fallback<TResult>(this Task<TResult> task, TResult fallbackValue)
    {
        try
        {
            return await task.ConfigureAwait(false);
        }
        catch
        {
            return fallbackValue;
        }
    }

    public static async Task OnFailure(this Task task, Action<Exception> onFailure)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onFailure(ex);
        }
    }
}