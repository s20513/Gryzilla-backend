﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Gryzilla_App.DTOs.Requests.User;

public class PutUserDto
{
    [Required]
    public int IdUser {get; set;}
    
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    [MinLength(5, ErrorMessage = "Min lenght : 5")]
    public string Nick { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Phone]
    public string PhoneNumber { get; set; } = null!;
    
    [MaxLength(100, ErrorMessage = "Max length : 100")]
    public string? LinkDiscord { get; set; }
    
    [MaxLength(100, ErrorMessage = "Max length : 100")]
    public string? LinkSteam { get; set; }
    
    [MaxLength(100, ErrorMessage = "Max length : 100")]
    public string? LinkEpic { get; set; }
    
    [MaxLength(100, ErrorMessage = "Max length : 100")]
    public string? LinkXbox { get; set; }
    
    [MaxLength(100, ErrorMessage = "Max length : 100")]
    public string? LinkPs { get; set; }
    
}