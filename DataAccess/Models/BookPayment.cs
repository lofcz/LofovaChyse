using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class BookPayment : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int PostId { get; set; }
        public virtual DateTime DateUnlocked { get; set; }
        public virtual bool IsPreview { get; set; }
    }
}
