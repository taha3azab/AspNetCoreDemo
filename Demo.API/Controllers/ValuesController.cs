using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.API.Data;
using Demo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ValuesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET api/values
        [HttpGet]
        [Produces(typeof(IEnumerable<Value>))]
        public async Task<IActionResult> Get()
        {
            var values = await _dataContext.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _dataContext.Values.FirstOrDefaultAsync(v => v.Id == id);
            if (value == null)
                return NotFound();
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Value value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _dataContext.AddAsync(value);
            await _dataContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Value value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var obj = await _dataContext.Values.FirstOrDefaultAsync(v => v.Id == value.Id);
            if (obj == null)
                return NotFound(value);

            obj.Name = value.Name;
            await _dataContext.SaveChangesAsync();
            return Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var value = await _dataContext.Values.FirstOrDefaultAsync(v => v.Id == id);
            if (value != null)
            {
                _dataContext.Values.Remove(value);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
