using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Pages;

public class FaceRecognitionModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public List<string> RecognitionResults { get; set; }

    public FaceRecognitionModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var file = Request.Form.Files["image"];
        if (file == null || file.Length == 0)
        {
            return Page();
        }

        using (var content = new MultipartFormDataContent())
        {
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.FileName);

            var client = _httpClientFactory.CreateClient("FaceApi");
            var response = await client.PostAsync("/recognize", content);

            var json = await response.Content.ReadAsStringAsync();
            RecognitionResults = JsonSerializer.Deserialize<List<string>>(json);

        }

        return Page();
    }
}
