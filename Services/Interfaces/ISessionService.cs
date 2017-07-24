using SharedWhiteBoard.Models;

namespace SharedWhiteBoard.Interfaces
{
    public interface ISessionService
    {
        Session GetSession(long sessionPin);

        Session CreateSession();

        bool JoinSession(long sessionPin);

        void EndSession(long sessionPin);
    }
}
