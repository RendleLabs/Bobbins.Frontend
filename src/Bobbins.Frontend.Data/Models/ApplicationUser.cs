using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Bobbins.Frontend.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    [PublicAPI]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(16)]
        public string ScreenName { get; set; }
    }
}
