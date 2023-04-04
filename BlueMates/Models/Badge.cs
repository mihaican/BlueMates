using System;
using System.Collections.Generic;

namespace BlueMates.Models
{
    public partial class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public int Price { get; set; }

        public virtual BadgesToUser? BadgesToUser { get; set; }
    }
}
