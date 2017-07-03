using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using WhiteBoardDetection;

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
                var image = await Request.Content.ReadAsByteArrayAsync();

                var inputDirectoryFullPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.InputFolder}\\image.jpg";
                System.IO.File.WriteAllBytes(inputDirectoryFullPath, image);
                
                var storageFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}";

                // TODO Use IoC
                var imageRotator = new ImageRotator();
                var whiteBoardExtractor = new WhiteBoardExtractor(new RectangleFinder(), new CornerFinder(new SimilarityChecker(), imageRotator), imageRotator);
                whiteBoardExtractor.DetectAndCrop(storageFolderPath);
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
