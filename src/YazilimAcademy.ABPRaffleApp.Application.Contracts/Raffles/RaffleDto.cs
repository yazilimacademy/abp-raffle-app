using System;
using Volo.Abp.Application.Dtos;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public sealed class RaffleDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset EndDate { get; set; }

    public bool IsActive { get; set; }
    public int ParticipantCount { get; set; }
}