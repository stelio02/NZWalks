using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //CREATE WALK
        //POST: https://localhost:7118/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {

            //Map DTO to Domain Model
            var walkDomain = mapper.Map<Walk>(addWalkRequestDTO);
            await walkRepository.CreateAsync(walkDomain);
            //Map Domain Model to DTO
            return Ok(mapper.Map<WalkDTO>(walkDomain));


        }

        //GET WALKS
        //GET: https://localhost:7118/api/walks?filterOn=Name&filterQuery=
        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var walksDomain = await walkRepository.GetAllAsync(filterOn, filterQuery);
            return Ok(mapper.Map<List<WalkDTO>>(walksDomain));
        }

        //GET SINGLE WALK BY ID
        //GET: https://localhost:7118/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walkDomain));
        }

        //UPDATE WALK
        //PUT: https://localhost:7118/api/walks/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {

            var walkDomain = mapper.Map<Walk>(updateWalkRequestDTO);

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walkDomain));
        }




        //DELETE WALK
        //DELETE: https://localhost:7118/api/walks/{id}

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomain = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(deletedWalkDomain));
        }

    }
}



