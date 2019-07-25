using System;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Business
{
    public class TopicService : ServiceBase, ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public ServiceResult<TopicServiceResultCode> AddTopic(AddOrUpdateTopicRequest request)
        {
            var isValidRequest = ValidateAddTopicRequest(request);
            if (!isValidRequest) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            var topic = new Topic { Id = Guid.NewGuid(), CourseId = request.CourseId, Title = request.Title, DisplayOrder = request.DisplayOrder };
            try
            {
                _topicRepository.Add(topic);
                _topicRepository.SaveChanges();
                return BuildResult(true, TopicServiceResultCode.Success, resultObj: topic);
            }
            catch (Exception)
            {
                return InternalErrorResult(TopicServiceResultCode.BackenException);
            }
        }

        private bool ValidateAddTopicRequest(AddOrUpdateTopicRequest request)
        {
            if (request == null) return false;
            if (request.CourseId == null || request.CourseId.Equals(Guid.Empty)) return false;
            if (string.IsNullOrWhiteSpace(request.Title)) return false;
            if (request.DisplayOrder <= 0) return false;

            return true;
        }

        public ServiceResult<TopicServiceResultCode> DeleteTopic(Guid topicId)
        {
            if (topicId == null || topicId.Equals(Guid.Empty)) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            try
            {
                var targetTopic = _topicRepository.Get(topicId);
                if (targetTopic == null) return DataErrorResult(TopicServiceResultCode.DataNotExist);
                _topicRepository.Delete(targetTopic);
                return BuildResult(true, TopicServiceResultCode.Success);
            }
            catch (Exception)
            {
                return InternalErrorResult(TopicServiceResultCode.BackenException);
            }
        }

        public ServiceResult<TopicServiceResultCode> GetAllTopicsOfCourse(Guid courseId)
        {
            if (courseId == null || courseId.Equals(Guid.Empty)) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            try
            {
                var topicList = _topicRepository.GetAllTopicsByCourseId(courseId);
                return BuildResult(true, TopicServiceResultCode.Success, resultObj: topicList);
            }
            catch (Exception)
            {
                return InternalErrorResult(TopicServiceResultCode.BackenException);
            }
        }

        public ServiceResult<TopicServiceResultCode> GetTopicByKey(Guid key)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<TopicServiceResultCode> UpdateTopic(AddOrUpdateTopicRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
