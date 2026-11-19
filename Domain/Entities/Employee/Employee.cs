using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Employee
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Department? Department { get; set; }

    }
}
