using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Core
{
   public class Surname
    {
        private long id;
        private string surnameText,createdBy,updatedBy;
        private DateTime? createdOn,updatedOn;
        private bool isDeleted;

        public long Id { get => id; set => id = value; }
        public string SurnameText { get => surnameText; set => this.surnameText = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
