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
        private readonly ITopicRepository _topicRepo;

        public TopicService(ITopicRepository topicRepository, ISystemErrorsRepository systemErrorsRepo) : base(systemErrorsRepo)
        {
            _topicRepo = topicRepository;
        }

        public ServiceResult<TopicServiceResultCode> AddTopic(AddOrUpdateTopicRequest request)
        {
            var isValidRequest = ValidateAddTopicRequest(request);
            if (!isValidRequest) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            var topic = new Topic { Id = Guid.NewGuid(), CourseId = request.CourseId, Title = request.Title, DisplayOrder = request.DisplayOrder };
            try
            {
                _topicRepo.Add(topic);
                _topicRepo.SaveChanges();
                return BuildResult(true, TopicServiceResultCode.Success, resultObj: topic);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(TopicServiceResultCode.BackendException);
            }
        }

        public ServiceResult<TopicServiceResultCode> DeleteTopic(Guid topicId)
        {
            if (topicId == Guid.Empty) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            try
            {
                var targetTopic = _topicRepo.Get(topicId);
                if (targetTopic == null) return DataErrorResult(TopicServiceResultCode.DataNotExist);
                _topicRepo.Delete(targetTopic);
                return BuildResult(true, TopicServiceResultCode.Success);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(TopicServiceResultCode.BackendException);
            }
        }

        public ServiceResult<TopicServiceResultCode> GetAllTopicsOfCourse(Guid courseId)
        {
            if (courseId == Guid.Empty) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            try
            {
                var topicList = _topicRepo.GetAllTopicsByCourseId(courseId);
                return BuildResult(true, TopicServiceResultCode.Success, resultObj: topicList);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(TopicServiceResultCode.BackendException);
            }
        }

        public ServiceResult<TopicServiceResultCode> GetTopicByKey(Guid key)
        {
            if (key == Guid.Empty) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            try
            {
                var topic = _topicRepo.Get(key);
                if (topic == null) return DataErrorResult(TopicServiceResultCode.DataNotExist);
                return BuildResult(true, TopicServiceResultCode.Success, resultObj: topic);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(TopicServiceResultCode.BackendException);
            }
        }

        public ServiceResult<TopicServiceResultCode> UpdateTopic(AddOrUpdateTopicRequest request)
        {
            var isValidRequest = ValidateUpdateTopicRequest(request);
            if (!isValidRequest) return ArgumentErrorResult(TopicServiceResultCode.ArgumentError);

            try
            {
                var targetTopic = _topicRepo.Get(request.Id);
                if (targetTopic == null) return DataErrorResult(TopicServiceResultCode.DataNotExist);
                UpdateDataOfTopic(targetTopic, request);
                _topicRepo.Update(targetTopic);
                _topicRepo.SaveChanges();
                return BuildResult(true, TopicServiceResultCode.Success, resultObj: targetTopic);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(TopicServiceResultCode.BackendException);
            }
        }

        private void UpdateDataOfTopic(Topic topic, AddOrUpdateTopicRequest request)
        {
            topic.Title = request.Title;
            topic.DisplayOrder = request.DisplayOrder;
        }

        private bool ValidateUpdateTopicRequest(AddOrUpdateTopicRequest request)
        {
            var isValidData = ValidateAddTopicRequest(request);
            var isValidKey = !(request.Id == Guid.Empty);
            return (isValidData && isValidKey);
        }

        private bool ValidateAddTopicRequest(AddOrUpdateTopicRequest request)
        {
            if (request == null) return false;
            if (request.CourseId == Guid.Empty) return false;
            if (string.IsNullOrWhiteSpace(request.Title)) return false;
            if (request.DisplayOrder <= 0) return false;

            return true;
        }
    }
}
