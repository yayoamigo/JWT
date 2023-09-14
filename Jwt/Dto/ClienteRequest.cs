using System.ComponentModel.DataAnnotations;

namespace Jwt.Dto
{
    public class ClienteRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
