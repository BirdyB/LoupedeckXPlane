namespace Loupedeck.XplanePlugin
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;

    using XPlaneConnector;
    using XPlaneConnector.DataRefs;

    public class XplanePlugin : Plugin
    {
        public override void Load()
        {
            this.Info.DisplayName = "X-Plane Controls";
            this.Info.Description = "Controls for X-Plane 11";
            this.Info.Author = "Berthold Ruehl";
            this.Info.Icon16x16 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("xplane_logo16.png"));
            this.Info.Icon32x32 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("xplane_logo32.png"));
            this.Info.Icon48x48 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("xplane_logo48.png"));
            this.Info.Icon256x256 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("xplane_logo256.png"));
            SupportClasses.DebugClass.init();

            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                Debug.WriteLine(eventArgs.Exception.ToString());
            };
            try
            {
                SupportClasses.ConnectorHandler.init();
               SupportClasses.AirplaneData.init();
               SupportClasses.AirplaneData.getData();
                SupportClasses.BoundariesData.init();
                SupportClasses.BoundariesData.getData();
                SupportClasses.SubscriptionHandler.init();
      
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                SupportClasses.DebugClass.ExceptionReceived(e);
            }

        }


        public override Boolean UsesApplicationApiOnly => true;
        public override void Unload()
        {
        
        }

        private void OnApplicationStarted(Object sender, EventArgs e)
        {
        }

        private void OnApplicationStopped(Object sender, EventArgs e)
        {
        }

        public override void RunCommand(String commandName, String parameter)
        {
        }

        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff)
        {
        }
    }
}
