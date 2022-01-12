using AutoMapper;
using HotelListing.DTOs;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Hotel> _logger;
        private readonly IMapper _mapper;
        public HotelsController(IUnitOfWork unitOfWork, ILogger<Hotel> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var Hotels =await _unitOfWork.Country.GetAll(null,null,new List<string> { "Country"});
                var results = _mapper.Map<IList<HotelDTO>>(Hotels);
                return Ok(results);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong with {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }
        [Authorize]
        [HttpGet("{id:int}",Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var Hotel =await _unitOfWork.Country.Get(h => h.Id == id, new List<string> { "Country" });
                var result = _mapper.Map<HotelDTO>(Hotel);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong with {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody]CreateHotelDTO hotelDTO )
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid post attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotelToBeInserted = _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWork.Country.Insert(hotelToBeInserted);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetHotel", new { id= hotelToBeInserted.Id},hotelToBeInserted );
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went wrong with {nameof(CreateHotel)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id,[FromBody] CreateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid||id<1)
            {

                _logger.LogError($"Invalid update attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {

                var hotelToBeUpdated = await _unitOfWork.Country.Get(h=>h.Id==id);
                if (hotelToBeUpdated == null)
                {
                    _logger.LogError($"Invalid update attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted data is invalid.");
                }

                _mapper.Map(hotelDTO, hotelToBeUpdated);
                _unitOfWork.Country.Update(hotelToBeUpdated);
                await _unitOfWork.Save();
                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went wrong with {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            
            try
            {
                var hotelToBeDeleted = await _unitOfWork.Country.Get(h => h.Id == id);
                if (hotelToBeDeleted == null)
                {
                    _logger.LogError($"Invalid delete attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submited data is invalid.");
                }
                await _unitOfWork.Country.Delete(id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong with {nameof(CreateHotel)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }


    }
}
