﻿namespace Gryzilla_App.DTOs.Responses.Group;

public class UserGroupDto
{
    public int IdGroup { get; set; }
    public int IdUserCreator { get; set; }
    public string? Nick { get; set; }
    public string GroupName { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}