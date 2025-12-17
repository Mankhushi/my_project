using Microsoft.EntityFrameworkCore;

namespace MSINS_API.Data
{
    public class MSINSDbContext: DbContext
    {
        public MSINSDbContext(DbContextOptions<MSINSDbContext> options) : base(options)
        {
        }
    }
}
