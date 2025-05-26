using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EventBookingApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            throw new Exception("User ID not found or invalid.");
        return userId;
    }

    [HttpPost("top-up")]
    public async Task<IActionResult> TopUp([FromBody] decimal amount)
    {
        var userId = GetUserId();

        await _walletService.TopUpAsync(userId, amount);
        var balance = await _walletService.GetBalanceAsync(userId);

        return Ok(new { Message = "Top-up successful", Balance = balance });
    }

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var userId = GetUserId();

        var balance = await _walletService.GetBalanceAsync(userId);
        return Ok(new { Balance = balance });
    }
}
