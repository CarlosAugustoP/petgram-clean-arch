using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.MessageData
{
    public class MessageRepository(MainDBContext db) : IMessageRepository
    {
        private readonly MainDBContext _db = db;

        public async Task<Message> CreateAsync(Message message, CancellationToken cancellationToken)
        {
            _db.Messages.Add(message);
            await _db.SaveChangesAsync(cancellationToken);
            return message;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var message = await _db.Messages.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            if (message != null)
            {
                _db.Messages.Remove(message);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Messages.AnyAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<List<Message>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.Messages.ToListAsync(cancellationToken);
        }

        public async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Messages.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public Task<int> GetByUserChatCountAsync(Guid userIdSender, Guid userIdReceiver, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedList<(Message, int)>> GetLatestMessagesAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
            var query = _db.Messages
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .GroupBy(m => new { m.SenderId, m.ReceiverId })
                .Select(g => new
                {
                    Message = g.OrderByDescending(m => m.CreatedAt).FirstOrDefault(),
                    UnreadCount = g.Count(m => !m.IsRead && m.ReceiverId == userId)
                })
                .Select(x => new ValueTuple<Message, int>(x.Message!, x.UnreadCount));
            return await PaginatedList<(Message, int)>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public async Task<PaginatedList<Message>> GetMessagesByUserChatAsync(Guid userIdSender, Guid userIdReceiver, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
             var query = _db.Messages
                .Where(m => (m.SenderId == userIdSender && m.ReceiverId == userIdReceiver) ||
                             (m.SenderId == userIdReceiver && m.ReceiverId == userIdSender))
                .OrderByDescending(m => m.CreatedAt);

            return await PaginatedList<Message>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public async Task<int> GetUnreadMessageCountAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _db.Messages.CountAsync(m => m.ReceiverId == userId && !m.IsRead, cancellationToken);
        }

        public async Task<Message> UpdateAsync(Message message, CancellationToken cancellationToken)
        {
            _db.Messages.Update(message);
            await _db.SaveChangesAsync(cancellationToken);
            return message;
        }
    }
}