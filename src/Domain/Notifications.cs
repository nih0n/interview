using Solution.Domain.Interfaces;

namespace Solution.Domain
{
    public static class Notifications
    {
        public static readonly INotification CompanyAlreadyExists = new Notification("Company already exists");
        public static readonly INotification CompanyNotFound = new Notification("Company not found");
        public static readonly INotification GroupAlreadyExists = new Notification("Group already exists");
        public static readonly INotification GroupNotFound = new Notification("Group not found");
    }
}
