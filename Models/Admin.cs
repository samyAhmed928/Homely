using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Homely_modified_api.Models
{
    public class Admin
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MinLength(8)] // Enforce minimum password length
        public string Password { get; set; } // Store hashed password

        public string Phone { get; set; } = string.Empty;
        [MaxLength(50)]
        [EmailAddress] // Validate email format
        public string Email { get; set; } = string.Empty;
        

        public void SetPassword(string password)
        {
            // Hash the password using a strong algorithm (e.g., bcrypt)
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                Password = Convert.ToBase64String(hashedBytes);
            }
        }

        public bool ValidatePassword(string password)
        {
            // Hash the provided password and compare it with the stored hash
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes) == Password;
            }
        }
    }
}