namespace BlazorApp.Services;

using ApiContracts;

public interface ICommentService
{
    // Oprette ny comment
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    
    // Opdatere eksisterende comment
    Task UpdateAsync(int id, UpdateCommentDto request);
    
    // Slette comment
    Task DeleteAsync(int id);
    
    // Hente én specifik comment
    Task<CommentDto> GetSingleAsync(int id);
    
    // Hente alle comments (eller filtrerede comments)
    Task<IEnumerable<CommentDto>> GetManyAsync(int? postId = null, int? userId = null);
}
