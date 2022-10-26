﻿using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Responses.ArticleComment;
using Gryzilla_App.DTOs.Responses.PostComment;

namespace Gryzilla_App.DTOs.Responses.Articles;

public class ArticleDto
{
    public int IdArticle { get; set; }
    public ReducedUserResponseDto? Author { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public TagDto[]? Tags { get; set; }
    public int? LikesNum { get; set; }
    public ArticleCommentDto[]? Comments { get; set; }
}