using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Address.Resposes;
using MediatR;

namespace CatalogService.Api.Features.Addresses.Queries;

public record GetAddressesQuery : IRequest<List<AddressResponse>>;

public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, List<AddressResponse>>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressesQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<List<AddressResponse>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = await _addressRepository.GetAllAsync(cancellationToken);
        List<AddressResponse> result = new List<AddressResponse>();
        foreach (var address in addresses)
        {
            AddressResponse addressResponse = new AddressResponse()
            {
                Id = address.Id,
                City = address.City,
                State = address.State,
                Street = address.Street,
                ZipCode = address.ZipCode,
                Description = address.Description,
                House = address.House
            };
            result.Add(addressResponse);
        }
        return result;
    }
}