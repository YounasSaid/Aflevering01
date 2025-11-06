namespace BlazorApp.Services;

using System.Text.Json;
using ApiContracts;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    // Opretter et nyt post
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        // Step 1: Send POST request til Web API endpoint /posts
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", request);
        
        // Step 2: Læs response body som tekst (JSON)
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld (status code 200-299)
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);  // Kast fejl med server's fejlbesked
        }
        
        // Step 4: Konverter JSON tekst til PostDto objekt og returner
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true  // Ignorer store/små bogstaver i property navne
        })!;
    }

    // Opdaterer et eksisterende post
    public async Task UpdateAsync(int id, UpdatePostDto request)
    {
        // Step 1: Send PUT request til Web API endpoint /posts/{id}
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"posts/{id}", request);
        
        // Step 2: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Ingen return værdi - metoden returnerer bare Task (void)
    }

    // Sletter et post
    public async Task DeleteAsync(int id)
    {
        // Step 1: Send DELETE request til Web API endpoint /posts/{id}
        HttpResponseMessage httpResponse = await client.DeleteAsync($"posts/{id}");
        
        // Step 2: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Ingen return værdi
    }

    // Henter et enkelt post baseret på ID
    public async Task<PostDto> GetSingleAsync(int id)
    {
        // Step 1: Send GET request til Web API endpoint /posts/{id}
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/{id}");
        
        // Step 2: Læs response body som tekst (JSON)
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Konverter JSON til PostDto og returner
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    // Henter alle posts (eller filtreret liste)
    public async Task<IEnumerable<PostDto>> GetManyAsync(string? titleContains = null, int? userId = null)
    {
        // Step 1: Byg URL med query parameters hvis angivet
        string url = "posts?";
        
        // Tilføj titleContains parameter hvis angivet
        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            url += $"titleContains={titleContains}&";
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
        
        // Step 5: Konverter JSON array til liste af PostDto objekter
        return JsonSerializer.Deserialize<IEnumerable<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
