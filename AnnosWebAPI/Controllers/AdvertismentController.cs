using AnnosWebAPI.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AnnosWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class AdvertismentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _autoMapper;
        public AdvertismentController(AppDbContext dbContext, IMapper autoMapper)
        {
            _dbContext = dbContext;
            _autoMapper = autoMapper;
        }

        // CREATE NEW ADVERTISMENT////////////////////////////////////////////////////////
        /// <summary>
        /// Create and add an advertisment to the database
        /// </summary>
        /// <param 
        /// name="advert">Below is the format needed for the request body
        /// </param>
        /// <returns>
        /// List of ALL advertisments
        /// </returns>
        /// <remarks>
        /// end point example: POST /api/Advert
        /// </remarks>
        /// <response code="201">
        /// Your advertisment was created successfully
        /// </response>
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<Advertisment>>> Post([FromBody] AdvertismentVM advertismentVM)
        {

            var advertisment = new Advertisment
            {
                Id = advertismentVM.Id,
                Name = advertismentVM.Name,
                Description = advertismentVM.Description,
                Price = advertismentVM.Price,
                DateAdded = advertismentVM.DateAdded,
            };

            _dbContext.Advertisments.Add(advertisment);
            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.Advertisments.ToListAsync());
        }


        // READ ALL ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve ALL advertisments from the database
        /// </summary>
        /// <returns>
        /// A full list of ALL adverts
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/Advert
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a full list of ALL adverts
        /// </response>
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<List<AdvertismentVM>>> GetAll()
        {
            var adverts = _dbContext.Advertisments
                .Select(a => new AdvertismentVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    DateAdded = a.DateAdded,
                });

            return Ok(adverts);

        }


        // READ ONE ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve a SPECIFIC advert from the database
        /// </summary>
        /// <param name="id">
        /// Id of specific advert
        /// </param>
        /// <returns>
        /// The chosen advert (by Id)
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/Advert/1
        /// </remarks>
        /// <response code="200">
        /// Successfully returned the chosen advert (by Id)
        /// </response>
        [HttpGet("GetOne/{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AdvertismentVM>> GetOne(int id)
        {
            var advert = _dbContext.Advertisments
                .Where(a => a.Id == id)
                .Select(a => new AdvertismentVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    DateAdded = a.DateAdded,
                });

            if (advert == null)
            {
                return BadRequest("Advertisment not found");
            }
            return Ok(advert);
        }


        // UPDATE ////////////////////////////////////////////////////////
        /// <summary>
        /// (Admin only) Update ALL properties of 1 advert in the database
        /// </summary>
        /// <param name="request">
        /// An instance of the Advert class
        /// </param>
        /// <returns>
        /// A full list of ALL adverts
        /// </returns>
        /// <remarks>
        /// Example end point: PUT /api/Advert
        /// </remarks>
        /// <response code="200">
        /// Your advert was updated successfully
        /// </response>
        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<AdvertismentVM>>> Put([FromBody] AdvertismentVM request)
        {
            var dbAdvert = await _dbContext.Advertisments.FindAsync(request.Id);

            if (dbAdvert == null)
            {
                return BadRequest("Advertisment not found");
            }

            dbAdvert.Name = request.Name;
            dbAdvert.Description = request.Description;
            dbAdvert.Price = request.Price;
            dbAdvert.DateAdded = request.DateAdded;

            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.Advertisments.ToListAsync());
        }


        // UPDATE (PATCH) ////////////////////////////////////////////////
        // Nuget
        // Microsoft.AspNetCore.JsonPatch
        // Microsoft.AspNetCore.MVC.NewtonsoftJson

        /// <summary>
        /// (Admin only) Partially update a SPECIFIC advert from the database
        /// </summary>
        /// <param name="id">
        /// Id of specific advert
        /// </param>
        /// <param name="request">
        /// An instance of the Advert class
        /// </param>
        /// <returns>
        /// The chosen partially updated advert (by Id)
        /// </returns>
        /// <remarks>
        /// Example end point: PATCH /api/Advert/1
        /// </remarks>
        /// <response code="200">
        /// Advert was partially updated successfully (by Id)
        /// </response>
        [HttpPatch("Patch/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Patch([FromBody] JsonPatchDocument request, int id)
        {
            var dbAdvert = await _dbContext.Advertisments.FindAsync(id);

            if (dbAdvert == null)
            {
                return BadRequest("Advertisment not found");
            }

            request.ApplyTo(dbAdvert);
            await _dbContext.SaveChangesAsync();

            return Ok(dbAdvert);
        }


        // DELETE ////////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve advert from the database for deletion
        /// </summary>
        /// <param name="id">
        /// Id of advertisment
        /// </param>
        /// <returns>
        /// The entire list of ALL adverts
        /// </returns>
        /// <remarks>
        /// Example end point: DELETE /api/Advert/1
        /// </remarks>
        /// <response code="200">
        /// Successfully deleted the chosen advertisment (by Id)
        /// </response>
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<AdvertismentVM>>> Delete(int id)
        {
            var dbAdvert = await _dbContext.Advertisments.FindAsync(id);

            if (dbAdvert == null)
            {
                return BadRequest("Advertisment not found");
            }

            _dbContext.Advertisments.Remove(dbAdvert);
            await _dbContext.SaveChangesAsync();

            return Ok(await _dbContext.Advertisments.ToListAsync());
        }
    }
}

