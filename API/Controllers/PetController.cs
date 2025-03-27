using API.Abstractions.DTOs;
using API.Abstractions.Helpers;
using API.Abstractions.Result;
using Application.Abstractions.Pets.CreatePetCommand;
using Application.Abstractions.Pets.GetTypeQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : PetGramController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PetController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the type of a pet. Useful for when creating a pet in the form.
        /// </summary>
        /// <param name="mediaFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-type")]
        public async Task<IActionResult> GetPetType([FromForm] GetTypeQuery mediaFile){
            return Ok(Result<Dictionary<string,string>>.Success(await _mediator.Send(mediaFile)));
        }

        /// <summary>
        /// Creates the pet 
        /// </summary>
        /// <param name="createPetCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-pet")]
        public async Task<IActionResult> CreatePet([FromBody] CreatePetCommand createPetCommand){
            var result = await _mediator.Send(createPetCommand);
            return Ok(Result<PetDto>.Success(_mapper.Map<PetDto>(result)));
        }
    }
}