using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

namespace CLRSurvey.Features.CLRSurveyTimerJobFeature
{
    [Guid("a6739d95-fb1e-48c5-a864-0ad563db7e82")]
    public class CLRSurveyTimerJobFeatureEventReceiver : SPFeatureReceiver
    {
        const string CLRSurvey_JOB_NAME = "CLRSurvey Timer Job";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            // make sure the job isn't already registered
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == CLRSurvey_JOB_NAME)
                    job.Delete();
            }

            // install the job
            CLRSurveyTimerJob CLRSurveyTimerJobLogger = new CLRSurveyTimerJob(CLRSurvey_JOB_NAME, site.WebApplication);
            SPMinuteSchedule schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 59;
            schedule.Interval = 10;
            CLRSurveyTimerJobLogger.Schedule = schedule;
            CLRSurveyTimerJobLogger.Update();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            // delete the job
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == CLRSurvey_JOB_NAME)
                    job.Delete();
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
