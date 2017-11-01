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
            Bind<INamespaceManager>().ToMethod(context =>
            {
                var menager = new NamespaceManager(new DebugEventLogger());
                NamepsaceConfiguration.Configure(menager);
                return menager;
            });
        }
    }
}