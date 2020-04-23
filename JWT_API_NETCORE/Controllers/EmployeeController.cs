using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT_API_NETCORE.Base;
using JWT_API_NETCORE.Models;
using JWT_API_NETCORE.Repository.Data;
using JWT_API_NETCORE.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_API_NETCORE.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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

        [HttpGet("{Email}")]
        public async Task<ActionResult<EmployeeVM>> Get(string Email)
        {
            var get = await _repository.GetByEmail(Email);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> Post(Employee employee)
        {
            await _repository.Post(employee);
            return CreatedAtAction("Get", new { Email = employee.Email }, employee);
        }




        [HttpPut("{email}")]
        public async Task<ActionResult<Employee>> Put(string email, Employee model)
        {

            var update = await _repository.GetEmail(email);
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

        [HttpDelete("{email}")]
        public async Task<ActionResult> Delete(string email)
        {
            var delete = await _repository.Delete(email);
            if (delete == null)
            {
                return NotFound();
            }
            return Ok(delete);
        }

        [HttpGet]
        [Route("ChartInfo")]
        [HttpGet]
        public async Task<IEnumerable<ChartViewModel>> Chart()
        {
            return await _repository.GetChart();
        }
    }
}