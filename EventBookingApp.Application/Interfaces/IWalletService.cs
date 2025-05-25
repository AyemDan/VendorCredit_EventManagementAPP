namespace EventBookingApp.Application.Interfaces;

public interface IWalletService
{
    decimal GetBalance(string userId);
    void TopUp(string userId, decimal amount);
    bool Deduct(string userId, decimal amount);
}
