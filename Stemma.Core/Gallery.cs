using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class Gallery
    {
        private long galleryId, fileUploadId, galleryTypeIDF;
        private string itemName, itemType, createdBy, updatedBy;
        private DateTime? itemDate, createdOn, updatedOn;
        private bool isDeleted;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long GalleryId { get => galleryId; set => galleryId = value; }
        public long FileUploadId { get => fileUploadId; set => fileUploadId = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public DateTime? ItemDate { get => itemDate; set => itemDate = value; }
        public string ItemType { get => itemType; set => itemType = value; }
        public long GalleryTypeIDF { get => galleryTypeIDF; set => galleryTypeIDF = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
