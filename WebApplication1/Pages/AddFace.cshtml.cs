using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class AddFaceModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public string Message { get; set; }

    public AddFaceModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var file = Request.Form.Files["image"];
        var name = Request.Form["name"];
        if (file == null || file.Length == 0 || string.IsNullOrEmpty(name))
        {
            return Page();
        }

        using (var content = new MultipartFormDataContent())
        {
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.FileName);
            content.Add(new StringContent(name), "name");

            var client = _httpClientFactory.CreateClient("FaceApi");
            var response = await client.PostAsync("/add-face", content);
            Message = await response.Content.ReadAsStringAsync();
        }

        return Page();
    }
}
