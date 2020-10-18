using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class FileUpload
    {
        protected long fileId, masterIdf;
        protected int size;
        protected string name, type, originalName, module, otherDetails, createdBy, updatedBy;
        protected DateTime createdOn, updatedOn;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FileId { get => fileId; set => fileId = value; }
        public long MasterIdf { get => masterIdf; set => masterIdf = value; }
        public string Module { get => module; set => module = value; }
        public string Name { get => name; set => name = value; }
        public int Size { get => size; set => size = value; }
        public string Type { get => type; set => type = value; }
        public string OriginalName { get => originalName; set => originalName = value; }
        public string OtherDetails { get => otherDetails; set => otherDetails = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
    }
}
