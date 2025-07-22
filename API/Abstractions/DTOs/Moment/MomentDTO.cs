using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Abstractions.DTOs.Media;
using API.Abstractions.DTOs.User;
using Domain.Models.UserAggregate;

namespace API.Abstractions.DTOs.Moment
{
    public class MomentDto
    {
        public string? Content { get; set; }
        public MediaDTO Media { get; set; }
        public MiniUserDto User { get; set; }
        

        public MomentDto() { }
        public MomentDto Map (Domain.Models.Moment moment)
        {
            var user = moment.Author;
            return new MomentDto
            {
                Content = moment.Content,
                Media = new MediaDTO(moment.Media.Id, moment.Media.Url, moment.Media.Type, moment.Media.MentionedPets.Select(x => new ReducedPetDto(x.Id, x.Name, x.ImgUrl, x.Species)).ToList()),
                User = new MiniUserDto(user!.ProfileImgUrl, user.Name)
            };
        }
    }
}