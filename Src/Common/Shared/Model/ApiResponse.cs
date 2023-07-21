namespace Shared.Model
{
    public class ApiResponse
    {
        public List<Error> Errors { get; set; }

        public bool Success =>
          !(Errors.Count > 0);

        public ApiResponse()
        {
            Errors = new List<Error>();
        }
    }

    public class ApiResponse<TModel> : ApiResponse
    {
        public TModel Data { get; set; }

        public ApiResponse(TModel data)
            : base()
        {
            Data = data;
        }
    }

}
