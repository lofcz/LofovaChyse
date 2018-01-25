using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaZed : IEntity
    {
        public virtual int Id { get; set; }

        [AllowHtml]
        public virtual string PostText { get; set; }
        public virtual DateTime PostDate { get; set; }
        public virtual KnihovnaUser PostOwner { get; set; }
        public virtual double Version { get; set; }
    }
}
