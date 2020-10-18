using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class SpouseRelation
    {
        private long id;
        private string relation, createdBy, updatedBy;
        private DateTime? createdOn, updatedOn;
        private bool isDeleted;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }
        public string Relation { get => relation; set => relation = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
