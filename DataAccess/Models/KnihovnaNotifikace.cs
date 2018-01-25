using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaNotifikace : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int UserFrom { get; set; }
        public virtual int UserTo { get; set; }

        [AllowHtml]
        public virtual string Text { get; set; }


        public virtual bool Displayed { get; set; }
        public virtual int ForceType { get; set; }
        public virtual int RewardType { get; set; }
        public virtual DateTime DateSent { get; set; }
    }
}
