using System;
using System.Collections.Generic;
using System.Text;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business
{
    public class ServiceBase
    {
        private static readonly string ArgumentErrorMsg = "Argument is not correct.";
        private static readonly string InternalErrorMsg = "Server internal error.";
        private static readonly string RequestFailureMsg = "Your request failed.";

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

        protected ServiceResult<T> ArgumentErrorResult<T>(T code)
        {
            return BuildResult(false, code, ArgumentErrorMsg);
        }

        protected ServiceResult<T> InternalErrorResult<T>(T code)
        {
            return BuildResult(false, code, InternalErrorMsg);
        }

        protected ServiceResult<T> DataErrorResult<T>(T code)
        {
            return BuildResult(false, code, RequestFailureMsg);
        }
    }
}
