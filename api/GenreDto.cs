using System.ComponentModel.DataAnnotations;

namespace MyFirstProject.Controllers.Api
{
    public class GenreDto
    {
        public byte Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}