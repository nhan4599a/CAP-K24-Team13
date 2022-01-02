using System;

namespace Shared
{
    public class CommandResponse<TResponse>
    {
        private readonly TResponse _response;

        private readonly string _errorMessage;

        private readonly Exception _exception;

        public TResponse Response
        { 
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Command is failed");
                return _response;
            }
        }

        public bool IsSuccess { get; private set; }

        public string ErrorMessage { get => IsSuccess ? null : _errorMessage; }

        public Exception Exception { get => IsSuccess ? null : _exception; }

        private CommandResponse(bool isSuccess, TResponse response, string errorMessage, Exception e)
        {
            IsSuccess = isSuccess;
            if (isSuccess)
                _response = response;
            else
            {
                _errorMessage = errorMessage;
                _exception = e;
            }
        }

        public static CommandResponse<TResponse> Success(TResponse response) => new(true, response, null, null);

        public static CommandResponse<TResponse> Error(string errorMessage, Exception e) => 
            new(false, default, errorMessage, e);
    }
}
