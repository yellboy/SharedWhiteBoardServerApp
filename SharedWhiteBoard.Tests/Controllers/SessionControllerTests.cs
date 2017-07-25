using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using Services.Interfaces;
using SharedWhiteBoard.Controllers;
using SharedWhiteBoard.Models;
using Resources;

namespace SharedWhiteBoard.Tests.Controllers
{
    [TestFixture]
    public class SessionControllerTests
    {
        [Test]
        public void WhenStartSession_ThenNewSessionAndDirectoryStructureCreatedAndSessionPinReturned()
        {
            var dummySession = new Session();

            var sessionServiceMock = new Mock<ISessionService>();

            sessionServiceMock.Setup(m => m.CreateSession())
                .Returns(dummySession);

            var directoryServiceMock = new Mock<IDirectoryStructureService>();

            var sessionController = new SessionController(sessionServiceMock.Object, directoryServiceMock.Object);

            // When
            var resultSessionPin = ((OkNegotiatedContentResult<long>) sessionController.StartSession()).Content;
            

            // Then
            sessionServiceMock.Verify(m => m.CreateSession());
            directoryServiceMock.Verify(m => m.CreateDirectoryStructureForBothParticipants($"{AppDomain.CurrentDomain.BaseDirectory}{Resources.Resources.StorageFolder}\\{dummySession.SessionPin}"));
            Assert.AreEqual(dummySession.SessionPin, resultSessionPin);
        }
    }
}
