using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Interfaces;
using EventBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApp.Infrastructure.Services;

public class WalletService : IWalletService
{
    private readonly Dictionary<string, Wallet> _wallets = new();

    public decimal GetBalance(string userId)
    {
        if (_wallets.TryGetValue(userId, out var wallet))
            return wallet.Balance;
        return 0;
    }

    public void TopUp(string userId, decimal amount)
    {
        if (!_wallets.ContainsKey(userId))
            _wallets[userId] = new Wallet { UserId = Guid.Parse(userId), Balance = 0 };

        _wallets[userId].Balance += amount;
    }

    public bool Deduct(string userId, decimal amount)
    {
        if (!_wallets.ContainsKey(userId) || _wallets[userId].Balance < amount)
            return false;

        _wallets[userId].Balance -= amount;
        return true;
    }
}
