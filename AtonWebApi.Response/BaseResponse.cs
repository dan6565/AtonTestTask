namespace AtonWebApi.Response
{
    public class BaseResponse<T>:IBaseResponse
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
    public class BaseResponse : IBaseResponse
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
    }
}