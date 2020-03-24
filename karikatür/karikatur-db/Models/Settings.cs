using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Settings
    {
        public Guid Id { get; set; }
        public string ProjectKey { get; set; }
        public int VersionNumber { get; set; }
        public bool UpdateRequired { get; set; }
    }
}
