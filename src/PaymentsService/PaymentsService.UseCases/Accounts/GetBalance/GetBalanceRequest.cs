using Microsoft.AspNetCore.Mvc;

namespace PaymentsService.UseCases.Accounts.GetBalance;

public sealed record GetBalanceRequest(
    [property: FromQuery] int UserId
    );