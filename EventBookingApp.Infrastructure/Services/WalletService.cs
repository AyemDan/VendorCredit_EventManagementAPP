using System;
using System.Threading.Tasks;
using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Interfaces;
using EventBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApp.Infrastructure.Services;

public class WalletService : IWalletService
{
    private readonly AppDbContext _context;

    public WalletService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetBalanceAsync(Guid userId)
    {
        var wallet = await _context
            .Wallets.AsNoTracking()
            .FirstOrDefaultAsync(w => w.UserId == userId);

        return wallet?.Balance ?? 0;
    }

    public async Task TopUpAsync(Guid userId, decimal amount)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            wallet = new Wallet { UserId = userId, Balance = 0 };
            _context.Wallets.Add(wallet);
        }

        wallet.Balance += amount;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeductAsync(Guid userId, decimal amount)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null || wallet.Balance < amount)
            return false;

        wallet.Balance -= amount;
        await _context.SaveChangesAsync();

        return true;
    }
}
