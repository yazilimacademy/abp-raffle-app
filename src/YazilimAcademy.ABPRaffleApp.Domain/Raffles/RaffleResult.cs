using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;


namespace YazilimAcademy.ABPRaffleApp.Domain.Results
{
    public class RaffleResult : FullAuditedEntity<Guid>, IHasConcurrencyStamp
    {
        public Guid RaffleId { get; set; }
        public Raffle Raffle { get; set; }

        public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; }

        public bool IsWinner { get; set; }
        public int Order { get; set; }

        public string ConcurrencyStamp { get; set; }

        protected RaffleResult()
        {
        }

        public RaffleResult(Guid id, Guid raffleId, Guid participantId, bool isWinner = false, int order = 0)
            : base(id)
        {
            RaffleId = raffleId;
            ParticipantId = participantId;
            IsWinner = isWinner;
            Order = order;
        }
    }
}
