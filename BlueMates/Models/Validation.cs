using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace BlueMates.Models
{
    public partial class Validation
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public DateTime? Time { get; set; }
        public string Pic { get; set; } = null!;
        public int EventId { get; set; }
        public string UserId { get; set; } = null!;
    }
}
