using Dapper;
using JWT_API_NETCORE.Context;
using JWT_API_NETCORE.Models;
using JWT_API_NETCORE.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_API_NETCORE.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, MyContext>
    {

        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }

        private readonly MyContext _myContext;


        public EmployeeRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
            _myContext = myContext;
        }

        public async Task<Employee> Get(string email)
        {
            return await _myContext.Set<Employee>().FindAsync(email);

            //throw new NotImplementedException();
        }


        public async Task<IEnumerable<EmployeeVM>> GetAll()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_ViewEmployee";

                var data = await connection.QueryAsync<EmployeeVM>(spName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<EmployeeVM>> GetByEmail(string Email)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_GetEmployeeById";
                parameters.Add("@email", Email);
                var data = await connection.QueryAsync<EmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }


        public async Task<IEnumerable<RegisterViewModel>> Create(RegisterViewModel employee)
        {

            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_InsertEmployee";
                parameters.Add("@firstname", employee.FirstName);
                parameters.Add("@lastname", employee.LastName);
                parameters.Add("@email", employee.Email);
                parameters.Add("@phonenumber", employee.PhoneNumber);
                parameters.Add("@birthdate", employee.BirthDate);
                parameters.Add("@address", employee.Address);
                parameters.Add("@department_id", employee.Department_Id);

                var data = await connection.QueryAsync<RegisterViewModel>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
                
        }


        public async Task<Employee> Delete(string email)
        {

            var entity = await Get(email);
            if (entity == null)
            {
                return entity;
            }
            entity.DeleteDate = DateTimeOffset.Now;
            entity.IsDelete = true;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;

            //throw new NotImplementedException();
        }



    }

}
