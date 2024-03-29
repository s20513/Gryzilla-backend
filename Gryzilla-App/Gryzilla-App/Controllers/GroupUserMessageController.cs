﻿using Gryzilla_App.DTOs.Requests.GroupUserMessage;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/groupsMessage")]
public class GroupUserMessageController : Controller
{
    private readonly IGroupUserMessageDbRepository _groupUserMessageDbRepository;

    public GroupUserMessageController(IGroupUserMessageDbRepository groupUserMessageDbRepository)
    {
        _groupUserMessageDbRepository = groupUserMessageDbRepository;
    }
    
    /// <summary>
    /// Get list of messages
    /// </summary>
    /// <returns>
    /// NotFound - if any message doesn't exist
    /// Ok - List of group
    /// </returns>
    [HttpGet("{idGroup:int}")]
    public async Task<IActionResult> GetMessages([FromRoute] int idGroup)
    {
        var group = await _groupUserMessageDbRepository.GetMessages(idGroup);

        return Ok(group);
    }
    /// <summary>
    /// Modify message
    /// </summary>
    /// <param name="idMessage">Id message</param>
    /// <param name="updateGroupUserMessage">update group user message</param>
    /// <returns>
    /// BadRequest - Id from route and Id in body have to be same
    /// NotFound - There is no group with given id
    /// Ok - return modified group
    /// </returns>
    [HttpPut("{idMessage:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> ModifyMessage([FromRoute] int idMessage, [FromBody] UpdateGroupUserMessageDto updateGroupUserMessage)
    {
        if (idMessage != updateGroupUserMessage.IdMessage)
        {
            return BadRequest(new StringMessageDto{ Message = "Id from route and Id in body have to be same" });
        }
        
        var result = await _groupUserMessageDbRepository.ModifyMessage(idMessage, updateGroupUserMessage, User);
        
        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "There is no message with given id" });
        }
        
        return Ok(result);
    
    }
    /// <summary>
    /// Delete message
    /// </summary>
    /// <param name="idMessage">int - Message Identifier</param>
    /// <returns>
    /// NotFound - There is no group with given id
    /// Ok - return body of group
    /// </returns>
    [HttpDelete("{idMessage:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteMessage([FromRoute] int idMessage)
    {
        var result = await _groupUserMessageDbRepository.DeleteMessage(idMessage, User);

        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "There is no message with given id" });
        }

        return Ok(result);
    }
    /// <summary>
    /// Create new message
    /// </summary>
    /// <param name="addGroupUserMessageDto">Dto - to store information about new message</param>
    /// <returns>
    /// NotFound - Cannot add group or wrong userId
    /// Ok - return new post
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> CreateNewMessage([FromBody] AddGroupUserMessageDto addGroupUserMessageDto)
    {
        var result = await _groupUserMessageDbRepository.AddMessage(addGroupUserMessageDto);

        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "Wrong userId or groupId" });
        }
        return Ok(result);
    }
}