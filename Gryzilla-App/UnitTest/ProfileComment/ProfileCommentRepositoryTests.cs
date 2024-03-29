﻿using System.Security.Claims;
using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.ProfileComment;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest.ProfileComment;

public class ProfileCommentRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ProfileCommentDbRepository _repository;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public ProfileCommentRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ProfileCommentDbRepository(_context);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "User"),
        };
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(claims);
        _mockClaimsPrincipal
            .Setup(x => x.FindFirst(It.IsAny<string>()))
            .Returns<string>(claimType => claims.FirstOrDefault(c => c.Type == claimType));
    }

    private async Task AddTestDataWithManyUser()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "User",
            RankLevel = 4
        });
        await _context.SaveChangesAsync();

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick1",
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick3",
            Password = "Pass3",
            Email = "email3",
            CreatedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
        
        await _context.ProfileComments.AddAsync(new Gryzilla_App.Models.ProfileComment
        {
            IdUser = 1,
            IdUserComment = 2,
            Description = "Description",
            CreatedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task AddNewProfileCommentToDb_Returns_ProfileCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();

        
        var newProfileCommentRequestDto = new NewProfileComment
        {
            IdUserComment = 3,
            IdUser = 1,
            Content = "Komentarz"
        };
        
        //Act
        var res = await _repository.AddProfileCommentToDb(newProfileCommentRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var profileComments = _context.ProfileComments.ToList();
        Assert.True(profileComments.Exists(e => e.IdUser == res.IdUser));
    }
    
    [Fact]
    public async Task AddNewProfileCommentToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var newProfileCommentRequestDto = new NewProfileComment
        {
            IdUserComment = 1,
            Content = "Komentarz"
        };
        
        //Act
        var res = await _repository.AddProfileCommentToDb(newProfileCommentRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task AddNewProfileCommentToDb_Returns_NullIdUserComment()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithManyUser();
        
        var newProfileCommentRequestDto = new NewProfileComment
        {
            IdUserComment = 1000,
            IdUser = 1,
            Content = "Komentarz"
        };
        
        //Act
        var res = await _repository.AddProfileCommentToDb(newProfileCommentRequestDto);
        
        //Assert
        Assert.Null(res);
    }

    [Fact]
    public async Task AddNewProfileCommentToDb_Returns_NullUserComment()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithManyUser();
        var idUser = 2;
        var newProfileCommentRequestDto = new NewProfileComment
        {
            IdUserComment = 4,
            Content = "Komentarz"
        };
        
        //Act
        var res = await _repository.AddProfileCommentToDb(newProfileCommentRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task DeleteProfilecommentToDb_Returns_ProfileCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();

        var idProfileComment = 1;
        
        //Act
        var res = await _repository.DeleteProfileCommentFromDb(idProfileComment, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);

        var profileComments = _context.ProfileComments.ToList();
        Assert.False(profileComments.Exists(e => e.IdProfileComment == res.IdUser));
    }
    
    [Fact]
    public async Task DeleteProfileCommentToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idProfileComment = 1;
        
        //Act
        var res = await _repository.DeleteProfileCommentFromDb(idProfileComment, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyProfileCommentToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idProfileComment = 3;
        
        var modifyProfileCommentRequestDto = new ModifyProfileComment
        {
            Content = "Komentarz"
        };
        
        //Act
        var res = await _repository.ModifyProfileCommentFromDb(idProfileComment, modifyProfileCommentRequestDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyProfileCommentToDb_Returns_ProfileCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();
        
        var idProfileComment = 1;
        
        var modifyProfileCommentRequestDto = new ModifyProfileComment
        {
            Content = "Komentarz"
        };
        
        //Act
        var res = await _repository.ModifyProfileCommentFromDb(idProfileComment, modifyProfileCommentRequestDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        
        var profileComments = await _context.ProfileComments.SingleOrDefaultAsync(e =>
            e.IdProfileComment == idProfileComment
            && e.Description   == modifyProfileCommentRequestDto.Content);
        
        Assert.NotNull(profileComments);
    }
    
    [Fact]
    public async Task GetProfileCommentsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyUser();

        //Act
        var res = await _repository.GetProfileCommentFromDb(2);
        
        //Assert
        Assert.NotNull(res);
        
        var profileComments = await _context.ProfileComments.Select(e => e.IdUser).ToListAsync();
        Assert.Equal(profileComments, res.Select(e => e.IdUser).ToList());
    }
    
    [Fact]
    public async Task GetProfileCommentsToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 3;
        
        //Act
        var res = await _repository.GetProfileCommentFromDb(idUser);
        
        //Assert
        Assert.Null(res);
    }
}