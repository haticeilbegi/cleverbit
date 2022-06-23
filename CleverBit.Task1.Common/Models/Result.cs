using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace CleverBit.Task1.Common.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        [Newtonsoft.Json.JsonIgnore, JsonIgnore, XmlIgnore]
        public Exception Exception { get; set; }

        public Result()
        {

        }

        public Result(string message)
        {
            Message = message;
        }

        public Result(string message, bool isSuccess)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result(string message, bool isSuccess, T data)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
        }

        public Result(Exception ex, T data)
        {
            Data = data;
            Message = ex.Message;
            Exception = ex;
        }
    }
}
