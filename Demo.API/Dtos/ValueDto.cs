using System.ComponentModel.DataAnnotations;

namespace Demo.API.Dtos
{
    public class ValueDto
    {
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [EmailAddress] 
        public string Email { get; set; }
    }
}