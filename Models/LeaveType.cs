using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveRequest.Models
{
    [Table("HR_LR_LeaveType")]
    public class LeaveType
    {
        public int ID { get; set; }
        public string Type { get; set; }
    }
}