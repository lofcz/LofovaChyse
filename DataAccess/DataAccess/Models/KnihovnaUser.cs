using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaUser : MembershipUser, IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual KnihovnaRole Role { get; set; }

    }
}
