using CatalogService.Api.Domain.Entities;

namespace CatalogService.Api.Features.Common.interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
    Task<Category?> GetAsync(string id, CancellationToken cancellationToken);
    Task<Category> CreateAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> UpdateAsync(Category category, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string categoryId, CancellationToken cancellationToken);
}