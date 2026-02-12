using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Address.Events;
using CatalogService.Contracts.Address.Requests;
using CatalogService.Contracts.Address.Resposes;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Addresses.Commands;

public record CreateAddressCommand(CreateAddressRequest CreateAddressDto) : IRequest<AddressResponse>;
 public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressResponse>
 {
     private readonly IAddressRepository _addressRepository;
     private readonly IPublishEndpoint _publishEndpoint;

     public CreateAddressCommandHandler(IAddressRepository addressRepository, IPublishEndpoint publishEndpoint)
     {
         _addressRepository = addressRepository;
         _publishEndpoint = publishEndpoint;
     }
     public async Task<AddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
     {
         Address address = new Address()
         {
             City = request.CreateAddressDto.City,
             State = request.CreateAddressDto.State,
             Street = request.CreateAddressDto.Street,
             House = request.CreateAddressDto.House,
             ZipCode = request.CreateAddressDto.ZipCode,
             Description = request.CreateAddressDto.Description
         };
         var result = await _addressRepository.CreateAsync(address, cancellationToken);

         await _publishEndpoint.Publish(
             new AddressCreatedEvent
             {
                 Id = result.Id,
                 CreatedOnUtc = DateTime.UtcNow
             },
             cancellationToken);
         
         AddressResponse addressResponse = new AddressResponse()
         {
             Id = result.Id,
             City = result.City,
             State = result.State,
             Street = result.Street,
             House = result.House,
             ZipCode = result.ZipCode,
             Description = result.Description
         };
         return addressResponse;
     }
 }