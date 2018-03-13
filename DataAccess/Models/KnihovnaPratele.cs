using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaPratele : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int UserFrom { get; set; }
        public virtual int UserTo { get; set; }
        public virtual bool Accepted { get; set; }
        public virtual DateTime DateSent { get; set; }
        public virtual DateTime DateAccepted { get; set; }
        public virtual bool Declined { get; set; }
        public virtual DateTime DateDeclined { get; set; }
        public virtual int Type { get; set; }
    }
}
