using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataWhisperer.Common
{
    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public OperationResult(OperationStatus status, string message, T data)
            : base(status, message)
        {
            Data = data;
        }

        public OperationResult(OperationStatus status, T data)
            : this(status, string.Empty, data)
        {

        }

        public OperationResult(string message, T data)
            : this(OperationStatus.Success, message, data)
        {

        }

        public OperationResult()
            : this(OperationStatus.Success, string.Empty, default(T))
        {

        }

        public static OperationResult Ok<T>(T data, string msg = "")
            => new OperationResult<T>(OperationStatus.Success, msg, data);

        public static OperationResult Warn<T>(T data, string msg = "")
            => new OperationResult<T>(OperationStatus.Warning, msg, data);

        public static OperationResult Error<T>(T data, string msg = "")
            => new OperationResult<T>(OperationStatus.Error, msg, data);
    }

    public class OperationResult
    {
        public bool IsSuccess
        {
            get { return Status == OperationStatus.Success; }
        }

        public OperationStatus Status { get; set; }
        public string Message { get; set; }

        public OperationResult(OperationStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public OperationResult(OperationStatus status)
            : this(status, string.Empty)
        {

        }

        public OperationResult(string message)
            : this(OperationStatus.Success, message)
        {

        }

        public OperationResult()
            : this(OperationStatus.Success, string.Empty)
        {

        }

        public static implicit operator bool(OperationResult result)
        {
            return result.IsSuccess;
        }

        public static OperationResult Ok(string msg = "")
            => new OperationResult(OperationStatus.Success, msg);

        public static OperationResult Warn(string msg = "")
            => new OperationResult(OperationStatus.Warning, msg);

        public static OperationResult Error(string msg = "")
            => new OperationResult(OperationStatus.Error, msg);
    }
}