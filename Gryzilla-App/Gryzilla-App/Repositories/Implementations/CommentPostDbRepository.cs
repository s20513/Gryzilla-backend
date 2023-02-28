﻿using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class  CommentPostDbRepository : ICommentPostDbRepository
{
    private readonly GryzillaContext _context;

    public CommentPostDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    public async Task<PostCommentDto?> AddCommentToPost(NewPostCommentDto newPostCommentDto)
    {
        int          idComment;
        Post?        post;
        UserDatum?   user;
        CommentPost? newCommentPost;

        post = await _context
            .Posts
            .Where(x => x.IdPost == newPostCommentDto.IdPost)
            .SingleOrDefaultAsync();
        
        if (post is null)
        {
            return null;
        }
        
        user = await _context
            .UserData
            .Where(x => x.IdUser == newPostCommentDto.IdUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        newCommentPost = new CommentPost
        {
            IdUser          = newPostCommentDto.IdUser,
            IdPost          = newPostCommentDto.IdPost,
            DescriptionPost = newPostCommentDto.Content
        };
        
        await _context.CommentPosts.AddAsync(newCommentPost);
        await _context.SaveChangesAsync();

        idComment = _context.CommentPosts.Max(x => x.IdComment);
        
        return new PostCommentDto
        {
            Nick        = user.Nick,
            IdUser      = user.IdUser,
            IdComment   = idComment,
            IdPost      = newPostCommentDto.IdPost,
            Description = newCommentPost.DescriptionPost
        };
    }

    public async Task<PostCommentDto?> ModifyPostCommentFromDb(PutPostCommentDto putPostCommentDto, int idComment)
    {
        string nick;
        var commentPost = await _context
            .CommentPosts
            .Where(x => 
                x.IdComment == idComment &&
                x.IdUser    == putPostCommentDto.IdUser &&
                x.IdPost    == putPostCommentDto.IdPost)
            .SingleOrDefaultAsync();
        
        if (commentPost is null)
        {
            return null;
        }

        commentPost.DescriptionPost = putPostCommentDto.Content;
        await _context.SaveChangesAsync();
        
        nick = await _context
            .UserData
            .Where(x => x.IdUser == putPostCommentDto.IdUser)
            .Select(x => x.Nick)
            .SingleAsync();
        
        return new PostCommentDto
        {
            Nick        = nick,
            IdComment   = idComment,
            IdPost      = putPostCommentDto.IdPost,
            IdUser      = putPostCommentDto.IdUser,
            Description = putPostCommentDto.Content
        };
    }

    public async Task<PostCommentDto?> DeleteCommentFromDb(int idComment)
    {
        string nick;
        var commentPost = await _context
            .CommentPosts
            .Where(x => x.IdComment == idComment)
            .SingleOrDefaultAsync();
        
        if (commentPost is null)
        {
            return null;
        }
        
        _context.CommentPosts.Remove(commentPost);
        await _context.SaveChangesAsync();
        
        nick = await _context
            .UserData
            .Where(x => x.IdUser == commentPost.IdUser)
            .Select(x => x.Nick)
            .SingleAsync();
        
        return new PostCommentDto
        {
            Nick        = nick,
            IdPost      = commentPost.IdPost,
            IdUser      = commentPost.IdUser,
            IdComment   = idComment,
            Description = commentPost.DescriptionPost
        };
    }
}