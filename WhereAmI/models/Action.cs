using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereAmI.models
{
    public class Action
    {
        public long ActionId { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }

        public virtual ICollection<Place> Places { get; set;}

        public override string ToString()
        {
            return this.Name + ": " + this.Command + ";";
        }
    }
}
