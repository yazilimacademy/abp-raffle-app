using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;

namespace YazilimAcademy.ABPRaffleApp.Domain.Results
{
    public class RaffleResult : FullAuditedEntity<Guid>, IHasConcurrencyStamp
    {
        // Foreign Keys
        public Guid RaffleId { get; set; }
        public Raffle Raffle { get; set; }

        public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; }

        // Kazanan mı (Winner) yoksa yedek mi (Backup) olduğunu belirleyen alan
        public bool IsWinner { get; set; }

        // Eğer sıralamayı veya yedek sırasını tutmak isterseniz
        public int Order { get; set; }

        // Concurrency
        public string ConcurrencyStamp { get; set; }

        protected RaffleResult()
        {
            // EF Core için parametresiz ctor
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
