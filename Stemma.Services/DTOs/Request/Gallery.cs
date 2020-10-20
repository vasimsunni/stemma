using System;

namespace Stemma.Services.DTOs.Request
{
    public class Gallery
    {
        public long Id { get; set; }
        public long FileUploadId { get; set; }
        public string ItemName { get; set; }
        public DateTime? ItemDate { get; set; }
        public string ItemType { get; set; }
        public long GalleryTypeIDF { get; set; }
    }
}
