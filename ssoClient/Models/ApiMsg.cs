using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssoClient.Models
{
    public class ApiMsg<T>
    {
        public ApiMsg() { }

        public ApiMsg(int code)
        {
            Code = code;
        }
        public ApiMsg(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }


        public ApiMsg(int code, string msg, T data)
        {
            Code = code;
            Msg = msg;
            Data = data;
        }

        public T Data { get; set; }

        /// <summary>
        /// 1表示成功,非1表示失败
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Msg { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApiMsg<T> ReturnSuccess()
        {
            return new ApiMsg<T>(1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApiMsg<T> ReturnSuccess(T data)
        {
            return new ApiMsg<T>(1, "", data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApiMsg<T> ReturnError(string message)
        {
            return new ApiMsg<T>(0, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApiMsg<T> ReturnError(int code, string message)
        {
            return new ApiMsg<T>(code, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApiMsg<T> ReturnError(string message, T data)
        {
            return new ApiMsg<T>(0, message, data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApiMsg<T> ReturnError(int code, string message, T data)
        {
            return new ApiMsg<T>(code, message, data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiMsg<T> SetSuccess()
        {
            this.Code = 1;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiMsg<T> SetSuccess(T data)
        {
            this.Code = 1;
            this.Data= data;
            return this;
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiMsg<T> SetError(string message)
        {
            this.Code = 0;
            this.Msg = message;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiMsg<T> SetError(int code,string message)
        {
            this.Code = code;
            this.Msg = message;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiMsg<T> SetError(string message, T data)
        {
            this.Code = 0;
            this.Msg = message;
            this.Data = data;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiMsg<T> SetError(int code,string message, T data)
        {
            this.Code = code;
            this.Msg = message;
            this.Data = data;
            return this;
        }
    }
    public class ApiMsg : ApiMsg<object>
    {
    }
}
