using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Employee;

namespace Talabat.Core.Specifications.EmployeeSpecs
{
    public class EmployeeWithDepartmentSpecifications : BaseSpecifications<Employee>
    {
        public EmployeeWithDepartmentSpecifications() 
        {
            Includes.Add(E => E.Department);
        }
        public EmployeeWithDepartmentSpecifications(int id):base(E => E.Id == id)
        {
            Includes.Add(E => E.Department);

        }
    }
}
