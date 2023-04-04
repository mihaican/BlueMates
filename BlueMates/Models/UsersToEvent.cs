using System;
using System.Collections.Generic;

namespace BlueMates.Models
{
    public partial class UsersToEvent
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int EventId { get; set; }
        public int InterestLevel { get; set; }
        public bool Validated { get; set; }

        public virtual Event IdNavigation { get; set; } = null!;
        public virtual AspNetUser? AspNetUser { get; set; }
    }
}
