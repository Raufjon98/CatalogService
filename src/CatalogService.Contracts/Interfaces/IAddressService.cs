using CatalogService.Contracts.Address.Requests;
using CatalogService.Contracts.Address.Resposes;
using MagicOnion;

namespace CatalogService.Contracts.Interfaces;

public interface IAddressService : IService<IAddressService>
{
    UnaryResult<AddressResponse> CreateAddressAsync(CreateAddressRequest address);
    UnaryResult<AddressResponse> UpdateAddressAsync(string addressId, CreateAddressRequest address);
    UnaryResult<bool> DeleteAddressAsync(string addressId);
    UnaryResult<AddressResponse?> GetAddressAsync(string addressId);
    UnaryResult<List<AddressResponse>> GetAllAddressesAsync();
}