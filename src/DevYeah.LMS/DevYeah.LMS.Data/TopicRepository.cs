using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace DevYeah.LMS.Data
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
