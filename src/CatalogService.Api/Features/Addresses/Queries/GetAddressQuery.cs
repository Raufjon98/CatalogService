using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Address.Resposes;
using MediatR;

namespace CatalogService.Api.Features.Addresses.Queries;

public record GetAddressQuery(string AddressId) : IRequest<AddressResponse>;
public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, AddressResponse>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<AddressResponse> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetAsync(request.AddressId, cancellationToken);

        if (address == null)
        {
            throw new NotFoundException(nameof(Address), request.AddressId);
        }
        
        AddressResponse result = new AddressResponse()
        {
            Id = address.Id,
            City = address.City,
            Street = address.Street,
            State = address.State,
            ZipCode = address.ZipCode,
            House = address.House,
            Description = address.Description,
        };
        return result;
    }
}