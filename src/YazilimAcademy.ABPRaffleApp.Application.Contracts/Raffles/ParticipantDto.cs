using System;
using Volo.Abp.Application.Dtos;
using YazilimAcademy.ABPRaffleApp.Domain.Shared;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public class ParticipantDto : AuditedEntityDto<Guid>
{
    public Guid RaffleId { get; set; }
    public FullName FullName { get; set; }
    public Email Email { get; set; }
}