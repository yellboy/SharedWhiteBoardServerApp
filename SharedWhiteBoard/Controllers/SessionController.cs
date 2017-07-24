using System;
using System.IO;
using System.Web.Http;
using Services.Services;
using SharedWhiteBoard.Interfaces;

namespace SharedWhiteBoard.Controllers
{
    public class SessionController : ApiController
    {
        private readonly ISessionService _sessionService;

        public SessionController()
        {
            _sessionService = new SessionService();
        }

        [HttpGet]
        [Route("SessionApi/Session")]
        public IHttpActionResult StartSession()
        {
            var session = _sessionService.CreateSession();

            var storageFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Resources.Resources.StorageFolder}\\{session.SessionPin}";

            Directory.CreateDirectory(storageFolderPath);
            Directory.CreateDirectory($"{storageFolderPath}/A");
            Directory.CreateDirectory($"{storageFolderPath}/B");
            Directory.CreateDirectory($"{storageFolderPath}/A/{Resources.Resources.InputFolder}");
            Directory.CreateDirectory($"{storageFolderPath}/B/{Resources.Resources.InputFolder}");
            Directory.CreateDirectory($"{storageFolderPath}/A/{Resources.Resources.OutputFolder}");
            Directory.CreateDirectory($"{storageFolderPath}/B/{Resources.Resources.OutputFolder}");

            return Ok(session.SessionPin);
        }

        [HttpGet]
        [Route("SessionApi/Session/{sessionPin:long}")]
        public IHttpActionResult ConnectToExistingSession(long sessionPin)
        {
            var connectionSucceded = _sessionService.JoinSession(sessionPin);

            if (!connectionSucceded)
            {
                return BadRequest(Resources.Resources.NoSessionWithTheGivenPin);
            }

            return Ok(Resources.Resources.ConnectionSuccessfull);
        }

        [HttpGet]
        [Route("SessionApi/Session/{sessionPin:long}/End")]
        public IHttpActionResult EndSession(long sessionPin)
        {
            _sessionService.EndSession(sessionPin);
            
            return Ok();
        }
    }
}
