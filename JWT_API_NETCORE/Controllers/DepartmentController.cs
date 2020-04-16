using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT_API_NETCORE.Base;
using JWT_API_NETCORE.Models;
using JWT_API_NETCORE.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_API_NETCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BasesController<Department, DepartmentRepository>
    {

        private readonly DepartmentRepository _repository;
        public DepartmentController(DepartmentRepository departmentRepository) : base(departmentRepository)
        {
            this._repository = departmentRepository;
            
        }


        [HttpGet]
        public async Task<ActionResult<Department>> Get()
        {
            var get = await _repository.Get();
            return Ok(new { data = get });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> Get(int id)
        {
            var get = await _repository.Get(id);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }


        [HttpPut("{Id}")]
        public async Task<ActionResult<Department>> Put(int Id, Department model)
        {
        
            var update = await _repository.Get(Id);
            if(update == null)
            {
                return NotFound();
            }

            update.Name = model.Name;
            update.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(update);
            return Ok("Update Successfully");
        }
    }
}