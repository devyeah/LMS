using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    interface IResourceService
    {
        ServiceResult<CourseServiceResultCode> GetAllResourceOfTopic(string topicId);

        #region video resource
        ServiceResult<VideoServiceResultCode> AddVideoResource(SaveOrUpdateVideoRequest request);
        ServiceResult<VideoServiceResultCode> UpdateVideoResource(SaveOrUpdateVideoRequest request);
        ServiceResult<VideoServiceResultCode> GetVideo(string videoId);
        void DeleteVideoResource(string videoId);
        #endregion

        #region practice resource
        ServiceResult<PracticeServiceResultCode> AddPracticeResource(SaveOrUpdatePracticeRequest request);
        ServiceResult<PracticeServiceResultCode> UpdatePracticeResource(SaveOrUpdatePracticeRequest request);
        ServiceResult<PracticeServiceResultCode> GetPractice(string practiceId);
        void DeletePracticeResource(string practiceId);
        #endregion

        #region file resource
        ServiceResult<FileServiceResultCode> AddFileResource(SaveOrUpdateFileRequest request);
        ServiceResult<FileServiceResultCode> UpdateFileResource(SaveOrUpdateFileRequest request);
        ServiceResult<FileServiceResultCode> GetFile(string fileId);
        void DeleteFileResource(string fileId);
        #endregion

        #region quiz resource
        ServiceResult<QuizServiceResultCode> AddQuizResource(SaveOrUpdateQuizRequest request);
        ServiceResult<QuizServiceResultCode> UpdateQuizResource(SaveOrUpdateQuizRequest request);
        ServiceResult<QuizServiceResultCode> GetQuiz(string quizId);
        void DeleteQuizResource(string quizId);
        #endregion
    }
}
