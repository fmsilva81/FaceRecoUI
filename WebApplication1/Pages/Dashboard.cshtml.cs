using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

public class DashboardModel : PageModel
{

    private readonly IHttpClientFactory _httpClientFactory;


    public string StatusMessage { get; set; }

    public DashboardModel (IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }

    public async Task OnGet()
    {
        var client = _httpClientFactory.CreateClient("FaceApi");
        var response = await client.GetAsync("/health");
        StatusMessage = await response.Content.ReadAsStringAsync();
    }
}