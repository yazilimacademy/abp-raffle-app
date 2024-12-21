using System;
using Volo.Abp.Application.Dtos;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public class ParticipantDto : AuditedEntityDto<Guid>
{
    public Guid RaffleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}