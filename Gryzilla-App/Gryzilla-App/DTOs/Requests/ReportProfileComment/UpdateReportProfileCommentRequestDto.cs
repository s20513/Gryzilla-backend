﻿using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportProfileComment;

public class UpdateReportProfileCommentRequestDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdProfileComment{ get; set; }
    
    [Required]
    public int IdReason { get; set; }
    
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Content{ get; set; }
    
    public bool Viewed { get; set; }
}