using System.Linq.Expressions;
using API.Abstractions.DTOs;
using API.Abstractions.DTOs.Likes;
using API.Abstractions.DTOs.Comments;

using API.Abstractions.Helpers;
using API.Abstractions.Requests;
using API.Abstractions.Result;
using Application.Abstractions.Comments;
using Application.Abstractions.Likes.GetLikesByPostQuery;
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
        /// <summary>
        /// Likes a Post by its Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets all the likes for a Post by its Id
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("view-likes/{postId}")]
        public async Task<IActionResult> GetLikesByPostId([FromRoute] Guid postId, [FromQuery] PageRequest query){
            var result = await _mediator.Send(new GetLikesByPostQuery(postId, query.PageIndex, query.PageSize));
            var likesDto = result.Items.Select(l => new LikeDto().Map(l)).ToList();
            return Ok(Result<List<LikeDto>>.Success(likesDto));
        }

        [HttpPost]
        [Route("comment")] 
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand command){
            command.SetUserId(CurrentUser.Id);
            var result = await _mediator.Send(command);
            var commentDto = new CommentDto().Map(result);
            return Created("api/Post", Result<CommentDto>.Success(commentDto));
        }
        
    }
}
