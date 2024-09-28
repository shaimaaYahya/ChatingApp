using System;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//[Authorize]
public class UsersController(IUserRepositry userRepositry, IMapper mapper, IPhotoService photoService) : BaseApiController
{
    //private readonly DataContext _context = context;
    //[AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        //var users = await userRepositry.GetUsersAsync();
        //var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);
        var users = await userRepositry.GetMembersAsync();
        return Ok(users);
    }

    //[Authorize]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        //var user = await userRepositry.GetUserByUsernameAsync(username);
        var user = await userRepositry.GetMemberAsync(username);
        if (user == null) return NotFound();
        //var userToReturn = mapper.Map<MemberDto>(user);

        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {

        // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // if(username == null) return BadRequest("No username found in token"); 

        var user = await userRepositry.GetUserByUsernameAsync(User.GetUserName());

        if (user == null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDto, user);

        //userRepositry.Update(user);

        if (await userRepositry.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {

        var user = await userRepositry.GetUserByUsernameAsync(User.GetUserName());

        if (user == null) return BadRequest("Cannot update user");

        var result = photoService.AddPhoto(file);

        var photo = new Photo
        {
            Url = result,
            PublicId = result,
        };

        user.Photos.Add(photo);

        if (await userRepositry.SaveAllAsync())
            return CreatedAtAction(nameof(GetUser),
                new { username = user.UserName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepositry.GetUserByUsernameAsync(User.GetUserName());

        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await userRepositry.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting main photo");

    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepositry.GetUserByUsernameAsync(User.GetUserName());

        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if(photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

        if(photo.PublicId != null){
            photoService.DeletePhoto(photo.Url);
        }

        user.Photos.Remove(photo);

        if(await userRepositry.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");

    }

    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<AppUser>> GetUser(int id){
    //     var user = await context.Users.FindAsync(id);

    //     if(user == null) return NotFound();

    //     return user;
    // }
}
