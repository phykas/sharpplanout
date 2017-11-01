using SharpPlanOut.Core;
using SharpPlanOut.Core.Random;
using SharpPlanOut.Services.Interfaces;
using System.Collections.Generic;

namespace SharpPlanOut.Demo
{
    public class NamepsaceConfiguration
    {
        public static void Configure(INamespaceManager namespaceManager)
        {
            VideoPromotionNamespace(namespaceManager);
            TextNamespace(namespaceManager);
        }

        private static void TextNamespace(INamespaceManager namespaceManager)
        {
            var myfirstNamespace = new SimpleNamespace("textNamespace", namespaceManager.Inputs, "userId", 100);

            Experiment demoButtonEx = new Experiment("textExperiment", namespaceManager.Inputs, (assignment, objects) =>
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
            myfirstNamespace.AddExperiment(demoButtonEx, 100);

            namespaceManager.AddNamespace(myfirstNamespace);
        }

        private static void VideoPromotionNamespace(INamespaceManager namespaceManager)
        {
            var myfirstNamespace = new SimpleNamespace("video_promotion", namespaceManager.Inputs, "userId", 100);

            Experiment videoExperiment = new Experiment("video_experiment1", namespaceManager.Inputs,
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
            myfirstNamespace.AddExperiment(videoExperiment, 100);

            namespaceManager.AddNamespace(myfirstNamespace);
        }
    }
}