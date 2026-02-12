using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Address.Requests;
using CatalogService.Contracts.Address.Resposes;
using MediatR;

namespace CatalogService.Api.Features.Addresses.Commands;

public record UpdateAddressCommand(string Id, CreateAddressRequest UpdateAddressDto) : IRequest<AddressResponse>;
public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, AddressResponse>
{
    private readonly IAddressRepository _addressRepository;

    public UpdateAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<AddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        Address address = new Address()
        {
            Id = request.Id,
            City = request.UpdateAddressDto.City,
            State = request.UpdateAddressDto.State,
            Street = request.UpdateAddressDto.Street,
            ZipCode = request.UpdateAddressDto.ZipCode,
            House = request.UpdateAddressDto.House,
            Description = request.UpdateAddressDto.Description
        };
        
        var result  = await _addressRepository.UpdateAsync(address, cancellationToken);
        if (result == null)
        {
            throw new Exception("Address was not updated!");
        }
        AddressResponse addressResponse = new AddressResponse()
        {
            Id = result.Id,
            City = result.City,
            State = result.State,
            Street = result.Street,
            ZipCode = result.ZipCode,
            House = result.House,
            Description = result.Description
        };
        return addressResponse;
    }
}