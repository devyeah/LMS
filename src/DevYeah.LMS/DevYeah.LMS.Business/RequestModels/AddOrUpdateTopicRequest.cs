using System;

namespace DevYeah.LMS.Business.RequestModels
{
    public class AddOrUpdateTopicRequest
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public short DisplayOrder { get; set; }
    }
}
