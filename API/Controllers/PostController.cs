using System.Linq.Expressions;
using API.Abstractions.DTOs;
using API.Abstractions.Helpers;
using API.Abstractions.Result;
using Application.Abstractions.Posts.CreatePostCommand;
using AutoMapper;
using MediatR;
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
        
    }
}
