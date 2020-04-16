﻿using JWT_API_NETCORE.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_API_NETCORE.Models
{
    [Table("Department")]
    public class Department : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<DateTimeOffset> CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }


        public Department() { }

        public Department(Department department)
        {
            this.Name = department.Name;
            this.CreateDate = DateTimeOffset.Now;
            this.IsDelete = false;
        }

        public void Update(Department department)
        {
            this.Name = department.Name;
            this.UpdateDate = DateTimeOffset.Now;
        }

        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }




    }
}
