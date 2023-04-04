using System;
using System.Collections.Generic;

namespace BlueMates.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int OrganizerId { get; set; }
        public DateTime? StartDate { get; set; }
        public string? Location { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Pic { get; set; }

        public virtual UsersToEvent? UsersToEvent { get; set; }
    }
}
