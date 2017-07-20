using System;
using System.Drawing;
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
        [Route("ImageApi/Image/{participantOrder}")]
        public async Task<IHttpActionResult> UploadImage(string participantOrder)
        {
            try
            {
                var image = await Request.Content.ReadAsByteArrayAsync();

                var storageFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\{participantOrder}";
                var inputDirectoryFullPath = $"{storageFolderPath}\\{Resources.Resources.InputFolder}\\image.jpg";

                System.IO.File.WriteAllBytes(inputDirectoryFullPath, image);

                // TODO Use IoC
                var imageRotator = new ImageRotator();
                var whiteBoardExtractor = new WhiteBoardExtractor(new CornerFinderAccordingToRectangles(new SimilarityChecker(), imageRotator, new RectangleFinder()), imageRotator, new DarkAreaExtractor());
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
            var filePath = GetOutputFilePath(participantOrder);

            return CreateResponseMessageFromFile(filePath);
        }

        private static HttpResponseMessage CreateResponseMessageFromFile(string filePath)
        {
            var fileContent = System.IO.File.ReadAllBytes(filePath);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileContent)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }

        private static string GetOutputFilePath(string participantOrder)
        {
            var outputFolderParentFolder = participantOrder == "A" ? "B" : "A";
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\{outputFolderParentFolder}\\{Resources.Resources.OutputFolder}\\image.jpg";

            return filePath;
        }

        [HttpGet]
        [Route("ImageApi/Image/{participantOrder}/Dark")]
        public HttpResponseMessage GetLastImageWithOnlyDarkAreas(string participantOrder)
        {
            var filePath = GetDarkOutputFilePath(participantOrder);
            
            return CreateResponseMessageFromFile(filePath);
        }

        private static string GetDarkOutputFilePath(string participantOrder)
        {
            var outputFolderParentFolder = participantOrder == "A" ? "B" : "A";
            var filePath1 =
                $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\{outputFolderParentFolder}\\{Resources.Resources.OutputFolder}\\dark.jpg";
            var filePath = filePath1;
            return filePath;
        }
    }
}
