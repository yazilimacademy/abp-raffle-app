using System;
using System.ComponentModel.DataAnnotations;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public class CreateUpdateParticipantDto
{
    [Required]
    public Guid RaffleId { get; set; }

    [Required]
    [StringLength(128)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(128)]
    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}