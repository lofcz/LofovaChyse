using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class ChatZpravy : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual int UserFrom { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
