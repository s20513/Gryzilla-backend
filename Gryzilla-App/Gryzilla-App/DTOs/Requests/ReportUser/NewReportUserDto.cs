﻿using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportUser;

public class NewReportUserDto
{    
    [Required]
    public int IdUserReported { get; set; }
    [Required]
    public int IdUser{ get; set; }
    [Required]
    public int IdReason { get; set; }
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Content{ get; set; }
    
}