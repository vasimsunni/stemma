﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class GalleryType
    {
        private long galleryTypeId;
        private string type, createdBy, updatedBy;
        private DateTime? createdOn, updatedOn;
        private bool isDeleted;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long GalleryTypeId { get => galleryTypeId; set => galleryTypeId = value; }
        public string Type { get => type; set => type = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
