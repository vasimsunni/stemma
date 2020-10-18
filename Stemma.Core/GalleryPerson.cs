using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class GalleryPerson
    {
        private long id, galleryIDF, personIDF;
        private string createdBy, updatedBy;
        private DateTime? createdOn, updatedOn;
        private bool isProfilePicture, isDeleted;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }
        public long GalleryIDF { get => galleryIDF; set => galleryIDF = value; }
        public long PersonIDF { get => personIDF; set => personIDF = value; }
        public bool IsProfilePicture { get => isProfilePicture; set => isProfilePicture = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
