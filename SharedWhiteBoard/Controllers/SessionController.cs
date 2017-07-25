using System;
using System.IO;
using System.Web.Http;
using Services.Interfaces;

namespace SharedWhiteBoard.Controllers
{
    public class SessionController : ApiController
    {
        private readonly ISessionService _sessionService;
        private readonly IDirectoryStructureService _directoryService;

        public SessionController(ISessionService sessionService, IDirectoryStructureService directoryService)
        {
            _sessionService = sessionService;
            _directoryService = directoryService;
        }

        [HttpGet]
        [Route("SessionApi/Session")]
        public IHttpActionResult StartSession()
        {
            var session = _sessionService.CreateSession();

            var storageFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Resources.Resources.StorageFolder}\\{session.SessionPin}";
            _directoryService.CreateDirectoryStructureForBothParticipants(storageFolderPath);

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
