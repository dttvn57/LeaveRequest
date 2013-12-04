using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveRequest.Models
{
    [Table("HR_PayPeriod")]
    public class PayPeriod
    {
        public int ID { get; set; }
        public string Period { get; set; }
    }
}