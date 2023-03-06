using Solution.Domain.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Solution.Application
{
    public class NotificationContext : INotificationContext
    {
        public bool HasNotifications => Notifications.Any();
        public ImmutableArray<INotification> Notifications => _notifications.ToImmutableArray();
        
        private readonly List<INotification> _notifications;

        public NotificationContext() => _notifications = new();

        public void Add(INotification notification) => _notifications.Add(notification);
    }
}
