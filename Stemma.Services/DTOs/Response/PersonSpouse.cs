using System;

namespace Stemma.Services.DTOs.Response
{
    public class PersonSpouse
    {
        public long Id { get; set; }
        public long PersonIdF { get; set; }
        public long SpousePersonIDF { get; set; }
        public long SpouseRelationIDF { get; set; }
        public DateTime? RelationDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string SpouseRelation { get; set; }
        public Person Spouse { get; set; }
    }
}
