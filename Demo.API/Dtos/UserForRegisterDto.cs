using System.ComponentModel.DataAnnotations;

namespace Demo.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required] public string Username { get; set; }
        [Required, StringLength(8, MinimumLength= 4, ErrorMessage="You must specify password between 4 to 8 char")] public string Password { get; set; }
    }
}