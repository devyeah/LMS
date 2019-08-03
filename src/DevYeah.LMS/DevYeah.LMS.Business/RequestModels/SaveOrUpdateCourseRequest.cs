using System;

namespace DevYeah.LMS.Business.RequestModels
{
    public class SaveOrUpdateCourseRequest
    {
        public Guid Id { get; set; }
        public Guid InstructorId { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public byte Edition { get; set; }
        public byte Level { get; set; }
        public float AvgLearningTime { get; set; }
        public Guid[] Categories { get; set; }
    }
}
