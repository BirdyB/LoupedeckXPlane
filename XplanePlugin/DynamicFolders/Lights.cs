using System;

using XPlaneConnector;
using XPlaneConnector.DataRefs;

namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class Lights : TemplateFolder
    {
        public Lights()
        {
            this.DisplayName = "Lights";
            this.GroupName = "X-Plane";
        }

        private string image { get; } = "lightbulb.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        public override Boolean Activate() {

            SupportClasses.SubscriptionHandler.OnValueChanged += this.SubscriptionHandler_OnValueChanged;

            return base.Activate();
        }

        private void SubscriptionHandler_OnValueChanged(TypeClasses.SubscriptionValue value)
        {
            if (value.displayName == this.DisplayName)
            {
                switch (value.dataRef.DataRef)
                {
                    //case string s when s.Contains("sim/cockpit/electrical/battery_array_on"):
                    //    this._buttons[$"Battery {value.getIndex() + 1}"].value = value.value;
                    //    this.CommandImageChanged($"Battery {value.getIndex() + 1}");
                    //    break;

                    case string s when s.Contains("sim/cockpit/electrical/beacon_lights_on"):
                        this._buttons[$"Beacon"].value = value.value;
                        this.CommandImageChanged($"Beacon");
                        break;

                    case string s when s.Contains("sim/cockpit/electrical/landing_lights_on"):
                        this._buttons[$"Landing"].value = value.value;
                        this.CommandImageChanged($"Landing");
                        break;

                    case string s when s.Contains("sim/cockpit/electrical/taxi_light_on"):
                        this._buttons[$"Taxi"].value = value.value;
                        this.CommandImageChanged($"Taxi");
                        break;

                    case string s when s.Contains("sim/cockpit/electrical/nav_lights_on"):
                        this._buttons[$"Nav"].value = value.value;
                        this.CommandImageChanged($"Nav");
                        break;

                    case string s when s.Contains("sim/cockpit/electrical/strobe_lights_on"):
                        this._buttons[$"Strobe"].value = value.value;
                        this.CommandImageChanged($"Strobe");
                        break;

                    default:
                        break;
                }


                }
        }

        protected override void FillSubscriptions()
        {
            SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
            {
                displayName = this.DisplayName,
                dataRef = new DataRefElement
                {
                    DataRef = "sim/cockpit/electrical/beacon_lights_on",
                    Frequency = 5
                }
            });

            SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
            {
                displayName = this.DisplayName,
                dataRef = new DataRefElement
                {
                    DataRef = "sim/cockpit/electrical/landing_lights_on",
                    Frequency = 5
                }
            });

            SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
            {
                displayName = this.DisplayName,
                dataRef = new DataRefElement
                {
                    DataRef = "sim/cockpit/electrical/taxi_light_on",
                    Frequency = 5
                }
            });
            SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
            {
                displayName = this.DisplayName,
                dataRef = new DataRefElement
                {
                    DataRef = "sim/cockpit/electrical/nav_lights_on",
                    Frequency = 5
                }
            });

            SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
            {
                displayName = this.DisplayName,
                dataRef = new DataRefElement
                {
                    DataRef = "sim/cockpit/electrical/strobe_lights_on",
                    Frequency = 5
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


                if(btn.value == 0)
                {
                    return SupportClasses.ButtonImages.customImage(size, "Beacon \r\n off", "", BitmapColor.White, BitmapColor.Black);
                }
                else
                {
                    return SupportClasses.ButtonImages.customImage(size, "Beacon \r\n on", "", new BitmapColor(255,251,0), BitmapColor.Black);
                }

            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Landing";
            temp.caption = "Landing";
            temp.command = Commands.LightsLandingLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                if (btn.value == 0)
                {
                    return SupportClasses.ButtonImages.customImage(size, "Landing \r\n off", "", BitmapColor.White, BitmapColor.Black);
                }
                else
                {
                    return SupportClasses.ButtonImages.customImage(size, "Landing \r\n on", "", new BitmapColor(255, 251, 0), BitmapColor.Black);
                }
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Taxi";
            temp.caption = "Taxi";
            temp.command = Commands.LightsTaxiLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                if (btn.value == 0)
                {
                    return SupportClasses.ButtonImages.customImage(size, "Taxi \r\n off", "", BitmapColor.White, BitmapColor.Black);
                }
                else
                {
                    return SupportClasses.ButtonImages.customImage(size, "Taxi \r\n on", "", new BitmapColor(255, 251, 0), BitmapColor.Black);
                }
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Nav";
            temp.caption = "Nav";
            temp.command = Commands.LightsNavLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                if (btn.value == 0)
                {
                    return SupportClasses.ButtonImages.customImage(size, "Nav \r\n off", "", BitmapColor.White, BitmapColor.Black);
                }
                else
                {
                    return SupportClasses.ButtonImages.customImage(size, "Nav \r\n on", "", new BitmapColor(255, 251, 0), BitmapColor.Black);
                }
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            temp.id = "Strobe";
            temp.caption = "Strobe";
            temp.command = Commands.LightsStrobeLightsToggle;

            temp.GetImage = (size, btn) =>
            {
                if (btn.value == 0)
                {
                    return SupportClasses.ButtonImages.customImage(size, "Strobe \r\n off", "", BitmapColor.White, BitmapColor.Black);
                }
                else
                {
                    return SupportClasses.ButtonImages.customImage(size, "Strobe \r\n on", "", new BitmapColor(255, 251, 0), BitmapColor.Black);
                }
            };

            this._buttons.TryAdd(temp.id, temp);
            temp = new TypeClasses.Button();

            base.FillButtons();
        }
    }
}
