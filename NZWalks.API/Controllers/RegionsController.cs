using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //GET ALL REGIONS ACTION
        //GET: https://localhost:7118/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //get data from database - domain models
            var regionsDomain = dbContext.Regions.ToList();

            //map domain models into dto
            var regionsDto = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain) 
            {

                regionsDto.Add(new RegionDTO()
                {

                    Id = regionDomain.Id,
                    Code= regionDomain.Code,
                    Name= regionDomain.Name,
                    RegionIamgeURL= regionDomain.RegionIamgeURL


                });
            
            }
            //return dto

            return Ok(regionsDto);
        }


        //GET SINGLE REGION
        //GET: https://localhost:7118/api/regions/{id}
        [HttpGet]
        [Route("{id}:Guid") ]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //get region domain model
            var regionDomain = dbContext.Regions.Find(id);

            if (regionDomain == null) 
            { 
            
                return NotFound();

            }

            //map domain model into dto
            var regionDto =new RegionDTO
            {
                
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionIamgeURL = regionDomain.RegionIamgeURL

                };
            //return dto
            return Ok(regionDto);
        }


        //CREATE REGIONS
        //POST: https://localhost:7118/api/regions

        [HttpPost]

        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {

            //Map DTO to Domain Model
            var regionDomain = new Region
            {

                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionIamgeURL = addRegionRequestDto.RegionIamgeURL

            };

            //Use Domain Model to create Region

            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();

            //Map Domain Model back to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionIamgeURL = regionDomain.RegionIamgeURL
            };

            return CreatedAtAction(nameof(GetById), new {id= regionDomain.Id }, regionDto);

        }

        }


    //UPDATE REGION
    //PUT: https://localhost:7118/api/regions/{id}

    [HttpPut]
    [Route("{id}:Guid")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        //check if region exists
        var regionDomain = dbContext.Regions.FirstorDeafult(x => x.Id == id);

        if (regionDomain == null)
        {
            return NotFound();
        }
    }
    
    }


