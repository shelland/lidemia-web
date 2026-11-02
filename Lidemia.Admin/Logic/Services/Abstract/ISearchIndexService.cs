// Created on 02/11/2026 17:33 by Laserson

namespace Lidemia.Admin.Logic.Services.Abstract;

public interface ISearchIndexService
{
    Task RebuildIndex(CancellationToken cancellationToken);
}