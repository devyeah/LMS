using System;
using System.Collections.Generic;
using System.Text;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business
{
    public class ServiceBase
    {
        protected static ServiceResult<T> BuildResult<T>(bool isSuccess, T code, string message = "", object resultObj = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = isSuccess,
                ResultCode = code,
                Message = message,
                ResultObj = resultObj
            };
        }
    }
}
