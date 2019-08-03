using System;
using System.Collections.Generic;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Data.Interfaces
{
    public interface ITopicRepository : IRepository<Topic>
    {
        IEnumerable<Topic> GetAllTopicsByCourseId(Guid courseId);
    }
}
