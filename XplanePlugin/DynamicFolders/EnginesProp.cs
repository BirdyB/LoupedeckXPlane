using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using XPlaneConnector;
using XPlaneConnector.DataRefs;

namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class EnginesProp : TemplateFolder
    {
        public EnginesProp()
        {
            this.DisplayName = "Engine (Prop)";
            this.GroupName = "Engines";
            this.Navigation = PluginDynamicFolderNavigation.None;
        }

        private string image { get; } = "propeller.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }


        public override Boolean Load()
        {
            return true;
        }

        public override Boolean Activate()
        {

            Debug.WriteLine("Activation started");
            while (!SupportClasses.AirplaneData.complete)
            {
                System.Threading.Thread.Sleep(10);
            }
            num_engines = Convert.ToInt32(SupportClasses.AirplaneData.getValue("num_engines"));
            this.FillButtons();
            this.FillSubscriptions();
            this.ButtonActionNamesChanged();
            SupportClasses.SubscriptionHandler.OnValueChanged += this.SubscriptionHandler_OnValueChanged;

            return base.Activate();
        }

        private void SubscriptionHandler_OnValueChanged(TypeClasses.SubscriptionValue value)
        {
            if (value.displayName == this.DisplayName)
            {
                switch (value.dataRef.DataRef)
                {

                    case string s when s.Contains("sim/flightmodel/engine/ENGN_running"):
                        this._buttons[$"cmdEngine {value.getIndex() + 1}"].value = value.value;
                        if (this._starterEngaged[value.getIndex()] && Convert.ToBoolean(this._buttons[$"cmdEngine {value.getIndex() + 1}"].value))
                        {
                            this._starterToken.Cancel();
                        }
                        this.CommandImageChanged($"cmdEngine {value.getIndex() + 1}");
                        break;

                    case string s when s.Contains("sim/flightmodel/engine/ENGN_thro"):
                        this._adjustments[$"Engine {value.getIndex()}"].freq = value.value;
                        this._buttons[$"ShowThrottleEngine {value.getIndex() + 1}"].value = value.value;
                        this.CommandImageChanged($"ShowThrottleEngine {value.getIndex() + 1}");
                        this.AdjustmentValueChanged($"Engine {value.getIndex()}");
                        break;

                    case string s when s.Contains("sim/flightmodel/engine/ENGN_mixt"):
                        this._adjustments[$"Mixture {value.getIndex()}"].freq = value.value;
                        this.AdjustmentValueChanged($"Mixture {value.getIndex()}");
                        break;

                    case string s when s.Contains("sim/cockpit/engine/fuel_pump_on"):
                        this._buttons[$"PumpEngine {value.getIndex() + 1}"].value = value.value;
                        this.CommandImageChanged($"PumpEngine {value.getIndex() + 1}");
                        break;
                    case string s when s.Contains("sim/flightmodel/engine/ENGN_prop"):
                        this._adjustments[$"Prop {value.getIndex()}"].freq = value.value;
                        this.AdjustmentValueChanged($"Prop {value.getIndex()}");
                        break;

                    default:
                        break;

                }
            }
        }


        public int num_engines;
        private bool[] _btnShown = new bool[8] { false, false, false, false, false, false, false, false };
        private bool[] _starterEngaged = new bool[8] { false, false, false, false, false, false, false, false };
        System.Threading.CancellationTokenSource _starterToken;


        protected override void FillSubscriptions()
        {
            for (int i = 0; i < num_engines; i++)
            {
                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_running[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });

                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_thro[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });

                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_mixt[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });

                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/cockpit/engine/fuel_pump_on[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });

                SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
                {
                    dataRef = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_prop[{i}]",
                        Frequency = 5
                    },
                    displayName = this.DisplayName
                });
            }
            base.FillSubscriptions();
        }
        protected override void FillAdjustments()
        {
            Loupedeck.XplanePlugin.TypeClasses.Adjustment temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            //if (!this._adjustments.ContainsKey("Com1"))
            //{
            //    temp.id = "Com1";
            //    temp.element = DataRefs.CockpitRadiosCom1StdbyFreqHz;
            //    temp.command = Commands.RadiosCom1StandyFlip;
            //    temp.format = "n2";
            //    temp.unit = "Mhz";
            //    temp.divider = 100;
            //    this._adjustments.Add(temp.id, temp);
            //    temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            //}

            for (int i = 0; i < this.num_engines; i++)
            {
                string engineref = $"sim/flightmodel/engine/ENGN_thro[{i}]";
                XPlaneConnector.DataRefElement element = new DataRefElement { DataRef = engineref, Frequency = 5 };
                try
                {
                    temp.id = $"Engine {i}";
                    temp.caption = $"Throttle {i + 1}";
                    temp.showPicture = true;
                    temp.showText = false;
                    temp.element = element;
                    temp.command = null;
                    temp.format = "n0";
                    temp.unit = "%";
                    temp.minvalue = 0;
                    temp.maxvalue = 1;
                    temp.divider = 0.01F;
                    temp.setdivider = 100;
                    this._adjustments.Add(temp.id, temp);

                }
                catch
                {

                }
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            for (int i = 0; i < this.num_engines; i++)
            {
                string engineref = $"sim/flightmodel/engine/ENGN_mixt[{i}]";
                XPlaneConnector.DataRefElement element = new DataRefElement { DataRef = engineref, Frequency = 5 };
                try
                {
                    temp.id = $"Mixture {i}";
                    temp.caption = $"Mixture {i + 1}";
                    temp.showPicture = true;
                    temp.showText = false;
                    temp.element = element;
                    temp.command = null;
                    temp.format = "n0";
                    temp.unit = "%";
                    temp.minvalue = 0;
                    temp.maxvalue = 1;
                    temp.divider = 0.01F;
                    temp.setdivider = 100;
                    this._adjustments.Add(temp.id, temp);

                }
                catch
                {

                }
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            for (int i = 0; i < this.num_engines; i++)
            {
                string engineref = $"sim/flightmodel/engine/ENGN_prop[{i}]";
                XPlaneConnector.DataRefElement element = new DataRefElement { DataRef = engineref, Frequency = 5 };
                try
                {
                    temp.id = $"Prop {i}";
                    temp.caption = $"Prop {i + 1}";
                    temp.showPicture = true;
                    temp.showText = false;
                    temp.element = element;
                    temp.command = null;
                    temp.format = "n0";
                    temp.unit = "rad/sec";
                    temp.minvalue = 0;
                    temp.maxvalue = 300;
                    temp.divider = 1;
                    temp.setdivider = 1;
                    this._adjustments.Add(temp.id, temp);

                }
                catch
                {

                }
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }

        }

        protected override void FillButtons()
        {

            for (int i = 0; i < num_engines; i++)
            {
                string btnid = $"ShowThrottleEngine {i + 1}";

                TypeClasses.Button showThrottleBtn = new TypeClasses.Button
                {
                    id = btnid,
                    caption = $"Throttle {i + 1}",
                    mode = TypeClasses.Button.btnmode.display,
                    value = 0,
                    prio = (i * 10) - 100,
                    loop = i,
                    divider = 0.01F,
                    format = "n0",
                    unit = "%",
                    command = Commands.NoneNone,
                };
                if (!this._buttons.ContainsKey(showThrottleBtn.id))
                {
                    this._buttons.Add(showThrottleBtn.id, showThrottleBtn);
                }
                showThrottleBtn = new Loupedeck.XplanePlugin.TypeClasses.Button();
            }

            for (int i = 0; i < num_engines; i++)
            {
                string btnid = $"PumpEngine {i + 1}";

                TypeClasses.Button pumpBtn = new TypeClasses.Button
                {
                    id = btnid,
                    caption = $"Pump {i + 1}",
                    prio = (i * 10),
                    loop = i,
                    command = new XPlaneCommand($"sim/fuel/fuel_pump_{i + 1}_tog", ""),
                    GetImage = (imageSize, btn) =>
                    {
                        if (Convert.ToBoolean(btn.value))
                        {
                            return SupportClasses.ButtonImages.activeImage(imageSize, btn.caption, "");
                        }
                        else
                        {
                            return SupportClasses.ButtonImages.standardImage(imageSize, btn.caption, "");
                        }
                    }
                };
                if (!this._buttons.ContainsKey(pumpBtn.id))
                {
                    this._buttons.Add(pumpBtn.id, pumpBtn);
                }
                pumpBtn = new Loupedeck.XplanePlugin.TypeClasses.Button();
            }


            for (int i = 0; i < num_engines; i++)
            {
                Debug.WriteLine($"Erstelle Button für Engine {i + 1}");
                string btnid = $"cmdEngine {i + 1}";

                TypeClasses.Button engBtn = new TypeClasses.Button
                {
                    id = btnid,
                    caption = $"Engine {i + 1}",
                    prio = (i * 10) + 20,
                    loop = i,
                    command = Commands.NoneNone,
                    RunCommand = (conn, btn) =>
                    {
                        toggleEngineButtons(btn.loop, btn);
                    },
                    GetImage = (imageSize, btn) =>
                    {
                        if (Convert.ToBoolean(btn.value))
                        {
                            return SupportClasses.ButtonImages.activeImage(imageSize, btn.caption, "");
                        }
                        else
                        {
                            return SupportClasses.ButtonImages.standardImage(imageSize, btn.caption, "");
                        }
                    }
                };
                if (!this._buttons.ContainsKey(engBtn.id))
                {
                    this._buttons.Add(engBtn.id, engBtn);
                }
                engBtn = new Loupedeck.XplanePlugin.TypeClasses.Button();
            }

            var refresh = new TypeClasses.Button
            {
                id = "Refresh",
                caption = "Refresh",
                command = Commands.NoneNone,
                RunCommand = (conn, btn) =>
                {
                    SupportClasses.SubscriptionHandler.resetValues(this.DisplayName);
                    foreach (KeyValuePair<string, TypeClasses.Button> refreshbtn in this._buttons)
                    {
                        this.CommandImageChanged(refreshbtn.Key);
                    }
                }
            };
            if (!this._buttons.ContainsKey(refresh.id))
            {
                this._buttons.Add(refresh.id, refresh);
            }



            base.FillButtons();
        }

        private void toggleEngineButtons(int num, TypeClasses.Button parentBtn)
        {

            string btnid = "Eng" + num;
            string btnAddId;
            IDictionary<string, TypeClasses.Button> engineButtons = new Dictionary<string, TypeClasses.Button>(); //Dictionary for additional Buttons for each Engine

            btnAddId = btnid + "MagDown";
            engineButtons.Add(btnAddId, new TypeClasses.Button
            {
                id = btnAddId,
                caption = parentBtn.caption + "\r\n" + "Mag Down",
                prio = parentBtn.prio + 1,
                command = new XPlaneConnector.XPlaneCommand($"sim/magnetos/magnetos_down_{parentBtn.loop + 1}", $"Magneto {parentBtn.loop + 1} down"),
            });
            ;

            btnAddId = btnid + "MagUp";
            engineButtons.Add(btnAddId, new TypeClasses.Button
            {
                id = btnAddId,
                caption = parentBtn.caption + "\r\n" + "Mag Up",
                prio = parentBtn.prio + 2,
                command = new XPlaneConnector.XPlaneCommand($"sim/magnetos/magnetos_up_{parentBtn.loop + 1}", $"Magneto {parentBtn.loop + 1} up"),
            });

            btnAddId = btnid + "Ignition";
            engineButtons.Add(btnAddId, new TypeClasses.Button
            {
                id = btnAddId,
                caption = parentBtn.caption + "\r\n" + "Ignition",
                prio = parentBtn.prio + 3,
                command = null,
                GetImage = (imageSize, btn) =>
                {
                    if (this._starterEngaged[parentBtn.loop])
                    {
                        return SupportClasses.ButtonImages.activeImage(imageSize, btn.caption, "");
                    }
                    else
                    {
                        return SupportClasses.ButtonImages.standardImage(imageSize, btn.caption, "");
                    }

                },
                RunCommand = (conn, btn) =>
                {
                    XPlaneConnector.DataRefElement engRun = new DataRefElement
                    {
                        DataRef = $"sim/flightmodel/engine/ENGN_running[{parentBtn.loop}]",
                        Frequency = 5
                    };
                    XPlaneCommand engIgn = new XPlaneCommand($"sim/ignition/engage_starter_{parentBtn.loop + 1}", $"sim/ignition/engage_starter_{parentBtn.loop + 1}");
                    if (this._starterEngaged[parentBtn.loop])
                    {

                        this._starterToken.Cancel();
                        conn.Unsubscribe(engRun.DataRef);
                        this._starterEngaged[parentBtn.loop] = false;
                    }



                    if (!this._starterEngaged[parentBtn.loop])
                    {
                        this._starterEngaged[parentBtn.loop] = true;
                        this.CommandImageChanged(btn.id);
                        this._starterToken = new System.Threading.CancellationTokenSource();
                        var token = this._starterToken.Token;
                        Task.Run(() =>
                        {
                            while (true)
                            {
                                conn.SendCommand(engIgn);
                                System.Threading.Thread.Sleep(50);
                                if (this._starterToken.IsCancellationRequested)
                                {
                                    this._starterEngaged[parentBtn.loop] = false;
                                    this.CommandImageChanged(btn.id);
                                    break;
                                }
                            }

                        }, token);

                        this._starterToken.CancelAfter(10000);

                    }

                },
            });




            //Update Dictionary
            System.Threading.Thread.Sleep(20);
            this.ButtonActionNamesChanged();



            if (!_btnShown[num])
            {
                this._buttons = new Dictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button>();
                this.FillButtons();
                foreach (KeyValuePair<string, TypeClasses.Button> engineButton in engineButtons)
                {
                    if (!this._buttons.ContainsKey(engineButton.Value.id))
                    {
                        this._buttons.Add(engineButton.Key, engineButton.Value);
                    }
                }
                for (int i = 0; i < _btnShown.Length; i++)
                {
                    this._btnShown[i] = false;
                }

                _btnShown[num] = true;
                System.Threading.Thread.Sleep(20);

                this.ButtonActionNamesChanged();

                SupportClasses.SubscriptionHandler.resetValues(this.DisplayName);

            }
            else
            {
                this._buttons = new Dictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button>();
                this.FillButtons();
                _btnShown[num] = false;
                System.Threading.Thread.Sleep(20);
                this.ButtonActionNamesChanged();

                SupportClasses.SubscriptionHandler.resetValues(this.DisplayName);

            }
        }

    }

}

