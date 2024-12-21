using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore.Configurations;

public sealed class RaffleConfiguration : IEntityTypeConfiguration<Raffle>
{
    public void Configure(EntityTypeBuilder<Raffle> builder)
    {
        builder.ToTable(ABPRaffleAppConsts.DbTablePrefix + "Raffles",
            ABPRaffleAppConsts.DbSchema);

        builder.Property(x => x.Name)
        .HasMaxLength(200)
        .IsRequired();

        builder.Property(x => x.Description)
        .HasMaxLength(2500)
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
