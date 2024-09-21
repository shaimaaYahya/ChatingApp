using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepositry userRepositry, IMapper mapper) : BaseApiController
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

    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<AppUser>> GetUser(int id){
    //     var user = await context.Users.FindAsync(id);

    //     if(user == null) return NotFound();

    //     return user;
    // }
}
