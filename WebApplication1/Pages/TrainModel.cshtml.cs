using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

public class TrainModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public string Message { get; set; }

    public TrainModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = _httpClientFactory.CreateClient("FaceApi");

        var response = await client.PostAsync("/train_custom_model", null);
        Message = await response.Content.ReadAsStringAsync();
        return Page();
    }
}

