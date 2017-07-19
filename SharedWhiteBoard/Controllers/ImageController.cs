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
            var filePath = GetOutputFilePath(participantOrder);

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
        [Route("ImageApi/Image/{participantOrder}/DarkArea")]
        public IHttpActionResult GetNonWhiteAreaFromLastImage(string participantOrder)
        {
            var filePath = GetOutputFilePath(participantOrder);

            var darkAreaFinder = new DarkAreaFinder();
            var darkAreas = darkAreaFinder.Find(new Bitmap(Image.FromFile(filePath)));

            return Ok(darkAreas);
        }
    }
}
