using System.Linq.Expressions;
using API.Abstractions.DTOs;
using API.Abstractions.DTOs.Likes;
using API.Abstractions.DTOs.Comments;

using API.Abstractions.Helpers;
using API.Abstractions.Requests;
using API.Abstractions.Result;
using Application.Abstractions.Likes.GetLikesByPostQuery;
using Application.Abstractions.Likes.LikePostCommand;
using Application.Abstractions.Posts.CreatePostCommand;
using Application.Abstractions.Posts.GetPostByIdQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Abstractions.Comments.CreateCommentCommand;
using Application.Abstractions.Comments.GetCommentsFromPostQuery;
using Application.Abstractions.Feed;
using Domain.Models;
using Application.Abstractions.Posts.GetByUserQuery;
using SharedKernel.Common;

namespace API.Controllers
{
    [ApiController]
    [Route("api/posts")]
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
        [Authorize]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostCommand command)
        {
            var req = new CreatePostCommand(command.Title, command.Medias, command.Content);
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
        public async Task<IActionResult> GetPostById(Guid id)
        {
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
        public async Task<IActionResult> LikePostById(Guid postId)
        {
            var command = new LikePostCommand
            {
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
        public async Task<IActionResult> GetLikesByPostId([FromRoute] Guid postId, [FromQuery] PageRequest query)
        {
            var result = await _mediator.Send(new GetLikesByPostQuery(postId, query.PageIndex, query.PageSize));
            var likesDto = result.Select(l => new LikeDto().Map(l));
            return Ok(Result<PaginatedList<LikeDto>>.Success(likesDto));
        }

        /// <summary>
        /// Creates a comment on a Post
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("comments")]
        public async Task<IActionResult> CreatePostComment([FromBody] CreateCommentCommand command)
        {
            command.SetUserId(CurrentUser.Id);
            var result = await _mediator.Send(command);
            var commentDto = new CommentDto().Map(result);
            return Created("api/Post", Result<CommentDto>.Success(commentDto));
        }
        /// <summary>
        /// Gets all the comments for a Post
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("comments/{postId}")]
        public async Task<IActionResult> GetPostComments([FromQuery] PageRequest pageRequest, [FromRoute] Guid postId)
        {
            var result = await _mediator.Send(new GetCommentsFromPostQuery(postId, pageRequest.PageIndex, pageRequest.PageSize));
            var commentsDto = result.Select(c => new CommentDto().Map(c));
            return Ok(Result<PaginatedList<CommentDto>>.Success(commentsDto));
        }

        /// <summary>
        /// Returns the feed 
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("feed")]
        public async Task<IActionResult> GetFeed([FromQuery] PageRequest pageRequest)
        {
            var r = await _mediator.Send(new GetFeedQuery(
                CurrentUser.Id, pageRequest.PageIndex, pageRequest.PageSize
            ));
            return Ok(Result<PaginatedList<PostDto>>.Success(r.Select(x => new PostDto().Map(x))));
        }

        /// <summary>
        /// Gets the feed for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("by-user/{userId}")]
        public async Task<IActionResult> GetFeedByUserId([FromRoute] Guid userId, [FromQuery] PageRequest pageRequest)
        {
            var r = await _mediator.Send(new GetByUserQuery(
                userId, pageRequest.PageIndex, pageRequest.PageSize
            ));
            return Ok(Result<PaginatedList<PostDto>>.Success(r.Select(x => new PostDto().Map(x))));
        }

        /// <summary>
        /// Gets the feed for the current user
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("by-user")]
        [Authorize]
        public async Task<IActionResult> GetFeedByCurrentUser([FromQuery] PageRequest pageRequest)
        {
            var r = await _mediator.Send(new GetByUserQuery(
                CurrentUser.Id, pageRequest.PageIndex, pageRequest.PageSize
            ));
            return Ok(Result<PaginatedList<PostDto>>.Success(r.Select(x => new PostDto().Map(x))));
        }
    }
}
