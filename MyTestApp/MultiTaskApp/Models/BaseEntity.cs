using System.ComponentModel.DataAnnotations;

namespace MultiTaskApp.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}