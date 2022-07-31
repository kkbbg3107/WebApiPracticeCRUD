using Microsoft.EntityFrameworkCore;
using WebapiCorePractice.Models;

namespace WebapiCorePractice.Data
{
    public class IssueDbContext :DbContext
    {
        public IssueDbContext(DbContextOptions<IssueDbContext> options): base(options)
        {

        }

        public DbSet<Issue> Issue { get; set; }
    }
}
