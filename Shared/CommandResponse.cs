namespace Shared
{
    public class CommandResponse<TResponse>
    {
        public TResponse Response { get; set; }

        public string ErrorMessage { get; set; }

        public System.Exception Exception { get; set; }
    }
}
