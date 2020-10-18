using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stemma.Core
{
    public class Bin
    {
        private long id, entityId;
        private string entity,title, deletedByIdentityId;
        private DateTime deletedOn;


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }
        public long EntityId { get => entityId; set => entityId = value; }
        public string Entity { get => entity; set => entity = value; }
        public string Title { get => title; set => title = value; }
        public string DeletedByIdentityId { get => deletedByIdentityId; set => deletedByIdentityId = value; }
        public DateTime DeletedOn { get => deletedOn; set => deletedOn = value; }
    }
}
