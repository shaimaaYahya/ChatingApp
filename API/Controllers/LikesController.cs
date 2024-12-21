using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(/*ILikesRepositry likesRepositry*/IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId){
        var sourceUserId = User.GetUserId();

        if(sourceUserId == targetUserId) return BadRequest("You connot like yourself");

        var existingLike = await unitOfWork.LikesRepositry.GetUserLike(sourceUserId, targetUserId);

        if(existingLike == null){
            var like = new UserLike{
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId,
            };

            unitOfWork.LikesRepositry.AddLike(like);
        }
        else{
            unitOfWork.LikesRepositry.DeleteLike(existingLike);
        }

        if(await unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds(){
        return Ok(await unitOfWork.LikesRepositry.GetCurrentUserLikeIds(User.GetUserId()));
    } 

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams){

        likesParams.UserId = User.GetUserId();
        var users = await unitOfWork.LikesRepositry.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users);
        
        return Ok(users);
    }
}
