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
                var Hotels =await _unitOfWork.Hotels.GetAll(null,null,new List<string> { "Country"});
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
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var Hotel =await _unitOfWork.Hotels.Get(h => h.Id == id, new List<string> { "Country" });
                var result = _mapper.Map<HotelDTO>(Hotel);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong with {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error, please try again later.");
            }
        }


    }
}
