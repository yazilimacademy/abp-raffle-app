using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Raffles;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore.Configurations;

public sealed class RaffleConfiguration : IEntityTypeConfiguration<Raffle>
{
    public void Configure(EntityTypeBuilder<Raffle> builder)
    {
        builder.ToTable(ABPRaffleAppConsts.DbTablePrefix + "Raffles",
            ABPRaffleAppConsts.DbSchema);

        builder.Property(x => x.Name)
        .HasMaxLength(RaffleConsts.MaxNameLength)
        .IsRequired();

        builder.Property(x => x.Description)
        .HasMaxLength(RaffleConsts.MaxDescriptionLength)
        .IsRequired();

        builder.Property(x => x.StartDate)
        .IsRequired();

        builder.Property(x => x.EndDate)
        .IsRequired();

        builder.Property(x => x.IsActive)
        .IsRequired();

        builder.ConfigureByConvention();
    }
}
