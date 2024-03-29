﻿namespace Gryzilla_App.DTOs.Responses.User;

public class UserDto
{
    public int IdUser {get; set;}
    public string Nick {get; set;} = null!;
    public string Email {get; set;} = null!;
    public string? PhoneNumber {get; set;}
    public int IdRank {get; set;}
    public string RankName {get; set;} = null!;
    public DateTime CreatedAt {get; set;}
    public string? LinkDiscord { get; set; }
    public string? LinkSteam { get; set; }
    public string? LinkEpic { get; set; }
    public string? LinkXbox { get; set; }
}