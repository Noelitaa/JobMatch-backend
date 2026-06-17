using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _dbContext;

    public NotificationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateManyAsync(List<Notification> notifications)
    {
        _dbContext.Notifications.AddRange(notifications);
        await _dbContext.SaveChangesAsync();
    }
}
