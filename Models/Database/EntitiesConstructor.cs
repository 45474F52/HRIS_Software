using System.Data.Entity.Core.EntityClient;

namespace HRIS_Software.Models.Database
{
    public partial class Entities
    {
        public Entities(EntityConnectionStringBuilder builder)
            : base(builder.ConnectionString)
        { }
    }
}
