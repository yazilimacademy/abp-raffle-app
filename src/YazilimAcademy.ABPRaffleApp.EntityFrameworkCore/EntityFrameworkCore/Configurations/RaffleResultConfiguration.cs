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

        builder.HasOne<Raffle>()
        .WithMany()
        .HasForeignKey(rr => rr.RaffleId);

        builder.HasOne<Participant>()
        .WithMany()
        .HasForeignKey(rr => rr.ParticipantId);

        builder.Property(x => x.IsWinner)
        .IsRequired();

        builder.Property(x => x.Order)
        .IsRequired();
    }
}
