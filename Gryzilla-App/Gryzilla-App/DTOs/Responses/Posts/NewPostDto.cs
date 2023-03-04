﻿using Gryzilla_App.DTOs.Responses.PostComment;

namespace Gryzilla_App.DTO.Responses.Posts;

public class NewPostDto
{
    public int IdPost { get; set; }
    public int IdUser { get; set; }
    public string Nick { get; set; }
    public string Content { get; set; }
    public string CreatedAt { get; set; }
    public string? [] Tags { get; set; }
    public int? Comments { get; set; }
    public int? Likes{ get; set; }
}