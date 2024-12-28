using System;
using System.Collections.Generic;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public sealed record DrawResultDto
{
    public List<ParticipantDto> Winners { get; set; }
    public List<ParticipantDto> Backups { get; set; }

    public DrawResultDto(List<ParticipantDto> winners, List<ParticipantDto> backups)
    {
        Winners = winners;
        Backups = backups;
    }
}