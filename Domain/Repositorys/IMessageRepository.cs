using Domain.Models;
using SharedKernel.Common;

namespace Domain.Repositorys
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Message>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<Message> CreateAsync(Message message, CancellationToken cancellationToken);
        Task<Message> UpdateAsync(Message message, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<Message>> GetMessagesByUserChatAsync(Guid userIdSender, Guid userIdReceiver, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
        Task<int> GetUnreadMessageCountAsync(Guid userId, CancellationToken cancellationToken);
        Task<int> GetByUserChatCountAsync(Guid userIdSender, Guid userIdReceiver, CancellationToken cancellationToken);
        Task<PaginatedList<(Message, int)>> GetLatestMessagesAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
    }
}