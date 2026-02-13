using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Address.Events;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Addresses.Commands;

public record DeleteAddressCommand(string AddressId) : IRequest<bool>;
public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteAddressCommandHandler(IAddressRepository addressRepository, IPublishEndpoint publishEndpoint)
    {
        _addressRepository = addressRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        await _addressRepository.DeleteAsync(request.AddressId, cancellationToken);
        await _publishEndpoint.Publish(
            new AddressDeletedEvent
            {
                Id = request.AddressId,
                DeletedOnUtc = DateTime.UtcNow
            });
        return true;
    }
}