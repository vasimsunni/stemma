using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Services.DTOs.Response
{
    public class GalleryPerson
    {
        public long Id { get; set; }
        public long GalleryIDF { get; set; }
        public long PersonIDF { get; set; }
        public bool IsProfilePicture { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public Person person { get; set; }
    }
}
