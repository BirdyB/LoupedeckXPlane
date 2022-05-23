using System;
using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.Buttons
{
    public class SimSpeedCmd : PluginDynamicCommand
    {
        public SimSpeedCmd() : base("SimSpeed", "SetSimSpeed", "Sim")
        {
        }

        public XPlaneConnector.XPlaneConnector connector = new XPlaneConnector.XPlaneConnector();

        private int _simSpeed = 1;

        protected override Boolean OnLoad() {
            this.connector.Start();
            this.connector.Subscribe(DataRefs.TimeSimSpeed, 5, (e, v) => {
                if (this._simSpeed != Convert.ToInt32(v))
                {
                    this._simSpeed = Convert.ToInt32(v);
                    this.ActionImageChanged();
                }
            });
            return base.OnLoad();
        }

        protected override void RunCommand(String actionParameter)
        {
            switch (this._simSpeed)
            {
                case 0:
                    connector.SetDataRefValue(DataRefs.TimeSimSpeed, 1);
                    this.ActionImageChanged();
                    break;
                case 1:
                    connector.SetDataRefValue(DataRefs.TimeSimSpeed, 2);
                    this.ActionImageChanged();
                    break;
                case 2:
                    connector.SetDataRefValue(DataRefs.TimeSimSpeed, 0);
                    this.ActionImageChanged();
                    break;
            }
            
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize) {
            var builder = new BitmapBuilder(imageSize);
            switch (this._simSpeed)
            {
                case 0:
                    return SupportClasses.ButtonImages.customImage(imageSize, $"SimSpeed\r\n{this._simSpeed}", "", new BitmapColor(255, 0, 0), BitmapColor.Black);
                    break;
                case 1:
                    return SupportClasses.ButtonImages.customImage(imageSize, $"SimSpeed\r\n{this._simSpeed}", "", new BitmapColor(255, 255, 255), BitmapColor.Black);
                    break;
                case 2:
                    return SupportClasses.ButtonImages.customImage(imageSize, $"SimSpeed\r\n{this._simSpeed}", "", new BitmapColor(0, 0, 255), BitmapColor.Black);
 
                    break;
                default:
                    return SupportClasses.ButtonImages.customImage(imageSize, $"SimSpeed\r\n{this._simSpeed}", "", new BitmapColor(255, 255, 255), BitmapColor.Black);
                    break;
            }


        }
    }
}
