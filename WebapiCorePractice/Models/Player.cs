using System;
using System.Collections.Generic;

namespace WebapiCorePractice.Models
{
    public partial class Player
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string Name { get; set; } = null!;
    }
}
