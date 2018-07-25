using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LofovaChyse.Class
{
    public static class Structs
    {
        public struct UserOptionalFields
        {
            public string Value { get; set; }
            public string Key { get; set; }
            public string Icon { get; set; }

            public UserOptionalFields(string val, string key, string icon)
            {
                this.Value = val;
                this.Key = key;
                this.Icon = icon;
            }
        }
    }
}