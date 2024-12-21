using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Shared;

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

        // 1 -> N Relationship: Bir Raffle'ın birden çok Participant'ı
        public ICollection<Participant> Participants { get; private set; }

        // EF Core'un ihtiyacı için korumalı ctor
        protected Raffle()
        {
            Participants = new List<Participant>();
        }

        public Raffle(Guid id, string name) : base(id)
        {
            Name = name;
            Participants = new List<Participant>();
        }

        public Participant AddParticipant(Guid participantId, FullName fullName, string email)
        {
            var participant = new Participant(participantId, Id, fullName, email);
            Participants.Add(participant);
            return participant;
        }

        public Participant UpdateParticipant(Guid participantId, FullName fullName, string email)
        {
            var participant = Participants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
            {
                throw new InvalidOperationException($"Participant with id {participantId} not found in raffle {Id}");
            }

            participant.FullName = fullName;
            participant.Email = email;

            return participant;
        }

        public void RemoveParticipant(Guid participantId)
        {
            var participant = Participants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
            {
                throw new InvalidOperationException($"Participant with id {participantId} not found in raffle {Id}");
            }

            Participants.Remove(participant);
        }
    }
}
