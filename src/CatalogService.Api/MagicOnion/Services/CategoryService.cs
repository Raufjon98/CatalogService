using CatalogService.Api.Features.Categories.Commands;
using CatalogService.Api.Features.Categories.Queries;
using CatalogService.Contracts.Category.Requests;
using CatalogService.Contracts.Category.Responses;
using CatalogService.Contracts.Interfaces;
using MagicOnion;
using MagicOnion.Server;
using MediatR;

namespace CatalogService.Api.MagicOnion.Services;

public class CategoryService : ServiceBase<ICategoryService>, ICategoryService
{
    private readonly IMediator _mediator;

    public CategoryService(IMediator mediator)
    {
        _mediator =  mediator;
    }
    public async UnaryResult<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest category)
    {
        var command = new CreateCategoryCommand(category);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<CategoryResponse> UpdateCategoryAsync(string categoryId, CreateCategoryRequest category)
    {
        var command = new UpdateCategoryCommand(categoryId, category);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<bool> DeleteCategoryAsync(string categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<CategoryResponse> GetCategoryAsync(string categoryId)
    {
        var query = new GetCategoryQuery(categoryId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<CategoryResponse>> GetAllCategoriesAsync()
    {
        var query = new GetCategoriesQuery();
        var result = await _mediator.Send(query);
        return result;
    }
}