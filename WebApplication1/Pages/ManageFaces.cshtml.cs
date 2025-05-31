using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ManageFacesModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public List<string> FaceNames { get; set; }

    public ManageFacesModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("FaceApi");
        var response = await client.GetAsync("/faces");
        var json = await response.Content.ReadAsStringAsync();
        FaceNames = JsonSerializer.Deserialize<List<string>>(json);

    }

    public async Task<IActionResult> OnPostDeleteAsync(string name)
    {
        var client = _httpClientFactory.CreateClient("FaceApi");
        var response = await client.DeleteAsync($"/delete-face?name={name}");
        if (response.IsSuccessStatusCode)
        {
            FaceNames.Remove(name);
        }
        return RedirectToPage();
    }
}
