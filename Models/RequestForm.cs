using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace LeaveRequest.Models
{
    [Table("HR_LR_RequestForm")]
    public class RequestForm
    {
        public RequestForm()
        {
            NoMoreSigsNeeded = false;
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }

        public string LName { get; set; }
        public string FName { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        public string Comment { get; set; }

        // 1
        public string TypeOfLeave1 { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        //// [DisplayFormat(DataFormatString = "{0:d}")] 
        [DataType(DataType.Date)]
        public DateTime? StartDate1 { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        [DataType(DataType.Date)]
        public DateTime? EndDate1 { get; set; }

        [Required(ErrorMessage = "Total Number of Hours is required")]
        public double TotalHours1 { get; set; }

        // 2
        public string TypeOfLeave2 { get; set; }

        //[Required(ErrorMessage = "Date is required")]
        //// [DisplayFormat(DataFormatString = "{0:d}")] 
        [DataType(DataType.Date)]
        public DateTime? StartDate2 { get; set; }

        //[Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime? EndDate2 { get; set; }

        public double TotalHours2 { get; set; }

        // 3
        public string TypeOfLeave3 { get; set; }

        //[Required(ErrorMessage = "Date is required")]
        //// [DisplayFormat(DataFormatString = "{0:d}")] 
        [DataType(DataType.Date)]
        public DateTime? StartDate3 { get; set; }

        //[Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime? EndDate3 { get; set; }

        public double TotalHours3 { get; set; }

        public string PayPeriod { get; set; }

        public int EmployeeSigId { get; set; }
        [ForeignKey("EmployeeSigId")]
        public virtual Signature EmployeeSig { get; set; }

        private ICollection<Signature> _ManagersSig;
        public virtual ICollection<Signature> ManagersSig
        {
            get { return _ManagersSig ?? (_ManagersSig = new HashSet<Signature>()); }
            set { _ManagersSig = value; }
        }

        public bool NoMoreSigsNeeded { get; set; }
    }
}