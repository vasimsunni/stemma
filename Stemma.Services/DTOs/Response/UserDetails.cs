using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Services.DTOs.Response
{
    public class UserDetails
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumbers { get; set; }
        public string ProfilePictureURL { get; set; }
        public string IdentityUserId { get; set; }
        public long UserId { get; set; }
        public string[] Roles { get; set; }
    }
}
