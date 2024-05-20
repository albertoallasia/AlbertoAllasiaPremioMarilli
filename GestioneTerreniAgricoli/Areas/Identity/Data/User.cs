using Microsoft.AspNetCore.Identity;

namespace GestioneTerreniAgricoli.Areas.Identity.Data
{
    public class User : IdentityUser
    {
        public string UserType { get; set; }

    }
}
