﻿using AutoMapper;
using HotelListing.DTOs;
using HotelListing.IRepository;
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
        [HttpGet("{id:int}")]
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
        
    }
}
