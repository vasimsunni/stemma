using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Services.DTOs.Response
{
    public class SpouseRelation
    {
        public long Id { get; set; }
        public string Relation { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
