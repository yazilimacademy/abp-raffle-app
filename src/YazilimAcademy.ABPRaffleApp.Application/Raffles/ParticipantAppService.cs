using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Domain.Shared;
using YazilimAcademy.ABPRaffleApp.Permissions;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public class ParticipantAppService :
    ApplicationService,
    IParticipantAppService
{
    private readonly IRepository<Raffle, Guid> _raffleRepository;

    public ParticipantAppService(IRepository<Raffle, Guid> raffleRepository)
    {
        _raffleRepository = raffleRepository;
    }

    public async Task<ParticipantDto> GetAsync(Guid id)
    {
        var raffle = await FindRaffleByParticipantIdAsync(id);
        var participant = raffle.Participants.First(p => p.Id == id);
        return ObjectMapper.Map<Participant, ParticipantDto>(participant);
    }

    public async Task<PagedResultDto<ParticipantDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var raffles = await _raffleRepository.GetListAsync();
        var participants = raffles.SelectMany(r => r.Participants).ToList();

        var totalCount = participants.Count;
        var items = participants
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<ParticipantDto>(
            totalCount,
            ObjectMapper.Map<List<Participant>, List<ParticipantDto>>(items)
        );
    }

    public async Task<ParticipantDto> CreateAsync(CreateUpdateParticipantDto input)
    {
        var raffle = await _raffleRepository.GetAsync(input.RaffleId);

        var fullName = FullName.Create($"{input.FirstName} {input.LastName}");
        var participant = raffle.AddParticipant(
            GuidGenerator.Create(),
            fullName,
            input.Email
        );

        await _raffleRepository.UpdateAsync(raffle);
        return ObjectMapper.Map<Participant, ParticipantDto>(participant);
    }

    public async Task<ParticipantDto> UpdateAsync(Guid id, CreateUpdateParticipantDto input)
    {
        var raffle = await FindRaffleByParticipantIdAsync(id);
        var fullName = FullName.Create($"{input.FirstName} {input.LastName}");

        var participant = raffle.UpdateParticipant(
            id,
            fullName,
            input.Email
        );

        await _raffleRepository.UpdateAsync(raffle);
        return ObjectMapper.Map<Participant, ParticipantDto>(participant);
    }

    public async Task DeleteAsync(Guid id)
    {
        var raffle = await FindRaffleByParticipantIdAsync(id);
        raffle.RemoveParticipant(id);
        await _raffleRepository.UpdateAsync(raffle);
    }

    public async Task<PagedResultDto<ParticipantDto>> GetParticipantsByRaffleAsync(Guid raffleId, PagedAndSortedResultRequestDto input)
    {
        var raffle = await _raffleRepository.GetAsync(raffleId);
        var participants = raffle.Participants;

        var totalCount = participants.Count;
        var items = participants
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<ParticipantDto>(
            totalCount,
            ObjectMapper.Map<List<Participant>, List<ParticipantDto>>(items)
        );
    }

    private async Task<Raffle> FindRaffleByParticipantIdAsync(Guid participantId)
    {
        var raffles = await _raffleRepository.GetListAsync();

        var raffle = raffles.FirstOrDefault(r => r.Participants.Any(p => p.Id == participantId));

        if (raffle == null)
        {
            throw new InvalidOperationException($"Could not find the raffle containing participant with id: {participantId}");
        }

        return raffle;
    }
}