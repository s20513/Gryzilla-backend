﻿using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportCommentArticle;

public class DeleteReportCommentArticleDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdComment{ get; set; }
    
    [Required]
    public int IdReason { get; set; }
}