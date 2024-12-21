
using System;
using System.ComponentModel.DataAnnotations;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public sealed class CreateUpdateRaffleDto
{
    [Required]
    [MaxLength(RaffleConsts.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [MaxLength(RaffleConsts.MaxDescriptionLength)]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow.AddDays(7);

    [Required]
    public bool IsActive { get; set; }
}
