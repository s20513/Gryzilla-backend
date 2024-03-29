﻿using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.ReportCommentPost;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.ReportCommentPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/reportCommentPost")]

public class ReportCommentPostController: Controller
{
    private readonly IReportCommentPostDbRepository _reportCommentPostDbRepository;

    public ReportCommentPostController(IReportCommentPostDbRepository reportCommentPostDbRepository)
    {
        _reportCommentPostDbRepository = reportCommentPostDbRepository;
    }

    /// <summary>
    ///  Get reports 
    /// </summary>
    /// <returns> List of reports</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportCommentPostDbRepository.GetReportCommentPostsFromDb();
        
        return Ok(reports);
    }

    /// <summary>
    /// Get one report
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <param name="idComment">Comment identifier</param>
    /// <param name="idReason">Reason identifier</param>
    /// <returns>Get one report</returns>
    [HttpGet("one/{idUser:int}/{idComment:int}/{idReason:int}")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReport([FromRoute] int idUser, [FromRoute] int idComment, [FromRoute] int idReason)
    {
        var report = await _reportCommentPostDbRepository.GetOneReportCommentPostFromDb(idUser, idComment, idReason);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given id found"});
        }
        
        return Ok(report);
    }
    
    /// <summary>
    /// Add Report Post Comment
    /// </summary>
    /// <param name="newReportCommentPost">NewReportCommentPostDto</param>
    /// <returns>NewReportCommentPostDto</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddReportPostComment([FromBody] NewReportCommentPostDto newReportCommentPost)
    {
        ReportCommentPostDto? report;
        try
        {
            report = await _reportCommentPostDbRepository.AddReportCommentPostToDb(newReportCommentPost);

            if (report is null)
            {
                return NotFound(new StringMessageDto{ Message = "User, comment or reason is wrong"});
            } 
        }
        catch (UserCreatorException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
        return Ok(report);
    }
    
    /// <summary>
    /// Delete Report Post Comment
    /// </summary>
    /// <param name="reportCommentPost">DeleteReportPostComment</param>
    /// <returns>DeleteReportPostComment</returns>
    [HttpDelete]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> DeleteReportPostComment([FromBody] DefaultReportCommentPostDto reportCommentPost)
    {
        var report = await _reportCommentPostDbRepository.DeleteReportCommentPostFromDb(reportCommentPost);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given id found"});
        }

        return Ok(report);
    }
    
    /// <summary>
    /// Update Report Comment Post Dto
    /// </summary>
    /// <param name="reportCommentPost">UpdateReportCommentPostDto</param>
    /// <returns>UpdateReportCommentPostDto</returns>
    [HttpPut]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> UpdateReportPostComment([FromBody] UpdateReportCommentPostDto reportCommentPost)
    {
        var report = await _reportCommentPostDbRepository.UpdateReportCommentPostFromDb(reportCommentPost);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given id found"});
        }

        return Ok(report);
    }
}