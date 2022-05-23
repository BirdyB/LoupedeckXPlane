using System;
using System.Diagnostics;
using System.Threading;
using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.Buttons
{
    public class IgnitionLL : PluginDynamicCommand
    {
        public IgnitionLL() : base("Igniter 1", "Igniter 1 engage", "Engines")
        {
        }

        public XPlaneConnector.XPlaneConnector connector = new XPlaneConnector.XPlaneConnector();
        private bool starterEngaged = false;
        private bool engineRunning = false;
        private System.Threading.CancellationTokenSource token = new System.Threading.CancellationTokenSource();

        protected override Boolean OnLoad() {
            connector.Start();
            return base.OnLoad();
        }

        protected override void RunCommand(String actionParameter) {

            /* if (!starterEngaged)
             { 
                 this.connector.SendCommand(Commands.MagnetosMagnetosBoth);

                 System.Threading.Tasks.Task.Run(() =>
                 {
                     while (!token.IsCancellationRequested)
                     {
                         this.connector.SendCommand(Commands.StartersEngageStarter1);
                         System.Threading.Thread.Sleep(20);
                         if (this.token.IsCancellationRequested)
                         {
                             break;
                         }
                     }
                 }, this.token.Token);


                 this.starterEngaged = true;

                 this.ActionImageChanged();
             }
             else
             {
                 this.token.Cancel();
                 this.starterEngaged = false;
                 this.ActionImageChanged();
             }
            */
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            /* var builder = new BitmapBuilder(imageSize);

            if (this.starterEngaged)
            {
                builder.Clear(new BitmapColor(255, 0, 0));
                builder.DrawText("Starter engaged");
            }
            else
            {
                builder*.Clear(new BitmapColor(0, 255, 0));
                builder.DrawText("Starter standby");
                return SupportClasses.ButtonImages.activeImage(imageSize, "Test","headphones.png");
            }


            return builder.ToImage(); */

            var graph = new TypeClasses.Graph(0,120,20,40,60,80,100,120);
            BitmapImage graphpic = graph.getGraph(70);

            return SupportClasses.ButtonImages._standardImageGraph(imageSize,BitmapColor.White,BitmapColor.Black,"Test",graphpic);
            //return graphpic;
        }

    }
}
