using System;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    public interface ITopicService
    {
        ServiceResult<TopicServiceResultCode> AddTopic(AddOrUpdateTopicRequest request);
        ServiceResult<TopicServiceResultCode> UpdateTopic(AddOrUpdateTopicRequest request);
        ServiceResult<TopicServiceResultCode> DeleteTopic(Guid topicId);
        ServiceResult<TopicServiceResultCode> GetTopicByKey(Guid key);
        ServiceResult<TopicServiceResultCode> GetAllTopicsOfCourse(Guid courseId);
    }
}
