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
    
    public partial class Salary
    {
        public int EmployeeId { get; set; }
        public decimal Wage { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
