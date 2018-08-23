using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataWhisperer.Models
{
    public class RequestModel
    {
        public int RequestTypeId { get; set; }
        public string AsUserName { get; set; }
        public string AdUserStartDate { get; set; }
        public string JiraAccountType { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}