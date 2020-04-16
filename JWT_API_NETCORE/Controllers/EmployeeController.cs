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
    public class EmployeeController : BasesController<Employee, EmployeeRepository>
    {

        private readonly EmployeeRepository _repository;
        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;

        }


        [HttpGet]
        public async Task<IEnumerable<EmployeeVM>> Get()
        {
            return await _repository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeVM>> Get(int id)
        {
            var get = await _repository.GetById(id);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }




        [HttpPut("{Id}")]
        public async Task<ActionResult<Employee>> Put(int Id, Employee model)
        {

            var update = await _repository.Get(Id);
            if (update == null)
            {
                return NotFound();
            }

            update.UpdateDate = DateTimeOffset.Now;
            update.FirstName = model.FirstName;
            update.LastName = model.LastName;
            update.Email = model.Email;
            update.BirthDate = model.BirthDate;
            update.PhoneNumber = model.PhoneNumber;
            update.Address = model.Address;
            update.IsDelete = model.IsDelete;
            update.Department_Id = model.Department_Id;
            await _repository.Put(update);
            return Ok("Update Successfully");
        }
    }
}