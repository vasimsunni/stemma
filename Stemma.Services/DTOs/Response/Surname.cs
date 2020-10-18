using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Services.DTOs.Response
{
    public class Surname
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long NoOfPeople { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
