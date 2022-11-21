﻿using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class LikesArticleDbRepository : ILikesArticleDbRepository
{
    private readonly GryzillaContext _context;
    
    public LikesArticleDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    private async Task<Article?> GetArticle(int idArticle)
    {
        var article = await _context
            .Articles
            .Where(x => x.IdArticle == idArticle)
            .Include(x=>x.IdUsers)
            .SingleOrDefaultAsync();

        return article;
    }
    private async Task<UserDatum?> GetUser(int idUser)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == idUser)
            .SingleOrDefaultAsync();
        
        return user;
    }

    private async Task<UserDatum?> GetArticleLike(int idArticle, int idUser)
    {
        var articleLike = await _context
            .Articles
            .Where(x => x.IdArticle == idArticle)
            .SelectMany(x => x.IdUsers)
            .Where(x=>x.IdUser   == idUser)
            .SingleOrDefaultAsync();

        return articleLike;
    }
   
    public async Task<bool?> ExistLikeArticle(int idUser, int idArticle)
    {
        var user = await GetUser(idUser);
        var article = await GetArticle(idArticle);
        
        if (article is null || user is null)
        {
            return null;
        }
        var articleLike = await GetArticleLike(idArticle, idUser);
        
        return articleLike is not null;
    }

    public async Task<object?> DeleteLikeFromArticle(int idUser, int idArticle)
    {
        var user = await GetUser(idUser);
        var article = await GetArticle(idArticle);
        
        if (article is null || user is null)
        {
            return "Article or user doesn't exist";
        }
        
        var articleLike = await GetArticleLike(idArticle, idUser);
        
        if (articleLike is null)
        {
            return "Like has not been assigned";
        }

        article.IdUsers.Remove(user);
        await _context.SaveChangesAsync();
        
        return "Deleted like";   
    }

    public async Task<object?> AddLikeToArticle(int idUser, int idArticle)
    {
        var user = await GetUser(idUser);
        var article = await GetArticle(idArticle);
        
        if (article is null || user is null)
        {
            return "Article or user doesn't exist";
        }
        
        var articleLike = await GetArticleLike(idArticle, idUser);

        if (articleLike is not null)
        {
            return "Like has been assigned";
        }
        
        article.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        
        return "Added like";
    }
}