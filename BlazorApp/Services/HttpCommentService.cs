namespace BlazorApp.Services;

using System.Text.Json;
using ApiContracts;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    // Opretter en ny comment
    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        // Step 1: Send POST request til Web API endpoint /comments
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
        
        // Step 2: Læs response body som tekst (JSON)
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld (status code 200-299)
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);  // Kast fejl med server's fejlbesked
        }
        
        // Step 4: Konverter JSON tekst til CommentDto objekt og returner
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true  // Ignorer store/små bogstaver i property navne
        })!;
    }

    // Opdaterer en eksisterende comment
    public async Task UpdateAsync(int id, UpdateCommentDto request)
    {
        // Step 1: Send PUT request til Web API endpoint /comments/{id}
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"comments/{id}", request);
        
        // Step 2: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Ingen return værdi - metoden returnerer bare Task (void)
    }

    // Sletter en comment
    public async Task DeleteAsync(int id)
    {
        // Step 1: Send DELETE request til Web API endpoint /comments/{id}
        HttpResponseMessage httpResponse = await client.DeleteAsync($"comments/{id}");
        
        // Step 2: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Ingen return værdi
    }

    // Henter en enkelt comment baseret på ID
    public async Task<CommentDto> GetSingleAsync(int id)
    {
        // Step 1: Send GET request til Web API endpoint /comments/{id}
        HttpResponseMessage httpResponse = await client.GetAsync($"comments/{id}");
        
        // Step 2: Læs response body som tekst (JSON)
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Konverter JSON til CommentDto og returner
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    // Henter alle comments (eller filtreret liste)
    public async Task<IEnumerable<CommentDto>> GetManyAsync(int? postId = null, int? userId = null)
    {
        // Step 1: Byg URL med query parameters hvis angivet
        string url = "comments?";
        
        // Tilføj postId parameter hvis angivet
        if (postId.HasValue)
        {
            url += $"postId={postId.Value}&";
        }
        
        // Tilføj userId parameter hvis angivet
        if (userId.HasValue)
        {
            url += $"userId={userId.Value}&";
        }
        
        // Fjern sidste '&' eller '?' hvis ingen parameters blev tilføjet
        url = url.TrimEnd('&', '?');
        
        // Step 2: Send GET request til Web API
        HttpResponseMessage httpResponse = await client.GetAsync(url);
        
        // Step 3: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 4: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 5: Konverter JSON array til liste af CommentDto objekter
        return JsonSerializer.Deserialize<IEnumerable<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
