using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class BookVersion : IEntity
    {
        public virtual int Id { get; set; }

        [AllowHtml]
        public virtual string Text { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int ChangedBy { get; set; }
        public virtual double Version { get; set; }
        public virtual int PostId { get; set; }

        [AllowHtml]
        public virtual string SumText { get; set; }
        public virtual bool IsSuggestion { get; set; }
        public virtual bool IsApproved { get; set; }
    }
}
