using System;
using System.Diagnostics;

using XPlaneConnector;
using XPlaneConnector.DataRefs;

namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class Electrics : TemplateFolder
    {
        public Electrics()
        {
            this.DisplayName = "Electrics";
            this.GroupName = "Electrics";
            this.Navigation = PluginDynamicFolderNavigation.None;
        }

        private string image { get; } = "battery.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        public int num_batteries;
        public int num_generators;

        public TypeClasses.Graph BatGraph = new TypeClasses.Graph(0,0,0,0,0,0);
        public TypeClasses.Graph BatAmp = new TypeClasses.Graph();
        public TypeClasses.Graph GenAmp = new TypeClasses.Graph();


        public override Boolean Activate() {
            while (!SupportClasses.AirplaneData.complete)
            {
                System.Threading.Thread.Sleep(10);
            }
            while (!SupportClasses.BoundariesData.complete)
            {
                System.Threading.Thread.Sleep(10);
            }
            num_batteries = Convert.ToInt32(SupportClasses.AirplaneData.getValue("num_batteries"));
            num_generators = Convert.ToInt32(SupportClasses.AirplaneData.getValue("num_generators"));

            BatGraph.red_lo = SupportClasses.BoundariesData.getValue("red_lo_BatVolts");
            BatGraph.yellow_lo = SupportClasses.BoundariesData.getValue("yellow_lo_BatVolts");
            BatGraph.green_lo = SupportClasses.BoundariesData.getValue("green_lo_BatVolts");
            BatGraph.green_hi = SupportClasses.BoundariesData.getValue("green_hi_BatVolts");
            BatGraph.yellow_hi = SupportClasses.BoundariesData.getValue("yellow_hi_BatVolts");
            BatGraph.red_hi = SupportClasses.BoundariesData.getValue("red_hi_BatVolts");
            BatGraph.init();

            //BatAmp.red_lo = SupportClasses.BoundariesData.getValue("red_lo_BatAmp");
            BatAmp.red_lo = SupportClasses.BoundariesData.getValue("red_hi_BatAmp")*-1;
            BatAmp.yellow_lo = BatAmp.red_hi * (float)0.2;
            BatAmp.green_lo = 0;
            BatAmp.yellow_hi = 0;
            BatAmp.red_hi = SupportClasses.BoundariesData.getValue("red_hi_BatAmp");
            BatAmp.green_hi = BatAmp.red_hi * (float)0.8;
            BatAmp.init();

            GenAmp.red_lo = SupportClasses.BoundariesData.getValue("red_lo_BatAmp");
            GenAmp.yellow_lo = BatAmp.red_hi * (float)0.2;
            GenAmp.green_lo = 0;
            GenAmp.yellow_hi = 0;
            GenAmp.red_hi = SupportClasses.BoundariesData.getValue("red_hi_BatAmp");
            GenAmp.green_hi = BatAmp.red_hi * (float)0.8;
            GenAmp.init();



            SupportClasses.SubscriptionHandler.OnValueChanged += this.SubscriptionHandler_OnValueChanged;
            return base.Activate();
        }

        private void SubscriptionHandler_OnValueChanged(TypeClasses.SubscriptionValue value)
        {
            if (value.displayName == this.DisplayName)
            {
                switch (value.dataRef.DataRef)
                {
                    case string s when s.Contains("sim/cockpit/electrical/battery_array_on"):
                        this._buttons[$"Battery {value.getIndex() + 1}"].value = value.value;
                        this.CommandImageChanged($"Battery {value.getIndex() + 1}");
                        break;
                    case string s when s.Contains("sim/cockpit/electrical/generator_on"):
                        this._buttons[$"Generator {value.getIndex() + 1}"].value = value.value;
                        this.CommandImageChanged($"Generator {value.getIndex() + 1}");
                        break;

                    case string s when s.Contains("sim/flightmodel/engine/ENGN_bat_volt"):
                        this._buttons[$"BatVolt {value.getIndex() + 1}"].value = value.value;
                        this.CommandImageChanged($"BatVolt {value.getIndex() + 1}");
                        break;
                    case string s when s.Contains("sim/flightmodel/engine/ENGN_bat_amp"):
                        this._buttons[$"BatAmp {value.getIndex() + 1}"].value = value.value;
                        this.CommandImageChanged($"BatAmp {value.getIndex() + 1}");
                        break;
                    case string s when s.Contains("sim/flightmodel/engine/ENGN_gen_amp"):
                        this._buttons[$"GenAmp {value.getIndex() + 1}"].value = value.value;
                        this.CommandImageChanged($"GenAmp {value.getIndex() + 1}");
                        break;

                    default:
                        break;

                }

                }
        }

        protected override void FillSubscriptions() {
            for (int i = 0; i < this.num_batteries; i++)
            {
                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/cockpit/electrical/battery_array_on[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });
            }

            for (int i = 0; i < this.num_generators; i++)
            {
                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/cockpit/electrical/generator_on[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });
            }

            for (int i = 0; i < this.num_batteries; i++)
            {
                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_bat_volt[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });
            }

            for (int i = 0; i < this.num_batteries; i++)
            {
                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_bat_amp[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });
            }

            for (int i = 0; i < this.num_generators; i++)
            {
                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_gen_amp[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });
            }

            base.FillSubscriptions();
        }

        protected override void FillButtons()
        {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            for (int i = 0; i < this.num_batteries; i++)
            {
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
                temp.id = $"Battery {i+1}";
                temp.caption = $"Battery {i+1}";
                temp.loop = i;
                temp.command = null;
                temp.RunCommand = (connector, btn)=>{
                    DataRefElement bat = new DataRefElement
                    {
                        DataRef = $"sim/cockpit/electrical/battery_array_on[{btn.loop}]",
                        Frequency = 5
                    };

                    if (Convert.ToBoolean(btn.value))
                    {
                        connector.SetDataRefValue(bat, 0);
                    }
                    else
                    {
                        connector.SetDataRefValue(bat, 1);
                    }
                    this.CommandImageChanged(temp.id);
                    

                };
                temp.GetImage = (size, btn) =>
                {


                    if (Convert.ToBoolean(btn.value))
                    {
                        return SupportClasses.ButtonImages.activeImage(size, btn.caption, "");
                    }
                    else
                    {
                        return SupportClasses.ButtonImages.standardImage(size, btn.caption, "");
                    }

                };

                this._buttons.TryAdd(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            }
            for (int i = 0; i < this.num_generators; i++)
            {
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
                temp.id = $"Generator {i+1}";
                temp.caption = $"Generator {i+1}";
                temp.loop = i;
                temp.command = null;
                temp.RunCommand = (connector, btn) => {
                    DataRefElement gen = new DataRefElement
                    {
                        DataRef = $"sim/cockpit/electrical/generator_on[{btn.loop}]",
                        Frequency = 5
                    };

                    if (Convert.ToBoolean(btn.value))
                    {
                        connector.SetDataRefValue(gen, 0);
                    }
                    else
                    {
                        connector.SetDataRefValue(gen, 1);
                    }
                    this.CommandImageChanged(temp.id);


                };
                temp.GetImage = (size, btn) =>
                {
                    if (Convert.ToBoolean(btn.value))
                    {
                        return SupportClasses.ButtonImages.activeImage(size, btn.caption, "");
                    }
                    else
                    {
                        return SupportClasses.ButtonImages.standardImage(size, btn.caption, "");
                    }
                };

                this._buttons.TryAdd(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            }

            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            for (int i = 0; i < this.num_batteries; i++)
            {
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
                temp.id = $"BatVolt {i + 1}";
                temp.caption = $"BatVolt {i + 1}";
                temp.loop = i;
                temp.command = Commands.NoneNone;
                temp.unit = "V";
                temp.format = "n1";
                temp.GetImage = (size, btn) =>
                {
                    return SupportClasses.ButtonImages._standardImageGraph(size, BitmapColor.White, BitmapColor.Black, $"Bat {btn.loop + 1}:\r\n{btn.getDisplayValue()}",BatGraph.getGraph(btn.value));

                };
                this._buttons.TryAdd(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            }

            for (int i = 0; i < this.num_batteries; i++)
            {
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
                temp.id = $"BatAmp {i + 1}";
                temp.caption = $"BatAmp {i + 1}";
                temp.loop = i;
                temp.command = Commands.NoneNone;
                temp.unit = "Amp";
                temp.format = "n1";
                temp.GetImage = (size, btn) =>
                {
                    return SupportClasses.ButtonImages._standardImageGraph(size, BitmapColor.White, BitmapColor.Black, $"Bat {btn.loop + 1}:\r\n{btn.getDisplayValue()}", BatAmp.getGraph(btn.value));

                };
                this._buttons.TryAdd(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            }

            for (int i = 0; i < this.num_generators; i++)
            {
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
                temp.id = $"GenAmp {i + 1}";
                temp.caption = $"GenAmp {i + 1}";
                temp.loop = i;
                temp.command = Commands.NoneNone;
                temp.unit = "Amp";
                temp.format = "n1";
                temp.GetImage = (size, btn) =>
                {
                    return SupportClasses.ButtonImages._standardImageGraph(size, BitmapColor.White, BitmapColor.Black, $"Gen {btn.loop + 1}:\r\n{btn.getDisplayValue()}", GenAmp.getGraph(btn.value));

                };
                this._buttons.TryAdd(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            }




            base.FillButtons();
        }



    }
}
