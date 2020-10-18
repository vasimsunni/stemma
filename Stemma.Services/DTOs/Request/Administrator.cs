using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Services.DTOs.Request
{
    public class Administrator
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ContactNumbers { get; set; }
        public string[] Roles { get; set; }
        public long UploadedFileId { get; set; }
    }
}
