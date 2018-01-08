using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class BookLikes : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int BookId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int Value { get; set; }
    }
}
