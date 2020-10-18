using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Core
{
    public class ApplicationUser:IdentityUser
    {
        //Extended Properties		
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string PostalCode { get; set; }
        //public string MobileNo { get; set; }
        //public int GenderTypeId { get; set; }
        //public DateTime? DOB { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
