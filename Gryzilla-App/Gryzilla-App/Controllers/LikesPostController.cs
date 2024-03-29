﻿using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/likesPost")]
public class LikesPostController : Controller
{
    private readonly ILikesPostDbRepository _likesPostDbRepository;

    public LikesPostController(ILikesPostDbRepository likesPostDbRepository)
    {
        _likesPostDbRepository= likesPostDbRepository;
    }

    /// <summary>
    ///  Add new like
    /// </summary>
    /// <param name="idUser">int idUser - User Identifier </param>
    /// <param name="idPost">int idPost - Post Identifier </param>
    /// <returns> 
    /// Status NotFound - If didn't find post or post  
    /// Status NotFound - If like has been assigned before
    /// Status Ok - added like successfully
    /// </returns>
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    [HttpPost("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> AddNewLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.AddLikeToPost(idUser, idPost);
        
        if (likes != null && !likes.Equals("Added like"))
        {
            return BadRequest(new StringMessageDto{ Message = likes });
        }
        
        return Ok(new StringMessageDto{Message = likes});
    }
    
    /// <summary>
    ///  Delete like
    /// </summary>
    /// <param name="idUser">int idUser - User Identifier </param>
    /// <param name="idPost">int idPost - Post Identifier </param>
    /// <returns> 
    /// Status NotFound - If didn't find post or post  
    /// Status NotFound - If like not been assigned before
    /// Status Ok - deleted like successfully
    /// </returns>
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    [HttpDelete("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> DeleteLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.DeleteLikeFromPost(idUser, idPost, User);
        
        if (likes != null && !likes.Equals("Deleted like"))
        {
            return BadRequest(new StringMessageDto{ Message = likes });
        }

        return Ok(new StringMessageDto{Message = likes});
    }
    
    /// <summary>
    ///  Exist method
    /// </summary>
    /// <param name="idUser">int idUser - User Identifier </param>
    /// <param name="idPost">int idPost - Post Identifier </param>
    /// <returns>
    /// true if like has been assigned, false - if not
    /// NotFound if user or post doesn't exist
    /// </returns>
    [Authorize(Roles = "Admin, User, Moderator, Redactor, Blocked")]
    [HttpGet("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> GetLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.ExistLike(idUser, idPost);
        
        if (likes is null)
        {
            return BadRequest(new StringMessageDto{ Message = "Post or user doesn't exist" }); 
        }
        return Ok(likes);
    }
}