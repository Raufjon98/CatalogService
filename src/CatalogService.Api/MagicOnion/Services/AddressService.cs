using CatalogService.Api.Features.Addresses.Commands;
using CatalogService.Api.Features.Addresses.Queries;
using CatalogService.Contracts.Address.Requests;
using CatalogService.Contracts.Address.Resposes;
using CatalogService.Contracts.Interfaces;
using MagicOnion;
using MagicOnion.Server;
using MediatR;

namespace CatalogService.Api.MagicOnion.Services;

public class AddressService : ServiceBase<IAddressService>, IAddressService
{
    private readonly IMediator _mediator;

    public AddressService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async UnaryResult<AddressResponse> CreateAddressAsync(CreateAddressRequest address)
    {
        var command = new CreateAddressCommand(address);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<AddressResponse> UpdateAddressAsync(string id, CreateAddressRequest address)
    {
        var command = new UpdateAddressCommand(id, address);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<bool> DeleteAddressAsync(string id)
    {
        var command = new DeleteAddressCommand(id);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<AddressResponse?> GetAddressAsync(string id)
    {
        var query = new GetAddressQuery(id);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<AddressResponse>> GetAllAddressesAsync()
    {
        var query = new GetAddressesQuery();
        var result = await _mediator.Send(query);
        return result;
    }
}