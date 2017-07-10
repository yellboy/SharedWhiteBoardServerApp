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
        private static int _count = 0;

        [HttpPost]
        [Route("ImageApi/Image/{participantOrder}")]
        public async Task<IHttpActionResult> UploadImage(string participantOrder)
        {
            try
            {
                var image = await Request.Content.ReadAsByteArrayAsync();

                var inputDirectoryFullPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\{participantOrder}\\{Resources.Resources.InputFolder}\\image.jpg";
                System.IO.File.WriteAllBytes(inputDirectoryFullPath, image);
                
                var storageFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\{participantOrder}";

                // TODO Use IoC
                var imageRotator = new ImageRotator();
                var whiteBoardExtractor = new WhiteBoardExtractor(new CornerFinderAccordingToRectangles(new SimilarityChecker(), imageRotator, new RectangleFinder()), imageRotator);
                whiteBoardExtractor.DetectAndCrop(storageFolderPath);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet]
        [Route("ImageApi/Image/{participantOrder}")]
        public HttpResponseMessage GetLastImage(string participantOrder)
        {
            string filePath;
            if (participantOrder == "B")
            {
                filePath =
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\A\\{Resources.Resources.OutputFolder}\\image.jpg";
            }
            else
            {
                _count++;
                if (_count > 3)
                {
                    _count = 1;
                }

                filePath =
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\B\\{Resources.Resources.OutputFolder}\\image{_count}.jpg";
            }

            var fileContent = System.IO.File.ReadAllBytes(filePath);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileContent)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;

        }
    }
}
