using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveRequest.Models
{
    [Table("HR_LR_Manager")]
    public class Manager
    {
        public int ID { get; set; }

        public string LName { get; set; }
        public string FName { get; set; }
        public string FullName {
            get { return LName + ", " + FName; }
        }

        public string Title { get; set; }
        public string Email { get; set; }

        public bool IsNotified { get; set; }
    }

}