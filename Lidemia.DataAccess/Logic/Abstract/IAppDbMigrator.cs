// Created on 18/11/2023 14:12 by shell

using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.DataAccess.Logic.Abstract;

public interface IAppDbMigrator
{
    Task MigrateDatabase(AsyncServiceScope scope);
}