﻿using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkRequestDTO,Walk>().ReverseMap();
            CreateMap<Walk,WalkDTO>().ReverseMap();
            CreateMap<Difficulty,DifficultyDTO>().ReverseMap();
            CreateMap<Walk,UpdateWalkRequestDTO>().ReverseMap();
        }
    }
}
