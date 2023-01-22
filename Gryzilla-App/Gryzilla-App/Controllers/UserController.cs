using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserDbRepository _userDbRepository;
    public UserController(IUserDbRepository userDbRepository)
    {
        _userDbRepository = userDbRepository;
    }

    /// <summary>
    /// Find user by id
    /// </summary>
    /// <param name="idUser"> User Identifier</param>
    /// <returns>Return Status OK - if user exists, return user body, otherwise return status Not Found</returns>
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetUser([FromRoute] int idUser)
    {
        var user = 
            await _userDbRepository.GetUserFromDb(idUser);

        if (user == null)
        {
            return NotFound("User doesn't exist"); 
        }
        
        return Ok(user);
    }
    
    /// <summary>
    /// Find all users from db
    /// </summary>
    /// <returns>Return Ok - return list of users, NotFound - if there are no users</returns>
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var user = 
            await _userDbRepository.GetUsersFromDb();

        if (user is null)
        {
            return NotFound("Users don't exist");
        }

        return Ok(user);
    }
    /// <summary>
    ///  Modify user 
    /// </summary>
    /// <param name="idUser">Int - User Identifier</param>
    /// <param name="putUserDto">Dto to store new user information</param>
    /// <returns>Return Status Ok - information about user modified correctly, return user body, Not Found - User doesn't exist</returns>
    [HttpPut("{idUser:int}")]
    public async Task<IActionResult> ModifyUser([FromRoute] int idUser, [FromBody] PutUserDto putUserDto)
    {
        if (idUser != putUserDto.IdUser)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }
        try
        {
            var user = await _userDbRepository.ModifyUserFromDb(idUser, putUserDto);
        
            if (user == null)
            { 
                return NotFound("User doesn't exist");
            }
        
            return Ok(user);
        }
        catch (SameNameException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///  Add new user TO DO
    /// </summary>
    /// <param name="addUserDto">Dto - information about new user</param>
    /// <returns> Return Status Ok - New user added correctly, return user body</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> PostNewUser([FromBody] AddUserDto addUserDto){

        try
        {
            var user = await _userDbRepository.AddUserToDb(addUserDto);
        
            if (user == null)
            { 
                return NotFound("User doesn't exist");
            }
        
            return Ok(user);
        }
        catch (SameNameException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    ///  Delete a user using the User Identifier TO DO
    /// </summary>
    /// <param name="idUser"> int - User Identifier </param>
    /// <returns> Return Status OK - User deleted correctly - return user body. Return Status Not Found - if user doesn't exist</returns>
    [HttpDelete("{idUser:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int idUser)
    {
        var user = await _userDbRepository.DeleteUserFromDb(idUser);
        
        if (user == null)
        { 
            return NotFound("User doesn't exist");
        }
        
        return Ok(user);
    }
    
    /// <summary>
    ///  Creates token for User
    /// </summary>
    /// <param name="loginRequestDto"> LoginRequestDto - Nick and password given by user </param>
    /// <returns> Return Status OK - New token created. Return Status Forbid - given credentials are wrong</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var res = await _userDbRepository.Login(loginRequestDto);
        
        if (res == null)
        { 
            return Forbid();
        }
        
        return Ok(res);
    }
    
    /// <summary>
    ///  Refresh users token
    /// </summary>
    /// <param name="refreshToken"> string - refresh token </param>
    /// <returns> Return Status OK - New token created. Return Status Forbid - token expired</returns>
    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var res = await _userDbRepository.RefreshToken(refreshToken);
        
        if (res == null)
        { 
            return Forbid();
        }
        
        return Ok(res);
    }

}