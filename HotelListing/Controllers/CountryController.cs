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
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var results = _mapper.Map<List<CountryDTO>>(await _unitOfWork.Countries.GetAll());
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong with {nameof(GetCountries)}");
                return StatusCode(500, "Internal server error, please try again later."); // 
            }
        }
        [HttpGet("{id:int}",Name ="GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(c => c.Id == id, new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDTO>(country);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong with {nameof(GetCountry)}");
                return StatusCode(500, "Internal server error, please try again later."); // 
            }
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid post attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var countryToInserted = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(countryToInserted);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetCountry", new { id = countryToInserted.Id }, countryToInserted);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went wrong with {nameof(CreateCountry)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {

                _logger.LogError($"Invalid update attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {

                var countryToBeUpdated = await _unitOfWork.Countries.Get(h => h.Id == id);
                if (countryToBeUpdated == null)
                {
                    _logger.LogError($"Invalid update attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalid.");
                }

                _mapper.Map(countryDTO, countryToBeUpdated);
                _unitOfWork.Countries.Update(countryToBeUpdated);
                await _unitOfWork.Save();
                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went wrong with {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {

            try
            {
                var countryToBeDeleted = await _unitOfWork.Country.Get(h => h.Id == id);
                if (countryToBeDeleted == null)
                {
                    _logger.LogError($"Invalid delete attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submited data is invalid.");
                }
                await _unitOfWork.Country.Delete(id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong with {nameof(CreateCountry)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }

    }
}
