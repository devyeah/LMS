namespace DevYeah.LMS.Business.ResultModels
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object ResultObj { get; set; }

        public T ResultCode { get; set; }
    }
}
