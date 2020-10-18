using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class Administrator
    {
        protected long adminId;
        protected string identityUserIdf, firstName, lastName, email, contactNumbers, createdBy, updatedBy;
        protected DateTime? createdOn, updatedOn;
        protected bool isActive, isDeleted;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AdminId { get => adminId; set => adminId = value; }
        public string IdentityUserIdf { get => identityUserIdf; set => identityUserIdf = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string ContactNumbers { get => contactNumbers; set => contactNumbers = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
    }
}
