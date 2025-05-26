namespace EventBookingApp.Application.Interfaces;

public interface IWalletService
{
    Task<decimal> GetBalanceAsync(Guid userId);
    Task TopUpAsync(Guid userId, decimal amount);
    Task<bool> DeductAsync(Guid userId, decimal amount);
}
