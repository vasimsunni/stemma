using System;

namespace Stemma.Core
{
    public class Surname
    {
        private long surnameId;
        private string title, createdBy, updatedBy;
        private DateTime? createdOn, updatedOn;
        private bool isDeleted;

        public long SurnameId { get => surnameId; set => surnameId = value; }
        public string Title { get => title; set => title = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
