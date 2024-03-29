﻿using System.Security.Claims;
using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest.CommentArticle;

public class CommentArticleDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly CommentArticleDbRepository _repository;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public CommentArticleDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new CommentArticleDbRepository(_context);
        
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

    private async Task CreateTestData()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Rank1",
            RankLevel = 1
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

        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentArticles.AddAsync(new Gryzilla_App.Models.CommentArticle
        {
            IdUser = 1,
            IdArticle = 1,
            DescriptionArticle = "DescPost1",
            CreatedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async void AddCommentToArticle_Returns_ArticleCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();
        
        var newArticleCommentDto = new NewArticleCommentDto
        {
            IdUser = 1,
            IdArticle = 1,
            Content = "DescArticle1"
        };
        
        //Act
        var res = await _repository.AddCommentToArticle(newArticleCommentDto);
        
        //Assert
        Assert.NotNull(res);
        
        var articleComments = _context.CommentArticles.ToList();
        Assert.True(articleComments.Count == 2);
        
        var articleComment = articleComments.SingleOrDefault(e => 
            e.IdCommentArticle       == res.IdComment &&
            e.IdArticle          == res.IdArticle &&
            e.IdUser          == res.IdUser &&
            e.DescriptionArticle == res.Content);
        Assert.NotNull(articleComment);
    }
    
    [Fact]
    public async void  AddCommentToArticle_With_No_Existing_Article_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();
        
        var newArticleCommentDto = new NewArticleCommentDto
        {
            IdUser = 1,
            IdArticle = 2,
            Content = "DescArticle1"
        };
        
        //Act
        var res = await _repository.AddCommentToArticle(newArticleCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void  AddCommentToArticle_With_No_Existing_User_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await CreateTestData();

        var newArticleCommentDto = new NewArticleCommentDto
        {
            IdUser = 2,
            IdArticle = 1,
            Content = "DescArticle1"
        };
        
        //Act
        var res = await _repository.AddCommentToArticle(newArticleCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void ModifyArticleCommentFromDb_Returns_ArticleCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var idComment = 1;
        
        var putArticleCommentDto = new PutArticleCommentDto
        {
            IdComment = idComment,
            IdArticle = 1,
            Content = "NewDescArticle1"
        };
        
        //Act
        var res = await _repository.ModifyArticleCommentFromDb(putArticleCommentDto, idComment, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        
        var articleComments = _context.CommentArticles.ToList();
        Assert.Single(articleComments);
        
        var articleComment = articleComments.SingleOrDefault(e => 
            e.IdCommentArticle       == res.IdComment &&
            e.IdArticle          == res.IdArticle &&
            e.IdUser          == res.IdUser &&
            e.DescriptionArticle == res.Content);
        Assert.NotNull(articleComment);
    }
    
    [Fact]
    public async void ModifyArticleCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var idComment = 2;
        
        var putArticleCommentDto = new PutArticleCommentDto
        {
            IdComment = idComment,
            IdArticle = 1,
            Content = "NewDescArticle1"
        };
        
        //Act
        var res = await _repository.ModifyArticleCommentFromDb(putArticleCommentDto, idComment, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void DeleteArticleCommentFromDb_Returns_ArticleCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var id = 1;
        
        //Act
        var res = await _repository.DeleteArticleCommentFromDb(id, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.Empty(postComments);
    }
    
    [Fact]
    public async void DeleteArticleCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();

        var id = 2;
        
        //Act
        var res = await _repository.DeleteArticleCommentFromDb(id, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void GetCommentFromArticle_Returns_Ok()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await CreateTestData();
        
        //Act
        var res = await _repository.GetArticleCommentsFromDb(1);
        
        //Assert
        //Assert
        Assert.NotNull(res);
        
        var articleComments = _context.CommentArticles.Where(x=>x.IdArticle ==1).ToList();
        Assert.True(res.Comments != null && res.Comments.Count() == articleComments.Count());
    }
    
    [Fact]
    public async void  GetCommentFromArticle_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        //Act
        var res = await _repository.GetArticleCommentsFromDb(1);
        
        //Assert
        Assert.Null(res);
    }
}