using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class KnihovnaRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser user = knihovnaUserDao.GetByLogin(username);

            if (user == null)
            {
                return false;
            }

            return user.Role.Identificator == roleName; 
        }

        public override string[] GetRolesForUser(string username)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser user = knihovnaUserDao.GetByLogin(username);

            KnihovnaUserRoleDao dd = new KnihovnaUserRoleDao();
            List<KnihovnaUserRole> r = dd.GetUserRoles(user.Id) as List<KnihovnaUserRole>;

            List<KnihovnaRole> vsechnyRole = new KnihovnaRoleDao().GetAll() as List<KnihovnaRole>;

            List<string> konecneRole = new List<string>();

            foreach (KnihovnaUserRole cr in r)
            {
                foreach (KnihovnaRole aktR in vsechnyRole)
                {
                    if (cr.RoleId == aktR.Id)
                    {
                        konecneRole.Add(aktR.Identificator);
                    }
                }
            }

            if (user == null)
            {
                return new string[] { };
            }

            return konecneRole.ToArray();
        }

        public override void CreateRole(string roleName)
        {

        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}