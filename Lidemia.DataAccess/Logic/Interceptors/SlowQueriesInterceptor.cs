// Created on 20/11/2026 19:48 by Laserson

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace Lidemia.DataAccess.Logic.Interceptors;

public class SlowQueriesInterceptor : DbCommandInterceptor
{
    private readonly ILogger<SlowQueriesInterceptor> logger;
    private readonly TimeSpan threshold;

    public SlowQueriesInterceptor(ILogger<SlowQueriesInterceptor> logger, TimeSpan threshold)
    {
        this.logger = logger;
        this.threshold = threshold;
    }

    // Covers SELECT queries (Async)
    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    // Covers SELECT queries (Sync)
    public override DbDataReader ReaderExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result)
    {
        LogIfSlow(command, eventData);
        return base.ReaderExecuted(command, eventData, result);
    }

    // Covers INSERT, UPDATE, DELETE (Async)
    public override ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    // Covers COUNT, SUM, etc. (Async)
    public override ValueTask<object?> ScalarExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        object? result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
    }

    private void LogIfSlow(DbCommand command, CommandExecutedEventData eventData)
    {
        if (eventData.Duration > this.threshold)
        {
            this.logger.LogWarning(
                "Slow Query ({Duration}ms): {CommandText}",
                eventData.Duration.TotalMilliseconds,
                command.CommandText);
        }
    }
}