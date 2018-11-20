using System.Linq;
using System.Threading.Tasks;
using Demo.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValuesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(IPagedList<Value>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 20)
        {
            var values = await _unitOfWork.GetRepository<Value>().GetPagedListAsync(null, null, null, pageIndex: 0, pageSize: 20);
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Value), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _unitOfWork.GetRepository<Value>()
                                        .GetFirstOrDefaultAsync(v => v, v => v.Id == id, x => x.OrderBy(v => v.Id));
            if (value == null)
                return NotFound();
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Value), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(Value value)
        {
            value.Id = 0;
            await _unitOfWork.GetRepository<Value>().InsertAsync(value);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
        }

        // PUT api/values/5
        [HttpPut()]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Value), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Value value)
        {
            _unitOfWork.GetRepository<Value>().Update(value);
            await _unitOfWork.SaveChangesAsync();
            return Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            _unitOfWork.GetRepository<Value>().Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
