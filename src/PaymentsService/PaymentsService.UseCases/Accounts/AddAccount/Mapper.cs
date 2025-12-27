using PaymentsService.Entities.Models;
using Riok.Mapperly.Abstractions;

namespace PaymentsService.UseCases.Accounts.AddAccount;

[Mapper]
internal static partial class Mapper
{
    internal static partial AddAccountResponse ToDto(this Account account);
}