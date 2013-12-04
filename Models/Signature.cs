using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace LeaveRequest.Models
{
    [Table("HR_LR_Signature")]
    public class Signature
    {  
        #region Constructor
        public Signature()
        {
            IsSignature = false;
        }        
        #endregion

        #region Property(s)

        public int ID { get; set; }
        public string SignatureStr { get; set; }

        public string LName { get; set; }
        public string FName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SignedDate { get; set; }
        
        public Guid? SignatureGuid { get; set; }
        
        public bool IsSignature { get; set; }

        public int SignedByManager { get; set; }
        #endregion
    }
}