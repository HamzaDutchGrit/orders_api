using Microsoft.AspNetCore.Identity;

namespace ProductsApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty; // Optioneel
        public bool IsAdmin { get; set; } = false; // Optioneel
    }
}
