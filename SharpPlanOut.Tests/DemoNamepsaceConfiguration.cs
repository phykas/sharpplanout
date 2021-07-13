using SharpPlanOut.Core;
using SharpPlanOut.Core.Random;
using System.Collections.Generic;
using SharpPlanOut.Core.Namespaces;

namespace SharpPlanOut.Demo
{
    public class NamespaceConfiguration
    {
        public static void Configure(INamespaceManagerService namespaceManager)
        {
            VideoPromotionNamespace(namespaceManager);
            TextNamespace(namespaceManager);
        }

        private static void TextNamespace(INamespaceManagerService namespaceManager)
        {
            var firstNamespace = new SimpleNamespace("textNamespace", namespaceManager.Inputs, "userId", 100);

            var demoButtonEx = new Experiment("textExperiment", namespaceManager.Inputs, (assignment, objects) =>
            {
                var demoButton = assignment.Set("demoButton", new UniformChoiceBuilder(new Dictionary<string, object>()
                {
                    {"choices", new[] {"requestDemo", "specialOffer", "earlyFreeAccess"}}
                }, namespaceManager.Inputs));

                var pageTitle = assignment.Set("pageTitle", new UniformChoiceBuilder(new Dictionary<string, object>()
                {
                    {"choices", new[] {"embracePower", "newGeneration", "improveBusiness"}}
                }, namespaceManager.Inputs));

                return true;
            }) { Log = objects => { namespaceManager.EventLogger.Log(objects); } };
            firstNamespace.AddExperiment(demoButtonEx, 100);

            namespaceManager.AddNamespace(firstNamespace);
        }

        private static void VideoPromotionNamespace(INamespaceManagerService namespaceManager)
        {
            var firstNamespace = new SimpleNamespace("video_promotion", namespaceManager.Inputs, "userId", 100);

            var videoExperiment = new Experiment("video_experiment1", namespaceManager.Inputs,
                (assignment, objects) =>
                {
                    var showVideo = assignment.Set("show_video",
                        new BernoulliTrialBuilder(new Dictionary<string, object>()
                        {
                            {"p", 0.6}
                        }, namespaceManager.Inputs));
                    if ((int)showVideo == 1)
                    {
                        var playVideo = assignment.Set("play_video",
                            new BernoulliTrialBuilder(new Dictionary<string, object>()
                            {
                                {"p", 0.4}
                            }, namespaceManager.Inputs));
                        if ((int)playVideo == 1)
                        {
                            var muteVideo = assignment.Set("mute_video",
                                new BernoulliTrialBuilder(new Dictionary<string, object>()
                                {
                                    {"p", 0.5}
                                }, namespaceManager.Inputs));
                        }
                    }

                    return true;
                }) { Log = objects => { namespaceManager.EventLogger.Log(objects); } };
            firstNamespace.AddExperiment(videoExperiment, 100);

            namespaceManager.AddNamespace(firstNamespace);
        }
    }
}