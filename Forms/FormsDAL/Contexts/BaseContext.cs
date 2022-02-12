using Microsoft.EntityFrameworkCore;

namespace FormsDal.Contexts
{
    public partial class BaseContext : DbContext
    {
        public BaseContext()
        {
        }

        public BaseContext(DbContextOptions<DbContext> options)
                : base(options)
        {
        }
    }
}
