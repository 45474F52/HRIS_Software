//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRIS_Software.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeesCours
    {
        public int EmployeeId { get; set; }
        public int CourseId { get; set; }
        public int Rating { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
