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
            var message = await _db.Messages.FindAsync(new object[] { id }, cancellationToken);
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

        public async Task<PaginatedList<Message>> GetMessagesByUserChatAsync(Guid userIdSender, Guid userIdReceiver, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
             var query = _db.Messages
                .Where(m => (m.SenderId == userIdSender && m.ReceiverId == userIdReceiver) ||
                             (m.SenderId == userIdReceiver && m.ReceiverId == userIdSender))
                .OrderByDescending(m => m.CreatedAt);

            return await PaginatedList<Message>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public async Task<Message> UpdateAsync(Message message, CancellationToken cancellationToken)
        {
            _db.Messages.Update(message);
            await _db.SaveChangesAsync(cancellationToken);
            return message;
        }
    }
}