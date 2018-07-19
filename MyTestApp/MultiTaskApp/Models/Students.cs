using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTaskApp.Models
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int Course { get; set; }
        public decimal Stipend { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        
        public int UniversityId { get; set; }
        [ForeignKey("UniversityId")]
        public University University { get; set; }
    }
}