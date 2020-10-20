using System;

namespace Stemma.Services.DTOs.Response
{
    public class GalleryType
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public long NoOfItem { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
