using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class BookOdznak : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int BookId { get; set; }
        public virtual int OdznakId { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
