using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using API.Abstractions;
namespace API.Config
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Users, UserDto>();
        }
    }
}
