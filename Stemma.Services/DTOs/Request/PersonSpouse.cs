using System;

namespace Stemma.Services.DTOs.Request
{
    public class PersonSpouse
    {
        public long Id { get; set; }
        public long PersonIdF { get; set; }
        public long SpousePersonIDF { get; set; }
        public long SpouseRelationIDF { get; set; }
        public DateTime? RelationDate { get; set; }
    }
}
