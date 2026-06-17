using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface INotificationRepository
{
    Task CreateManyAsync(List<Notification> notifications);
}
