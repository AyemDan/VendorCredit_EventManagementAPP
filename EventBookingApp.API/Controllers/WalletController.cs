using EventBookingApp.Application.DTOs;
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

    [HttpPost("top-up")]
    public IActionResult TopUp([FromBody] decimal amount)
    {
        var userId = "mock-user-id"; // Replace with real userId when auth is in
        _walletService.TopUp(userId, amount);
        var balance = _walletService.GetBalance(userId);
        return Ok(new { Message = "Top-up successful", Balance = balance });
    }

    [HttpGet("balance")]
    public IActionResult GetBalance()
    {
        var userId = "mock-user-id";
        var balance = _walletService.GetBalance(userId);
        return Ok(new { Balance = balance });
    }
}
