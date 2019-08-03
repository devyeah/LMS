using System;

namespace DevYeah.LMS.Business.RequestModels
{
    public class AddOrUpdateTopicRequest
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public short DisplayOrder { get; set; }
    }
}
