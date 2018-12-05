using System.Linq;
using System.Threading.Tasks;
using Demo.API.Dtos;
using Demo.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Demo.API.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
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
        [ProducesResponseType(typeof(PagedListDto<ValueDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 20)
        {
            var values = await _unitOfWork.GetRepository<Value>()
                                .GetPagedListAsync(null, q => q.OrderBy(v => v.Id), null, pageIndex, pageSize);
            return Ok(values?.Adapt<PagedListDto<ValueDto>>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValueDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _unitOfWork.GetRepository<Value>()
                                            .GetFirstOrDefaultAsync(
                                                v => v.Adapt<ValueDto>(),
                                                v => v.Id == id,
                                                x => x.OrderBy(v => v.Id));
            if (value == null)
                return NotFound();
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValueDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(ValueDto value)
        {
            value.Id = 0;
            var dbValue = value?.Adapt<Value>();
            await _unitOfWork.GetRepository<Value>().InsertAsync(dbValue);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dbValue.Id }, dbValue?.Adapt<ValueDto>());
        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValueDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(ValueDto value)
        {
            var dbValue = value?.Adapt<Value>();
            _unitOfWork.GetRepository<Value>().Update(dbValue);
            await _unitOfWork.SaveChangesAsync();
            return Ok(dbValue.Adapt<ValueDto>());
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
