﻿using System;
using System.Collections.Generic;

namespace Common.Models.Models
{
    public partial class TblLogin
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? Status { get; set; }
    }
}
