using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikesRepositry likesRepositry) : BaseApiController
{
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId){
        var sourceUserId = User.GetUserId();

        if(sourceUserId == targetUserId) return BadRequest("You connot like yourself");

        var existingLike = await likesRepositry.GetUserLike(sourceUserId, targetUserId);

        if(existingLike == null){
            var like = new UserLike{
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId,
            };

            likesRepositry.AddLike(like);
        }
        else{
            likesRepositry.DeleteLike(existingLike);
        }

        if(await likesRepositry.SaveChanges()) return Ok();

        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds(){
        return Ok(await likesRepositry.GetCurrentUserLikeIds(User.GetUserId()));
    } 

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams){

        likesParams.UserId = User.GetUserId();
        var users = await likesRepositry.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users);
        
        return Ok(users);
    }
}
