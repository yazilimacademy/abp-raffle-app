using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities;

namespace YazilimAcademy.ABPRaffleApp.Domain.Raffles
{
    public class Raffle : FullAuditedAggregateRoot<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public bool IsActive { get; set; }

        // Concurrency
        public string ConcurrencyStamp { get; set; }

        // 1 -> N Relationship: Bir Raffle’ın birden çok Participant’ı
        public ICollection<Participants.Participant> Participants { get; set; }

        // EF Core’un ihtiyacı için korumalı ctor
        protected Raffle()
        {
        }

        public Raffle(Guid id, string name) : base(id)
        {
            Name = name;
            Participants = new List<Participants.Participant>();
        }
    }
}
