using Microsoft.EntityFrameworkCore;

namespace AnnosWebAPI.Data
{
    public class DataInitializer
    {
        private readonly AppDbContext _dbContext;
        public DataInitializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void SeedData()
        {
            _dbContext.Database.Migrate();
        }
    }
}
