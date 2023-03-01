using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL
{
    public class WebThreadTest
    {
        public void Start()
        {
            System.Threading.Thread th = new Thread(MainTh);
            th.IsBackground = true;
            th.Start();
        }
        public void MainTh()
        {
            RedisHelperStatic.DBDefault.StringSet("SSO:WebThreadTestStart", DateTime.Now.ToString());
            while (true)
            {
                RedisHelperStatic.DBDefault.StringSet("SSO:WebThreadTestShile", DateTime.Now.ToString());

                System.Threading.Thread.Sleep(20000);



            }
        }

    
    }
}
