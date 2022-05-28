namespace AtonWebApi.Response
{
    public interface IBaseResponse
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }       
    }
}