﻿using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.CommentPost;

public class CommentPostDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly CommentPostDbRepository _repository;

    public CommentPostDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new CommentPostDbRepository(_context);
    }

    private async void AddTestDataToDb()
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

        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async void AddCommentToPost_Returns_PostCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //AddTestDataToDb()
        
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

        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        
        var newPostCommentDto = new NewPostCommentDto
        {
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "DescPost1"
        };
        
        //Act
        var res = await _repository.AddCommentToPost(newPostCommentDto);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.Single(postComments);
        
        var postComment = postComments.SingleOrDefault(e => 
                                                    e.IdComment       == res.IdComment &&
                                                    e.IdPost          == res.IdPost &&
                                                    e.IdUser          == res.IdUser &&
                                                    e.DescriptionPost == res.Description);
        Assert.NotNull(postComment);
    }
    
    [Fact]
    public async void AddCommentToPost_With_No_Existing_Post_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var newPostCommentDto = new NewPostCommentDto
        {
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "DescPost1"
        };
        
        //Act
        var res = await _repository.AddCommentToPost(newPostCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void AddCommentToPost_With_No_Existing_User_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
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

        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();

        var newPostCommentDto = new NewPostCommentDto
        {
            IdUser = 2,
            IdPost = 1,
            DescriptionPost = "DescPost1"
        };
        
        //Act
        var res = await _repository.AddCommentToPost(newPostCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void ModifyPostCommentFromDb_Returns_PostCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

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

        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentPosts.AddAsync(new Gryzilla_App.Models.CommentPost
        {
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "DescPost1"
        });
        await _context.SaveChangesAsync();
        
        var idComment = 1;
        
        var putPostCommentDto = new PutPostCommentDto
        {
            IdComment = idComment,
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "NewDescPost1"
        };
        
        //Act
        var res = await _repository.ModifyPostCommentFromDb(putPostCommentDto, idComment);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.Single(postComments);
        
        var postComment = postComments.SingleOrDefault(e => 
            e.IdComment       == res.IdComment &&
            e.IdPost          == res.IdPost &&
            e.IdUser          == res.IdUser &&
            e.DescriptionPost == res.Description);
        Assert.NotNull(postComment);
    }
    
    [Fact]
    public async void ModifyPostCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

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

        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentPosts.AddAsync(new Gryzilla_App.Models.CommentPost
        {
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "DescPost1"
        });
        await _context.SaveChangesAsync();
        
        var idComment = 2;
        
        var putPostCommentDto = new PutPostCommentDto
        {
            IdComment = idComment,
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "NewDescPost1"
        };
        
        //Act
        var res = await _repository.ModifyPostCommentFromDb(putPostCommentDto, idComment);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void DeleteCommentFromDb_Returns_PostCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

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

        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentPosts.AddAsync(new Gryzilla_App.Models.CommentPost
        {
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "DescPost1"
        });
        await _context.SaveChangesAsync();

        var id = 1;
        
        //Act
        var res = await _repository.DeleteCommentFromDb(id);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.Empty(postComments);
    }
    
    [Fact]
    public async void DeleteCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

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

        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentPosts.AddAsync(new Gryzilla_App.Models.CommentPost
        {
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "DescPost1"
        });
        await _context.SaveChangesAsync();

        var id = 2;
        
        //Act
        var res = await _repository.DeleteCommentFromDb(id);
        
        //Assert
        Assert.Null(res);
    }
    
}