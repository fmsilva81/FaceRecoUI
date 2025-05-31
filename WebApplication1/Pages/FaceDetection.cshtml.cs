using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;


namespace WebApplication1.Pages
{


    public class FaceDetectionModel : PageModel
{
        private readonly IHttpClientFactory _httpClientFactory;

        public List<DetectionResult> DetectionResults { get; set; }

    public FaceDetectionModel(IHttpClientFactory httpClientFactory)
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

                var response = await client.PostAsync("/detect", content);

            var json = await response.Content.ReadAsStringAsync();
            DetectionResults = JsonSerializer.Deserialize<List<DetectionResult>>(json);
        }

        return Page();
    }
}

public class DetectionResult
{
    public int X { get; set; }
    public int Y { get; set; }
    public float Confidence { get; set; }
}
}

