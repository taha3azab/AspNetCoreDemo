using System.ComponentModel.DataAnnotations;

namespace Demo.API.Models
{
    public class Value
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [EmailAddress] public string Email { get; set; }
    }
}