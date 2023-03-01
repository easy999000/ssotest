using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisHelper
    {


        private string RedisConnStr = "";
        public ConnectionMultiplexer DBConn;
        public RedisHelper(string connStr)
        {
            RedisConnStr = connStr;
            DBConn = StackExchange.Redis.ConnectionMultiplexer.Connect(RedisConnStr);
        }

        //public bool test2(int sssssssss1, string ssssssssss2 = "sssssssss", string sssssssssssss3 = "sssssssss", string sssssssss4 = "sssssssss", string sssssssss5 = "sssssssss", string sssssssss6 = "sssssssss", string sssssssss7 = "sssssssss", string ssssssssss8 = "sssssssss" )
        //{
        //    return true;
        //}
        //public bool test2(RedisKey key, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None )
        //{
        //    return true;
        //}

        #region GetDatabase
        public IDatabase GetDatabase(int db = -1)
        {
            return DBConn.GetDatabase(db);
        }
        public IDatabase DBDefault
        {
            get
            {
                return GetDatabase();
            }
        }
        public IDatabase DB0
        {
            get
            {
                return GetDatabase(0);
            }
        }
        public IDatabase DB1
        {
            get
            {
                return GetDatabase(1);
            }
        }
        public IDatabase DB2
        {
            get
            {
                return GetDatabase(2);
            }
        }
        public IDatabase DB3
        {
            get
            {
                return GetDatabase(3);
            }
        }
        public IDatabase DB4
        {
            get
            {
                return GetDatabase(4);
            }
        }
        public IDatabase DB5
        {
            get
            {
                return GetDatabase(5);
            }
        }
        public IDatabase DB6
        {
            get
            {
                return GetDatabase(6);
            }
        }
        public IDatabase DB7
        {
            get
            {
                return GetDatabase(7);
            }
        }
        public IDatabase DB8
        {
            get
            {
                return GetDatabase(8);
            }
        }
        public IDatabase DB9
        {
            get
            {
                return GetDatabase(9);
            }
        }
        public IDatabase DB10
        {
            get
            {
                return GetDatabase(10);
            }
        }
        public IDatabase DB11
        {
            get
            {
                return GetDatabase(11);
            }
        }
        public IDatabase DB12
        {
            get
            {
                return GetDatabase(12);
            }
        }
        public IDatabase DB13
        {
            get
            {
                return GetDatabase(13);
            }
        }
        public IDatabase DB14
        {
            get
            {
                return GetDatabase(14);
            }
        }
        public IDatabase DB15
        {
            get
            {
                return GetDatabase(15);
            }
        }



        #endregion

         
    }

    public class RedisHelperStatic
    {
        public static RedisHelper Redis;
        public static void InitStatic(string connStr)
        {
            Redis = new RedisHelper(connStr);

            //// DB0.StringSet("",) /// 没有提示
            //Redis.test2(   ///可以复现bug

        }

        #region GetDatabase
        public static IDatabase GetDatabase(int db = -1)
        {
            return Redis.GetDatabase(db);
        }
        public static IDatabase DBDefault
        {
            get
            {
                return GetDatabase();
            }
        }
        public static IDatabase DB0
        {
            get
            {
                return GetDatabase(0);
            }
        }
        public static IDatabase DB1
        {
            get
            {
                return GetDatabase(1);
            }
        }
        public static IDatabase DB2
        {
            get
            {
                return GetDatabase(2);
            }
        }
        public static IDatabase DB3
        {
            get
            {
                return GetDatabase(3);
            }
        }
        public static IDatabase DB4
        {
            get
            {
                return GetDatabase(4);
            }
        }
        public static IDatabase DB5
        {
            get
            {
                return GetDatabase(5);
            }
        }
        public static IDatabase DB6
        {
            get
            {
                return GetDatabase(6);
            }
        }
        public static IDatabase DB7
        {
            get
            {
                return GetDatabase(7);
            }
        }
        public static IDatabase DB8
        {
            get
            {
                return GetDatabase(8);
            }
        }
        public static IDatabase DB9
        {
            get
            {
                return GetDatabase(9);
            }
        }
        public static IDatabase DB10
        {
            get
            {
                return GetDatabase(10);
            }
        }
        public static IDatabase DB11
        {
            get
            {
                return GetDatabase(11);
            }
        }
        public static IDatabase DB12
        {
            get
            {
                return GetDatabase(12);
            }
        }
        public static IDatabase DB13
        {
            get
            {
                return GetDatabase(13);
            }
        }
        public static IDatabase DB14
        {
            get
            {
                return GetDatabase(14);
            }
        }
        public static IDatabase DB15
        {
            get
            {
                return GetDatabase(15);
            }
        }



        #endregion

    }
}
