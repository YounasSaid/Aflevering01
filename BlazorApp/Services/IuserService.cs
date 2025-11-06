namespace BlazorApp.Services;

using ApiContracts;

public interface IUserService
{
    // Oprette ny bruger
    Task<UserDto> AddUserAsync(CreateUserDto request);
    
    // Opdatere eksisterende bruger
    Task UpdateAsync(int id, UpdateUserDto request);
    
    // Slette bruger
    Task DeleteAsync(int id);
    
    // Hente én specifik bruger
    Task<UserDto> GetSingleAsync(int id);
    
    // Hente alle brugere (eller filtrerede brugere)
    Task<IEnumerable<UserDto>> GetManyAsync(string? usernameContains = null);
}
