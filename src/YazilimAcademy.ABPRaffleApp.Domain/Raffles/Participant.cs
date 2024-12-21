using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Domain.Shared;

namespace YazilimAcademy.ABPRaffleApp.Domain.Participants
{
    public class Participant : FullAuditedEntity<Guid>, IHasConcurrencyStamp
    {
        // Foreign Key - Tek bir Raffle’a ait
        public Guid RaffleId { get; set; }
        public Raffle Raffle { get; set; }

        // Value Object
        public FullName FullName { get; set; }

        // Başka bir iletişim bilgisi eklemek isterseniz:
        public Email Email { get; set; }

        // Concurrency 
        public string ConcurrencyStamp { get; set; }

        protected Participant()
        {
            // EF Core parametresiz ctor
        }

        public Participant(Guid id, Guid raffleId, FullName fullName, string email = null)
            : base(id)
        {
            RaffleId = raffleId;
            FullName = fullName;
            Email = email;
        }
    }
}
