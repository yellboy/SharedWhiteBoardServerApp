using System.Web.Http;

namespace SharedWhiteBoard.Controllers
{
    public class SessionController : ApiController
    {
        public static long? SessionPin;

        [HttpGet]
        [Route("SessionApi/Session")]
        public IHttpActionResult StartSession()
        {
            if (SessionPin.HasValue)
            {
                return BadRequest("Session already started");
            }

            // TODO Put into service
            SessionPin = 222222;

            return Ok(SessionPin);
        }

        [HttpGet]
        [Route("SessionApi/Session/{sessionPin:long}")]
        public IHttpActionResult ConnectToExistingSession(long sessionPin)
        {
            if (SessionPin != sessionPin)
            {
                return BadRequest();
            }

            return Ok("Connection successfull");
        }

        [HttpGet]
        [Route("SessionApi/Session/End")]
        public IHttpActionResult EndSession()
        {
            SessionPin = null;
            
            return Ok();
        }
    }
}
