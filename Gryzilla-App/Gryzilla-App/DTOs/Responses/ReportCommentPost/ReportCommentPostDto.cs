﻿namespace Gryzilla_App.DTOs.Responses.ReportCommentPost;

public class ReportCommentPostDto
{
    public int IdUser { get; set; }
    
    public int IdComment{ get; set; }
    
    public int IdReason { get; set; }
    
    public string Description{ get; set; }
    
    public DateTime ReportedAt { get; set; }
    
    public bool Viewed { get; set; }
}