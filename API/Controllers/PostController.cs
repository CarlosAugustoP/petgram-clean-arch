using System.Linq.Expressions;
using API.Abstractions.DTOs;
using API.Abstractions.Helpers;
using API.Abstractions.Result;
using Application.Abstractions.Likes.LikePostCommand;
using Application.Abstractions.Posts.CreatePostCommand;
using Application.Abstractions.Posts.GetPostByIdQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : PetGramController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a Post based on a user's request
        /// </summary>
        /// <returns>
        /// The Created Post
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostCommand command){
            var req = new CreatePostCommand(command.Title, command.MediaFiles, command.Content);
            req.SetUserId(CurrentUser.Id);
            var result = await _mediator.Send(req);
            var postDto = new PostDto().Map(result);
            return Created("api/Post", Result<PostDto>.Success(postDto));
        }

        /// <summary>
        /// Gets a Post by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPostById(Guid id){
            var result = await _mediator.Send(new GetPostByIdQuery(id));
            var postDto = new PostDto().Map(result);
            return Ok(Result<PostDto>.Success(postDto));
        }

        [HttpPost]
        [Authorize]
        [Route("like/{postId}")]
        public async Task<IActionResult> LikePostById(Guid postId){
            var command = new LikePostCommand {
                UserId = CurrentUser.Id,
                PostId = postId
            };
            var result = await _mediator.Send(command);
            var postDto = new PostDto().Map(result);
            return Ok(Result<PostDto>.Success(postDto));
        }
        
    }
}
