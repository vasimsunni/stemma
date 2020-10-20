using System;

namespace Stemma.Services.DTOs.Response
{
    public class Gallery
    {
        public long Id { get; set; }
        public long FileUploadId { get; set; }
        public string ItemName { get; set; }
        public DateTime? ItemDate { get; set; }
        public string ItemType { get; set; }
        public long GalleryTypeIDF { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public long NoOfPeople { get; set; }
        public string GalleryType { get; set; }

    }
}
