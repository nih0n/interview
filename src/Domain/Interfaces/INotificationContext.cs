using System.Collections.Immutable;

namespace Solution.Domain.Interfaces
{
    public interface INotificationContext
    {
        bool HasNotifications { get; }
        ImmutableArray<INotification> Notifications { get; }
        void Add(INotification notification);
    }
}
