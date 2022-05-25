using System;

using XPlaneConnector;
using XPlaneConnector.DataRefs;

namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class Lights : TemplateFolder
    {
        public Lights()
        {
            this._FirstValueReceived = true;
            this.DisplayName = "Lights";
            this.GroupName = "X-Plane";
        }

        private string image { get; } = "lightbulb.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        private bool beacon = false;
        private bool landing = false;
        private bool taxi = false;
        private bool nav = false;
        private bool strobe = false;

        protected override void FillSubscriptions()
        {
            this._subscriptions.TryAdd(new XPlaneConnector.DataRefElement { DataRef = "sim/cockpit/electrical/beacon_lights_on", Frequency = 5 }, (e, v) =>
            {
                if (this.beacon != Convert.ToBoolean(v))
                {
                    this.beacon = Convert.ToBoolean(v);
                    this.CommandImageChanged("Beacon");
                }
            });
            this._subscriptions.TryAdd(new XPlaneConnector.DataRefElement { DataRef = "sim/cockpit/electrical/landing_lights_on", Frequency = 5 }, (e, v) =>
            {
                if (this.landing != Convert.ToBoolean(v))
                {
                    this.landing = Convert.ToBoolean(v);
                    this.CommandImageChanged("Landing");
                }
            });
            this._subscriptions.TryAdd(new XPlaneConnector.DataRefElement { DataRef = "sim/cockpit/electrical/taxi_light_on", Frequency = 5 }, (e, v) =>
            {
                if (this.taxi != Convert.ToBoolean(v))
                {
                    this.taxi = Convert.ToBoolean(v);
                    this.CommandImageChanged("Taxi");
                }
            });

            this._subscriptions.TryAdd(new XPlaneConnector.DataRefElement { DataRef = "sim/cockpit/electrical/nav_lights_on", Frequency = 5 }, (e, v) =>
            {
                if (this.nav != Convert.ToBoolean(v))
                {
                    this.nav = Convert.ToBoolean(v);
                    this.CommandImageChanged("Nav");
                }
            });

            this._subscriptions.TryAdd(new XPlaneConnector.DataRefElement { DataRef = "sim/cockpit/electrical/strobe_lights_on", Frequency = 5 }, (e, v) =>
            {
                if (this.strobe != Convert.ToBoolean(v))
                {
                    this.strobe = Convert.ToBoolean(v);
                    this.CommandImageChanged("Strobe");
                }
            });



            base.FillSubscriptions();
        }

          
        protected override void FillAdjustments() {
            var temp = new TypeClasses.Adjustment();
            temp.id = $"Cockpit-\n\rlight";
            temp.showPicture = false;
            temp.element = new DataRefElement {DataRef = "sim/cockpit/electrical/cockpit_lights", Frequency = 5 };
            temp.command = null;
            temp.format = "n0";
            temp.unit = "%";
            temp.minvalue = 0;
            temp.maxvalue = 1;
            temp.divider = 0.01F;
            temp.setdivider = 100;
            this._adjustments.TryAdd(temp.id, temp);


            base.FillAdjustments();
        }
        protected override void FillButtons() {
            var temp = new TypeClasses.Button();
            temp.id = "Beacon";
            temp.caption = "Beacon";
            temp.command = Commands.LightsBeaconLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                var builder = new BitmapBuilder(size);

                if(this.beacon == false)
                {
                    builder.Clear(BitmapColor.White);
                    builder.DrawText("Beacon \r\n off", BitmapColor.Black);
                }
                else
                {
                    builder.Clear(new BitmapColor(255,251,0));
                    builder.DrawText("Beacon \r\n on", BitmapColor.Black);
                }
                return builder.ToImage();
                //sim/cockpit/electrical/beacon_lights_on
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Landing";
            temp.caption = "Landing";
            temp.command = Commands.LightsLandingLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                var builder = new BitmapBuilder(size);

                if (this.landing == false)
                {
                    builder.Clear(BitmapColor.White);
                    builder.DrawText("Landing \r\n off", BitmapColor.Black);
                }
                else
                {
                    builder.Clear(new BitmapColor(255, 251, 0));
                    builder.DrawText("Landing \r\n on", BitmapColor.Black);
                }
                return builder.ToImage();
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Taxi";
            temp.caption = "Taxi";
            temp.command = Commands.LightsTaxiLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                var builder = new BitmapBuilder(size);

                if (this.taxi == false)
                {
                    builder.Clear(BitmapColor.White);
                    builder.DrawText("Taxi \r\n off", BitmapColor.Black);
                }
                else
                {
                    builder.Clear(new BitmapColor(255, 251, 0));
                    builder.DrawText("Taxi \r\n on", BitmapColor.Black);
                }
                return builder.ToImage();
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Nav";
            temp.caption = "Nav";
            temp.command = Commands.LightsNavLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                var builder = new BitmapBuilder(size);

                if (this.nav == false)
                {
                    builder.Clear(BitmapColor.White);
                    builder.DrawText("Nav \r\n off", BitmapColor.Black);
                }
                else
                {
                    builder.Clear(new BitmapColor(255, 251, 0));
                    builder.DrawText("Nav \r\n on", BitmapColor.Black);
                }
                return builder.ToImage();
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Strobe";
            temp.caption = "Strobe";
            temp.command = Commands.LightsStrobeLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                var builder = new BitmapBuilder(size);

                if (this.strobe == false)
                {
                    builder.Clear(BitmapColor.White);
                    builder.DrawText("Strobe \r\n off", BitmapColor.Black);
                }
                else
                {
                    builder.Clear(new BitmapColor(255, 251, 0));
                    builder.DrawText("Strobe \r\n on", BitmapColor.Black);
                }
                return builder.ToImage();
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            base.FillButtons();
        }
    }
}
