﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET ALL REGIONS ACTION
        //GET: https://localhost:7118/api/regions
        [HttpGet]
        //[Authorize(Roles ="Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll Regions Action Method was invoked");

            //get data from database - domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            //map domain models into dto
            // var regionsDto= mapper.Map < List<RegionDTO>>(regionsDomain);

            //return dto
            logger.LogInformation($"Finished GetAll Regions requesht with data: {JsonSerializer.Serialize(regionsDomain)}");
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }


        //GET SINGLE REGION
        //GET: https://localhost:7118/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //get region domain model
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {

                return NotFound();

            }

            //map domain model into dto
            //return dto
            return Ok(mapper.Map<List<RegionDTO>>(regionDomain));
        }


        //CREATE REGIONS
        //POST: https://localhost:7118/api/regions

        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]


        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                //Map DTO to Domain Model
                var regionDomain = mapper.Map<Region>(addRegionRequestDto);

                //Use Domain Model to create Region

                regionDomain = await regionRepository.CreateAsync(regionDomain);

                //Map Domain Model back to DTO
                var regionDto = mapper.Map<RegionDTO>(regionDomain);

                return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);
            

            

        }




        //UPDATE REGION
        //PUT: https://localhost:7118/api/regions/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            
                //map dto to domain model
                var regionDomain = mapper.Map<Region>(updateRegionRequestDto);

                regionDomain = await regionRepository.UpdateAsync(id, regionDomain);

                if (regionDomain == null)
                {
                    return NotFound();
                }

                //Convert Domain Model to DTO
                var regionDto = mapper.Map<RegionDTO>(regionDomain);
                return Ok(regionDto);
           
        }


        //DELETE REGION
        //DELETE:https://localhost:7118/api/regions/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //check if region exists
            var regionDomain = await regionRepository.DeleteAsync(id);
            if (regionDomain == null)
            {
                return NotFound();

            }

            //return deleted region optional
            //map domain model to dto
            var regionDto = mapper.Map<RegionDTO> (regionDomain);

            return Ok(regionDto);
        }
    }
}


