using Ninject.Modules;
using SharpPlanOut.Core;
using SharpPlanOut.Core.Loggers;
using SharpPlanOut.Logger.MongoDb;

namespace SharpPlanOut.Di.Loggers
{
    public class LoggersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventLogger>().To<DebugEventLogger>();
        }
    }
}