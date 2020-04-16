using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT_API_NETCORE.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_API_NETCORE.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;

        public BasesController(TRepository repository) { this._repository = repository; }


        //[HttpGet]
        //public async Task<ActionResult<TEntity>> Get()
        //{
        //    var get = await _repository.Get();
        //    return Ok(new { data = get });
        //}


        //[HttpGet("{Id}")]
        //public async Task<ActionResult<TEntity>> Get(int Id)
        //{
        //    var get = await _repository.Get(Id);
        //    if (get == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(get);
        //}


        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            await _repository.Post(entity);
            return CreatedAtAction("Get", new { Id = entity.Id }, entity);
        }


        //[HttpPut("{Id}")]
        //public async Task<ActionResult<TEntity>> Put(int Id, TEntity entity)
        //{
        //    if (Id != entity.Id)
        //    {
        //        return BadRequest();
        //    }
        //    //var put = await _repository.Get(id);
        //    await _repository.Put(entity);
        //    return Ok("Update Successfully");
        //}


        [HttpDelete("{Id}")]
        public async Task<ActionResult<TEntity>> Delete(int id)
        {
            var delete = await _repository.Delete(id);
            if (delete == null)
            {
                return NotFound();
            }
            return delete;
        }










    }
}