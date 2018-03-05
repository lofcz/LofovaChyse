using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class BookOdznakArchetyp : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual string Image { get; set; }
        public virtual string Name { get; set; }
    }
}
