using Solution.Domain.Interfaces;
using System.Threading.Tasks;

namespace Solution.Application.UseCases
{
    public abstract class UseCase<Input, Output>
    {
        protected readonly INotificationContext _notification;

        protected UseCase(INotificationContext notification) => _notification = notification;

        public abstract Output Execute(Input input);
    }
}
