using AutoMapper;
using LipaNaMpesaAPI.Dto;
using LipaNaMpesaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LipaNaMpesaAPI.MapperProfile
{
    public class ApiAutomapper :Profile
    {
        public ApiAutomapper()
        {
            CreateMap<Setting, GetConfigDto>().ReverseMap();
        }
    }
}
