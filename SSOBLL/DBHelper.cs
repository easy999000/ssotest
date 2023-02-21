using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DBHelper : FreeSqlHelperStatic
{
    public static IFreeSql SSOCenter
    {
        get
        {
            return StaticDB.GetDB("SSOCenter");
        }
    }
}

