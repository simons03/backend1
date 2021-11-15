﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebbApi.Models.authentication
{
    public class SignUpModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Admin { get; set; } = false;
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
    }
}
