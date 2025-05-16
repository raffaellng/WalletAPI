using Microsoft.EntityFrameworkCore;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Interfaces;

namespace WalletAPI.Infrastructure.Data.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> ListByUserAsync(Guid senderUserId, DateTime? inicio, DateTime? fim)
        {
            var query = _context.Transactions
                .Include(t => t.ReceiverUser)
                .Where(t => t.SenderUserId == senderUserId)
                .AsQueryable();

            if (inicio.HasValue)
                query = query.Where(t => t.CreatedAt >= DateTime.SpecifyKind(inicio.Value, DateTimeKind.Utc));
            
            if (fim.HasValue)
                query = query.Where(t => t.CreatedAt <= DateTime.SpecifyKind(fim.Value, DateTimeKind.Utc));

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }
    }
}
