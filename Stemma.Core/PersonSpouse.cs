using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class PersonSpouse
    {
        private long id, personIdF, spousePersonIDF, spouseRelationIDF;
        private string createdBy, updatedBy;
        private DateTime? relationDate, createdOn, updatedOn;
        private bool isDeleted;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }
        public long PersonIdF { get => personIdF; set => personIdF = value; }
        public long SpousePersonIDF { get => spousePersonIDF; set => spousePersonIDF = value; }
        public long SpouseRelationIDF { get => spouseRelationIDF; set => spouseRelationIDF = value; }
        public DateTime? RelationDate { get => relationDate; set => relationDate = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
