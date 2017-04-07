
using System.Collections.Generic;

namespace TeCHeCerchi.Shared.Models
{
    public class Quest
    {

        public int Id { get; set; }

        public List<Beacon> Beacons { get; set; }

        public int Major { get; set; }

        public Clue Clue { get; set; }

        public Extra Extra { get; set; }

        public Question Question { get; set; }
    }
}

