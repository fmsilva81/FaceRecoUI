using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApplication1.Pages;

public class BatchDetectionModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public List<BatchDetectionResult> DetectionResults { get; set; }

    public BatchDetectionModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var files = Request.Form.Files;
        if (files.Count == 0)
        {
            return Page();
        }

        using (var content = new MultipartFormDataContent())
        {
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "images", file.FileName);
            }

            var client = _httpClientFactory.CreateClient("FaceApi");

            var response = await client.PostAsync("/batch-detect", content);

            var json = await response.Content.ReadAsStringAsync();
            DetectionResults = JsonSerializer.Deserialize<List<BatchDetectionResult>>(json);
        }

        return Page();
    }
}

public class BatchDetectionResult
{
    public string ImageName { get; set; }
    public List<FaceApiResponse> FaceResults { get; set; }
}
