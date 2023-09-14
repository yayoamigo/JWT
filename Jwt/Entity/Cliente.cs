using System.ComponentModel.DataAnnotations;


namespace Jwt.Entity

{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordSalt { get; set; }
    }
}
