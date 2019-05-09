

namespace TCM.Web.API.Core.Signalr
{

    //TO DO 
    //public class HubGlobalErrorHandling : HubPipelineModule
    //{
    //    private ILogger _logger;
    //    public ILogger Logger
    //    {
    //        get
    //        {
    //            if (_logger == null)
    //            {
    //                _logger = (ILogger)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger));
    //            }
    //            return _logger;
    //        }
    //        set
    //        {
    //            if (value != null) _logger = value;
    //        }
    //    }

    //    protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
    //    {
    //        Logger.Exception(exceptionContext.Error);
    //        Logger.Debug("=> Exception " + exceptionContext.Error.Message);
    //        Debug.WriteLine("=> Exception " + exceptionContext.Error.Message);
    //        if (exceptionContext.Error.InnerException != null)
    //        {
    //            Logger.Debug("=> Inner Exception " + exceptionContext.Error.InnerException.Message);
    //            Debug.WriteLine("=> Inner Exception " + exceptionContext.Error.InnerException.Message);
    //        }
    //        base.OnIncomingError(exceptionContext, invokerContext);

    //    }
    //}
}