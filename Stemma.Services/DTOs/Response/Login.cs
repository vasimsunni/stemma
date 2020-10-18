using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Services.DTOs.Response
{
    public class Login
    {
        public string Token { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}
