namespace BlazorApp.Services;

using ApiContracts;

public interface IPostService
{
    // Oprette nyt post
    Task<PostDto> AddPostAsync(CreatePostDto request);
    
    // Opdatere eksisterende post
    Task UpdateAsync(int id, UpdatePostDto request);
    
    // Slette post
    Task DeleteAsync(int id);
    
    // Hente ét specifikt post
    Task<PostDto> GetSingleAsync(int id);
    
    // Hente alle posts (eller filtrerede posts)
    Task<IEnumerable<PostDto>> GetManyAsync(string? titleContains = null, int? userId = null);
}
