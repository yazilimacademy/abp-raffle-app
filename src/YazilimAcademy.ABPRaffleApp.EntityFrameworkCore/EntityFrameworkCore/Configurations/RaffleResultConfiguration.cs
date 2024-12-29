using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Domain.Results;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore.Configurations;

public sealed class RaffleResultConfiguration : IEntityTypeConfiguration<RaffleResult>
{
    public void Configure(EntityTypeBuilder<RaffleResult> builder)
    {
        builder.ToTable(ABPRaffleAppConsts.DbTablePrefix + "RaffleResults",
            ABPRaffleAppConsts.DbSchema);

        builder.ConfigureByConvention();

        builder.Property(x => x.IsWinner)
        .IsRequired();

        builder.Property(x => x.Order)
        .IsRequired();

        builder.HasOne(rr => rr.Raffle)
        .WithMany(r => r.RaffleResults)
        .HasForeignKey(rr => rr.RaffleId)
        .OnDelete(DeleteBehavior.Cascade); // or Restrict/NoAction as you wish

        builder.HasOne(rr => rr.Participant)
    .WithMany(p => p.RaffleResults) // Link to the new navigation property
    .HasForeignKey(rr => rr.ParticipantId)
    .OnDelete(DeleteBehavior.Cascade);

    }
}
