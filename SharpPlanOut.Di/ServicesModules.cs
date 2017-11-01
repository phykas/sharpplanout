using Ninject.Modules;
using SharpPlanOut.Core.Loggers;
using SharpPlanOut.Demo;
using SharpPlanOut.Logger.MongoDb;
using SharpPlanOut.Services.Interfaces;
using SharpPlanOut.Services.Namespace;

namespace SharpPlanOut.Di
{
    public class ServicesModules : NinjectModule
    {
        public override void Load()
        {
            //Demo namespace binding
            Bind<INamespaceManagerService>().ToMethod(context =>
            {
                var menager = new NamespaceManagerService(new DebugEventLogger());
                NamepsaceConfiguration.Configure(menager);
                return menager;
            });
        }
    }
}