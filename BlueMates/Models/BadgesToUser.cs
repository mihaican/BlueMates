using System;
using System.Collections.Generic;
using BlueMates.Models;

namespace BlueMates.Models
{
    public partial class BadgesToUser
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int BadgeId { get; set; }

        public virtual Badge Badge { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
