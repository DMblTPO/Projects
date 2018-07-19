using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTaskApp.Models
{
    public class University : BaseEntity
    {
        public string Name { get; set; }
        public decimal Rating { get; set; }
        public string City { get; set; }

        [ForeignKey("UniversityId")]
        public ICollection<Student> Students { get; set; }
    }
}