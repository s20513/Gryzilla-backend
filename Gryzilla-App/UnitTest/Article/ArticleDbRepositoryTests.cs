﻿using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Article;

public class ArticleDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ArticleDbRepository _repository;

    public ArticleDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ArticleDbRepository(_context);
    }
    
    private async Task AddTestDataWithManyArticles()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank()
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
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();

        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 2,
            Title = "Title2",
            CreatedAt = DateTime.Now,
            Content = "Content2"
        });
        await _context.SaveChangesAsync();

        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        
        await _context.Tags.AddAsync(new Gryzilla_App.Models.Tag
        {
            NameTag = "Tag1"
        });
        await _context.SaveChangesAsync();
        await _context.CommentArticles.AddAsync(new Gryzilla_App.Models.CommentArticle
        {
            IdArticle = 1,
            IdUser = 1,
            CreatedAt = DateTime.Now,
            DescriptionArticle = "Content"
        });
        await _context.SaveChangesAsync();
        await _context.CommentArticles.AddAsync(new Gryzilla_App.Models.CommentArticle
        {
            IdArticle = 2,
            IdUser = 1,
            CreatedAt = DateTime.Now,
            DescriptionArticle = "Content"
        });
        await _context.SaveChangesAsync();
        await _context.CommentArticles.AddAsync(new Gryzilla_App.Models.CommentArticle
        {
            IdArticle = 2,
            IdUser = 1,
            CreatedAt = DateTime.Now,
            DescriptionArticle = "Content"
        });
        await _context.SaveChangesAsync();
        var tag = await _context.Tags.FirstAsync();
        var article = await _context.Articles.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        article.IdTags.Add(tag);
        article.IdUsers.Add(user);

        await _context.SaveChangesAsync();
    }
    
    private async Task AddTestDataWithOneArticle()
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
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
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
        
        await _context.Tags.AddAsync(new Gryzilla_App.Models.Tag
        {
            NameTag = "Tag1"
        });
        await _context.SaveChangesAsync();
        await _context.Reasons.AddAsync(new Gryzilla_App.Models.Reason
        {
            ReasonName = "ReasonName"
        });
        await _context.SaveChangesAsync();
        var tag = await _context.Tags.FirstAsync();
        var article = await _context.Articles.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        article.IdTags.Add(tag);
        article.IdUsers.Add(user);

        await _context.SaveChangesAsync();

        await _context.CommentArticles.AddAsync(new Gryzilla_App.Models.CommentArticle
        {
            IdUser = 1,
            IdArticle = 1,
            DescriptionArticle = "DescArt1",
            CreatedAt = DateTime.Now
        });

        await _context.ReportCommentArticles.AddAsync(new Gryzilla_App.Models.ReportCommentArticle
        {
            IdUser = 2,
            Description = "Zła treść",
            IdReason = 1,
            IdCommentArticle = 1,
            ReportedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task GetArticleFromDb_Returns_ArticleDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneArticle();

        var id = 1;
        
        //Act
        var res = await _repository.GetArticleFromDb(id);
        
        //Assert
        Assert.NotNull(res);
        
        var articles = _context.Articles.ToList();
        Assert.NotNull(articles);
        
        var article = articles.SingleOrDefault(e => 
            e.IdArticle       == res.IdArticle);
        Assert.NotNull(article);
    }
    
    [Fact]
    public async void GetArticleFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneArticle();

        var id = 0;
        
        //Act
        var res = await _repository.GetArticleFromDb(id);
        
        //Assert
        Assert.Null(res);
    }

    [Fact]
    public async Task GetArticlesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();

        //Act
        var res = await _repository.GetArticlesFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context.Articles.Select(e => e.IdArticle).ToListAsync();
        Assert.Equal(articles, res.Select(e => e.IdArticle));
    }
    
    [Fact]
    public async Task GetArticlesFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetArticlesFromDb();
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetArticlesByMostLikesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();

        //Act
        var res = await _repository.GetArticlesByMostLikesFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
                                .Articles
                                .OrderByDescending(e => e.IdUsers.Count)
                                .Select(e => e.IdArticle)
                                .ToListAsync();
        
        
        Assert.Equal(articles, res.Select(e => e.IdArticle));
    }
    
    [Fact]
    public async Task GetArticlesByMostLikesFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetArticlesByMostLikesFromDb();
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetArticlesByLeastLikesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();

        //Act
        var res = await _repository.GetArticlesByLeastLikesFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
            .Articles
            .OrderBy(e => e.IdUsers.Count)
            .Select(e => e.IdArticle)
            .ToListAsync();
        
        
        Assert.Equal(articles, res.Select(e => e.IdArticle));
    }
    
    [Fact]
    public async Task GetArticlesByLeastLikesFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetArticlesByLeastLikesFromDb();
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetArticlesByEarliestDateFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();

        //Act
        var res = await _repository.GetArticlesByEarliestDateFromDb();
        
        //Assert
        Assert.NotNull(res);

        var articles = await _context
            .Articles
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => e.IdArticle)
            .ToListAsync();
        
        
        Assert.Equal(articles, res.Select(e => e.IdArticle));
    }
    
    [Fact]
    public async Task GetArticlesByEarliestDateFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetArticlesByEarliestDateFromDb();
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetArticlesByOldestDateFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();

        //Act
        var res = await _repository.GetArticlesByOldestDateFromDb();
        
        //Assert
        Assert.NotNull(res);

        var articles = await _context
            .Articles
            .OrderBy(e => e.CreatedAt)
            .Select(e => e.IdArticle)
            .ToListAsync();
        
        
        Assert.Equal(articles, res.Select(e => e.IdArticle));
    }
    
    [Fact]
    public async Task GetArticlesByOldestDateFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetArticlesByOldestDateFromDb();
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddNewArticleToDb_Returns_ArticleDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneArticle();

        var newArticleRequestDto = new NewArticleRequestDto
        {
            IdUser = 1,
            Title = "Fun Title",
            Content = "blablabla",
            Tags = new []
            {
                "Tag1",
                "Tag2"
            }
        };
        
        //Act
        var res = await _repository.AddNewArticleToDb(newArticleRequestDto);
        
        //Assert
        Assert.NotNull(res);
        
        var articles = _context.Articles.ToList();
        Assert.True(articles.Exists(e => e.IdArticle == res.IdArticle));
    }
    
    [Fact]
    public async Task AddNewArticleToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneArticle();

        var newArticleRequestDto = new NewArticleRequestDto
        {
            IdUser = 3,
            Title = "Fun Title",
            Content = "blablabla",
            Tags = new []
            {
                "Tag1",
                "Tag2"
            }
        };
        
        //Act
        var res = await _repository.AddNewArticleToDb(newArticleRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task DeleteArticleFromDb_Returns_ArticleDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneArticle();

        var id = 1;
        
        //Act
        var res = await _repository.DeleteArticleFromDb(id);
        
        //Assert
        Assert.NotNull(res);
        
        var articles = _context.Articles.ToList();
        Assert.False(articles.Exists(e => e.IdArticle == res.IdArticle));
    }
    
    [Fact]
    public async Task DeleteArticleFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneArticle();

        var id = 0;
        
        //Act
        var res = await _repository.DeleteArticleFromDb(id);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyArticleFromDb_Returns_ArticleDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneArticle();

        var id = 1;

        var putArticleRequestDto = new PutArticleRequestDto
        {
            IdArticle = id,
            Title = "NewTitle",
            Content = "NewContent",
            Tags = new []
            {
                "Tag1",
                "Tag2"
            }
        };
        
        //Act
        var res = await _repository.ModifyArticleFromDb(putArticleRequestDto, id);
        
        //Assert
        Assert.NotNull(res);
        
        var article = await _context.Articles.SingleOrDefaultAsync(e =>
               e.IdArticle == id
            && e.Title     == putArticleRequestDto.Title
            && e.Content   == putArticleRequestDto.Content);
        
        Assert.NotNull(article);
    }
    
    [Fact]
    public async Task ModifyArticleFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var id = 1;

        var putArticleRequestDto = new PutArticleRequestDto
        {
            IdArticle = id,
            Title = "NewTitle",
            Content = "NewContent"
        };
        
        //Act
        var res = await _repository.ModifyArticleFromDb(putArticleRequestDto, id);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetQtyArticlesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();

        //Act
        var res = await _repository.GetQtyArticlesFromDb(5);
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
            .Articles
            .Skip(0)
            .Take(5)
            .Select(e => e.IdArticle)
            .ToListAsync();


        if (res != null) Assert.Equal(articles, res.Articles.Select(x=>x.IdArticle));
    }
    [Fact]
    public async Task GetQtyArticlesFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyArticlesFromDb(5);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyArticlesFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyArticlesFromDb(4));
    }
    
    [Fact]
    public async Task GetQtyArticlesMostLikesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();
        
        DateTime time = DateTime.Now;

        //Act
        var res = await _repository.GetQtyArticlesByMostLikesFromDb(5,time);
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
            .Articles
            .Skip(0)
            .Take(5)
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdArticle)
            .ToListAsync();


        if (res != null) Assert.Equal(articles, res.Articles.Select(e => e.IdArticle));
    }
    [Fact]
    public async Task GetTopArticle_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetTopArticles();

        //Assert
        Assert.Null(res);
    }
    
    
    [Fact]
    public async Task GetTopArticle_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithManyArticles();

        //Act
        var res = await _repository.GetTopArticles();
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
            .Articles
            .Skip(0)
            .Take(3)
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdArticle)
            .ToListAsync();


        if (res != null) Assert.Equal(articles, res.Select(e => e.IdArticle));
    }
    
    [Fact]
    public async Task GetQtyArticlesByMostLikesFromDb_Returns_Null()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyArticlesByMostLikesFromDb(5, time);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyArticlesByMostLikesFromDb_Returns_WrongNumberException()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyArticlesByMostLikesFromDb(4,time));
    }
    
    [Fact]
    public async Task GetQtyArticlesByCommentsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithManyArticles();
        DateTime time = DateTime.Now;
        
        //Act
        var res = await _repository.GetQtyArticlesByCommentsFromDb(5,time);
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
            .Articles
            .Where(x=>x.CreatedAt < time)
            .Skip(0)
            .Take(5)
            .OrderByDescending(e => e.CommentArticles.Count)
            .Select(e => e.IdArticle)
            .ToListAsync();


        if (res != null) Assert.Equal(articles, res.Articles.Select(e => e.IdArticle));
    }
    [Fact]
    public async Task GetQtyArticlesByCommentsFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        DateTime time = DateTime.Now;

        //Act
        var res = await _repository.GetQtyArticlesByCommentsFromDb(5, time);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyArticlesCommentsFromDb_Returns_WrongNumberException()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyArticlesByCommentsFromDb(4, time));
    }
    
    [Fact]
    public async Task GetQtyArticleByDateFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();
        DateTime time = DateTime.Now;
        //Act
        var res = await _repository.GetQtyArticlesByEarliestDateFromDb(5, time);
        
        //Assert
        Assert.NotNull(res);

        var articles = await _context
            .Articles
            .Where(x=>x.CreatedAt < time)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(0)
            .Take(5)
            .Select(e => e.IdArticle)
            .ToListAsync();

        if (res != null) Assert.Equal(articles, res.Articles.Select(e => e.IdArticle));
        
    }
    [Fact]
    public async Task GetQtyArticlesByDateFromDb_Returns_Null()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyArticlesByEarliestDateFromDb(5, time);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyArticlesByDateFromDb_Returns_WrongNumberException()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyArticlesByEarliestDateFromDb(4, time));
    }
    [Fact]
    public async Task GetQtyArticlesByDateOldestFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();
        DateTime time = DateTime.Now;
        //Act
        var res = await _repository.GetQtyArticlesByOldestDateFromDb(5,time);
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
            .Articles
            .Skip(0)
            .Take(5)
            .OrderBy(e => e.CreatedAt)
            .Select(e => e.IdArticle)
            .ToListAsync();


        if (res != null) Assert.Equal(articles, res.Articles.Select(e => e.IdArticle));
    }
    [Fact]
    public async Task GetQtyPostsByDateOldestFromDb_Returns_Null()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyArticlesByOldestDateFromDb(5, time);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetArticlesByDateOldestFromDb_Returns_WrongNumberException()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyArticlesByOldestDateFromDb(4,time));
    }
    
    [Fact]
    public async Task GetUserArticlesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyArticles();

        var idUser = 1;

        //Act
        var res = await _repository.GetUserArticlesFromDb(idUser);

        //Assert
        Assert.NotNull(res);

        var articles = await _context.Articles
            .Where(e => e.IdUser == idUser)
            .Select(e => e.IdArticle)
            .ToListAsync();
        
        if (res != null) Assert.Equal(articles, res.Select(e => e.IdArticle));
    }
}