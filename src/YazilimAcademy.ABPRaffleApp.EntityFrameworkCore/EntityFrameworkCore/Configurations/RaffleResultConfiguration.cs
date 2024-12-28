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

        // RaffleResult -> Participant
        builder.HasOne(rr => rr.Participant)
        // If you want to see the RaffleResults from a participant, add:
        // .WithMany(p => p.RaffleResults)
        // else if you do not have such property, use .WithOne() or .WithMany() with no param
        .WithMany()
        .HasForeignKey(rr => rr.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
