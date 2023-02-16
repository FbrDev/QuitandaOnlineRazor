using Microsoft.AspNetCore.Identity;

namespace QuitandaOnline.Models
{
    public class AppUser : IdentityUser
    {
        public string Nome { get; set; }
    }
}
