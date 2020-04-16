using JWT_API_NETCORE.Context;
using JWT_API_NETCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_API_NETCORE.Repository.Data
{
    public class DepartmentRepository : GeneralRepository<Department, MyContext>
    {
        public DepartmentRepository(MyContext myContext) : base(myContext)
        {

        } 
    }
}
