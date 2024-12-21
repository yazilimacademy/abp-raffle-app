using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Shared;
using YazilimAcademy.ABPRaffleApp.Raffles;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore.Configurations;

public sealed class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable(ABPRaffleAppConsts.DbTablePrefix + "Participants",
            ABPRaffleAppConsts.DbSchema);

        builder.ConfigureByConvention();

        // builder.OwnsOne(p => p.FullName,
        //     fullNameBuilder =>
        //     {
        //         fullNameBuilder.Property(fn => fn.FirstName)
        //         .IsRequired()
        //         .HasMaxLength(128)
        //         .HasColumnName("FirstName");

        //         fullNameBuilder.HasIndex(fn => fn.FirstName);

        //         fullNameBuilder.Property(fn => fn.LastName)
        //         .IsRequired()
        //         .HasMaxLength(128)
        //         .HasColumnName("LastName");

        //         fullNameBuilder.HasIndex(fn => fn.LastName);
        //     });

        builder.Property(x => x.FullName)
        .HasConversion(fullName => fullName.ToString(),
        value => FullName.Create(value))
        .IsRequired();

        builder.Property(x => x.FullName)
        .HasMaxLength(ParticipantConsts.MaxFullNameLength)
        .IsRequired();

        builder.HasIndex(x => x.FullName);

        builder.Property(p => p.Email)
        .HasConversion(email => email.Value,
        value => new Email(value))
        .IsRequired();

        builder.Property(x => x.Email)
        .HasMaxLength(ParticipantConsts.MaxEmailLength)
        .IsRequired();

        builder.HasIndex(x => x.Email)
        .IsUnique();
    }
}
