using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Homely_modified_api.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Phone { get; set; } = string.Empty;
        [MaxLength(50)]
        [EmailAddress] // Validate email format
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8)] // Enforce minimum password length
        public string PasswordHash { get; set; } // Store hashed password

        public int Plan { get; set; }
        public int NumberOfAdds { get; set; }

        public void SetPassword(string password)
        {
            // Hash the password using a strong algorithm (e.g., bcrypt)
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                PasswordHash = Convert.ToBase64String(hashedBytes);
            }
        }

        public bool ValidatePassword(string password)
        {
            // Hash the provided password and compare it with the stored hash
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                Console.WriteLine($"entered pass : {password}");
                Console.WriteLine($"hashed : {Convert.ToBase64String(hashedBytes)}");
                Console.WriteLine($"real hashed : {PasswordHash}");
                return Convert.ToBase64String(hashedBytes) == PasswordHash;
            }
        }
    }
}