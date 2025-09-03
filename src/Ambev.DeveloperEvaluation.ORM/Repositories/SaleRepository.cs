using Ambev.DeveloperEvaluation.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<PaginatedList<Sale>> GetAllAsync(
        int pageNumber = AppConfiguration.DefaultPageNumber,
        int pageSize = AppConfiguration.DefaultPageSize,
        bool includeItems = true,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.AsQueryable();

        if (includeItems)
            query = query.Include(s => s.Items);

        return await PaginatedList<Sale>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
    }

    public async Task<Sale?> GetByIdAsync(Guid id, bool includeItems = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.AsQueryable();

        if (includeItems)
            query = query.Include(s => s.Items);

        return await query.FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}