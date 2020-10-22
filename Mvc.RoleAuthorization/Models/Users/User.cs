using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.RoleAuthorization.Models.Users
{
    public class ApplicationUser : IdentityUser<int>
    {

        public virtual string FriendlyName
        {
            get
            {
                string friendlyName = string.IsNullOrWhiteSpace(ArFullName) ? UserName : ArFullName;

                if (!string.IsNullOrWhiteSpace(JobTitle))
                    friendlyName = $"{JobTitle} {friendlyName}";

                return friendlyName;
            }
        }

        [MaxLength(70)]
        public string FirstName { get; set; }

        [MaxLength(70)]
        public string LastName { get; set; }

        [MaxLength(150)]
        public string ArFullName { get; set; }

        [MaxLength(150)]
        public string EnFullName { get; set; }


        public string JobTitle { get; set; }
        public DateTime? BirthDay { get; set; }

        public string StoppedReason { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

        public DateTime LastActive { get; set; }

      


        [DefaultValue(typeof(Decimal), "0")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalRating { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string Configuration { get; set; }

        public DateTime LastTokenPhoneDate { get; set; }


    }
}