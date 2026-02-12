using CatalogService.Api.Features.Common.interfaces;
using MediatR;

namespace CatalogService.Api.Features.Addresses.Commands;

public record DeleteAddressCommand(string AddressId) : IRequest<bool>;
public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;

    public DeleteAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;    
    }
    public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        return await _addressRepository.DeleteAsync(request.AddressId, cancellationToken);
    }
}