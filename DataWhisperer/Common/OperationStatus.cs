using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataWhisperer.Common
{
    public enum OperationStatus
    {
        Success,
        Processing,
        InvalidInput,
        Duplicate,
        NotFound,
        Error,
        Warning,
        Other,
    }
}