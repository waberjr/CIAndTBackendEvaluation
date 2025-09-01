using Ambev.DeveloperEvaluation.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<PaginatedList<Sale>> GetAllAsync(
        int pageNumber = AppConfiguration.DefaultPageNumber,
        int pageSize = AppConfiguration.DefaultPageSize,
        bool includeItems = true,
        CancellationToken cancellationToken = default);

    Task<Sale?> GetByIdAsync(Guid id, bool includeItems = true, CancellationToken cancellationToken = default);

    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
}