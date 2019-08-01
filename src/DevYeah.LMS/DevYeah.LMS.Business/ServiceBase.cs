using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Interfaces;

namespace DevYeah.LMS.Business
{
    public abstract class ServiceBase
    {
        private static readonly string ArgumentErrorMsg = "Argument is not correct.";
        private static readonly string InternalErrorMsg = "Server internal error.";
        private static readonly string RequestFailureMsg = "Your request failed.";

        protected readonly ISystemErrorsRepository _systemErrorsRepo;

        protected ServiceBase(ISystemErrorsRepository systemErrorsRepo)
        {
            _systemErrorsRepo = systemErrorsRepo;
        }

        protected ServiceResult<T> BuildResult<T>(bool isSuccess, T code, string message = "", object resultObj = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = isSuccess,
                ResultCode = code,
                Message = message,
                ResultObj = resultObj
            };
        }

        protected ServiceResult<T> ArgumentErrorResult<T>(T code) => BuildResult(false, code, ArgumentErrorMsg);

        protected ServiceResult<T> InternalErrorResult<T>(T code) => BuildResult(false, code, InternalErrorMsg);

        protected ServiceResult<T> DataErrorResult<T>(T code) => BuildResult(false, code, RequestFailureMsg);
    }
}
