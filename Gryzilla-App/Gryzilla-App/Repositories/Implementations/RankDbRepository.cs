using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses.Rank;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class RankDbRepository : IRankDbRepository
{
    private readonly GryzillaContext _context;

    public RankDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<RankDto?> AddNewRank(AddRankDto addRankDto)
    {
        var rank = await _context
            .Ranks
            .Where(x => x.Name == addRankDto.Name)
            .SingleOrDefaultAsync();

        if (rank is not null)
        {
            throw new SameNameException("Rank with given name already exists!");
        }
        
        var newRank = new Rank
        {
            Name      = addRankDto.Name,
            RankLevel = addRankDto.RankLevel
        };
        
        await _context.Ranks.AddAsync(newRank);
        await _context.SaveChangesAsync();
        
        return new RankDto
        {
            IdRank    = await _context.Ranks.Select(x => x.IdRank).OrderByDescending(x => x).FirstAsync(),
            Name      = newRank.Name,
            RankLevel = newRank.RankLevel
        };
        
    }

    public async Task<RankDto?> ModifyRank(PutRankDto putRankDto, int idRank)
    {
        var rankCount = _context
            .Ranks
            .Where(x =>x.IdRank != idRank)
            .Count(x => x.Name == putRankDto.Name);
            
        if (rankCount > 0)
        {
            throw new SameNameException("Rank with given name already exists!");
        }
        
        var rank = await _context
            .Ranks
            .Where(x => x.IdRank == idRank)
            .SingleOrDefaultAsync();

        if (rank is not null)
        {
            rank.Name      = putRankDto.Name;
            rank.RankLevel = putRankDto.RankLevel;
            await _context.SaveChangesAsync();
        
            return new RankDto
            {
                IdRank    = idRank,
                RankLevel = rank.RankLevel,
                Name      = rank.Name
            };   
        }

        return null;
    }

    public async Task<RankDto?> DeleteRank(int idRank)
    {
        var rank = await _context
            .Ranks
            .Where(x => x.IdRank == idRank)
            .SingleOrDefaultAsync();

        if (rank is null)
        {
            return null;
        }
        
        var userRank = await _context
            .UserData
            .Include(x => x.IdRankNavigation)
            .Where(x => x.IdRank == idRank)
            .ToArrayAsync();

        if (userRank.Length > 0)
        {
            throw new ReferenceException("Cannot delete. Some user have this rank!");
        }
        
        _context.Ranks.Remove(rank);
        await _context.SaveChangesAsync();
        
        return new RankDto
        {
            IdRank    = idRank,
            RankLevel = rank.RankLevel,
            Name      = rank.Name
        };
    }

    public async Task<RankDto[]> GetRanks()
    {
        var ranks =  await _context.Ranks
            .Select(e => new RankDto
            {
                IdRank    = e.IdRank,
                RankLevel = e.RankLevel,
                Name      = e.Name
            }).ToArrayAsync();
        
        return ranks;
    }
}