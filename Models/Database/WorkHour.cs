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
    
    public partial class WorkHour
    {
        public int EmployeeId { get; set; }
        public System.DateTime Date { get; set; }
        public byte Hours { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
