//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankingProject_Demo.DataAccess
{
    using System;
    using System.Collections.Generic;
    /* 
COPY RIGHT @AUGMENTO LABS 2020
Created By Alam
*/
    public partial class Transcation_Details
    {
        public int TranscationId { get; set; }
        public string AccountNumber { get; set; }
        public string TranscationType { get; set; }
        public decimal Amount { get; set; }
        public Nullable<System.DateTime> TranscationDate { get; set; }
    
        public virtual AccountHolderDetail AccountHolderDetail { get; set; }
    }
}
