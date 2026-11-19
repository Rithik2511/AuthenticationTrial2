using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Employee
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        List<Employee> Employees { get; set; }
    }
}
