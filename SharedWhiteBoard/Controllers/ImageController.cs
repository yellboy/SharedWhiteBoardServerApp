using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SharedWhiteBoard.Resources;

namespace SharedWhiteBoard.Controllers
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("ImageApi/Image")]
        public async Task UploadImage()
        {
            try
            {
                System.IO.File.AppendAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\log.txt", "Starting upload.\n");

                var image = await Request.Content.ReadAsByteArrayAsync();

                var inputDirectoryFullPath =
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.InputFolder}\\image.jpg";
                System.IO.File.WriteAllBytes(inputDirectoryFullPath, image);

                System.IO.File.AppendAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\log.txt", "Saved to input dir.\n");

                var whiteBoardDetectionAppPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\WhiteBoardDetection.exe";
                var storageFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}";
                var process = Process.Start(whiteBoardDetectionAppPath, storageFolderPath);

                System.IO.File.AppendAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\log.txt", $"Started process {whiteBoardDetectionAppPath} with parameter {storageFolderPath}\n");

                process.WaitForExit();

                System.IO.File.AppendAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\log.txt", $"Process ended.\n");
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\errors.txt", e.Message);
                System.IO.File.AppendAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\errors.txt", e.StackTrace);
            }
        }

        [HttpGet]
        [Route("ImageApi/Image")]
        public HttpResponseMessage GetLastImage()
        {
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.OutputFolder}\\image.jpg";
            var bytes = System.IO.File.ReadAllBytes(filePath);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }
    }
}
