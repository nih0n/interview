using Solution.Domain.Interfaces;

namespace Solution.Domain
{
    public class Notification : INotification
    {
        public string Message { get; init; }

        public Notification(string message) => Message = message;
    }
}
