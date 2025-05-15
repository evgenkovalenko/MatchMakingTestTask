using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace MatchMaking.Common.Types
{
    public abstract class BaseService<TRequest, TResponse> : ICustomService<TRequest, TResponse> where TRequest : BaseRequest where TResponse : BaseResponse, new()
    {
        protected ILogger Logger { get; private set; }

        public BaseService(ILogger<BaseService<TRequest, TResponse>> logger)
        {
            Logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            TResponse response;

            try
            {
                await Validate(request);
                response = await Execute(request);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response = HandleError(ex);
            }

            response.ExecutionTime = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Stop();

            Log(request, response);

            return response;
        }

        protected virtual void Log(TRequest request, TResponse response)
        {
            Logger.LogInformation("RequestExecuted. Typ: {Type}: ExecutionTime: {ExecutionTime:n0}ms", this.GetType().ToString(), response.ExecutionTime);
        }

        protected virtual Task Validate(TRequest request)
        {
            return Task.CompletedTask;
        }

        public abstract Task<TResponse> Execute(TRequest request);

        protected virtual TResponse HandleError(Exception ex)
        {
            TResponse response = new TResponse()
            {
                Success = false,
                Message = ex.Message,
            };

            Logger.LogError("Error occured. Typ: {Type}, Exception :{Exception}", this.GetType().ToString(), ex.GetExceptionMessage());

            return response;
        }
    }
}
