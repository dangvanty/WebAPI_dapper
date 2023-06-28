﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebAPI_dapper.Data.Interfaces;
using WebAPI_dapper.Data.ViewModel;
using WebAPI_dapper.Extensions;
using WebAPI_dapper.Helpers;

namespace WebAPI_dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeRepository _attributeRepository;

        public AttributeController(IConfiguration configuration, IAttributeRepository attributeRepository)
        {
           
            _attributeRepository = attributeRepository;

        }

        [HttpGet("{id}")]
        public async Task<AttributeViewModel> Get(int id)
        {
            return await _attributeRepository.GetById(id, CultureInfo.CurrentCulture.Name);
        }

        [HttpGet]
        public async Task<List<AttributeViewModel>> GetAll()
        {
            return await _attributeRepository.GetAll(CultureInfo.CurrentCulture.Name);
        }

        [HttpPost]
        [ValidateModel]
        public async Task AddAttribute([FromBody] AttributeViewModel attribute)
        {
            await _attributeRepository.Add(CultureInfo.CurrentCulture.Name, attribute);
        }

        [HttpPut("{id}")]
        [ValidateModel]

        public async Task Update(int id, [FromBody] AttributeViewModel attribute)
        {
            await _attributeRepository.Update(id, CultureInfo.CurrentCulture.Name, attribute);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _attributeRepository.Delete(id);
        }
    }
}
