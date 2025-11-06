namespace BlazorApp.Services;

using System.Text.Json;
using ApiContracts;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    // Opretter en ny bruger
    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        // Step 1: Send POST request til Web API endpoint /users
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);
        
        // Step 2: Læs response body som tekst (JSON)
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld (status code 200-299)
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);  // Kast fejl med server's fejlbesked
        }
        
        // Step 4: Konverter JSON tekst til UserDto objekt og returner
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true  // Ignorer store/små bogstaver i property navne
        })!;
    }

    // Opdaterer en eksisterende bruger
    public async Task UpdateAsync(int id, UpdateUserDto request)
    {
        // Step 1: Send PUT request til Web API endpoint /users/{id}
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"users/{id}", request);
        
        // Step 2: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Ingen return værdi - metoden returnerer bare Task (void)
    }

    // Sletter en bruger
    public async Task DeleteAsync(int id)
    {
        // Step 1: Send DELETE request til Web API endpoint /users/{id}
        HttpResponseMessage httpResponse = await client.DeleteAsync($"users/{id}");
        
        // Step 2: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Ingen return værdi
    }

    // Henter en enkelt bruger baseret på ID
    public async Task<UserDto> GetSingleAsync(int id)
    {
        // Step 1: Send GET request til Web API endpoint /users/{id}
        HttpResponseMessage httpResponse = await client.GetAsync($"users/{id}");
        
        // Step 2: Læs response body som tekst (JSON)
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 3: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 4: Konverter JSON til UserDto og returner
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    // Henter alle brugere (eller filtreret liste)
    public async Task<IEnumerable<UserDto>> GetManyAsync(string? usernameContains = null)
    {
        // Step 1: Byg URL med query parameter hvis usernameContains er angivet
        string url = "users";
        if (!string.IsNullOrWhiteSpace(usernameContains))
        {
            url += $"?userNameContains={usernameContains}";  // Tilføj query parameter
        }
        
        // Step 2: Send GET request til Web API
        HttpResponseMessage httpResponse = await client.GetAsync(url);
        
        // Step 3: Læs response
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        // Step 4: Tjek om request var succesfuld
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        // Step 5: Konverter JSON array til liste af UserDto objekter
        return JsonSerializer.Deserialize<IEnumerable<UserDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
