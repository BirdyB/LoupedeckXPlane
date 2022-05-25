using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
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
            FuelLeft.red_lo = 0;
            FuelLeft.yellow_lo = 15;
            FuelLeft.green_lo = 15;
            FuelLeft.green_hi = 100;
            FuelLeft.yellow_hi = 100;
            FuelLeft.red_hi = 100;

            FuelRight.red_lo = 0;
            FuelRight.yellow_lo = 15;
            FuelRight.green_lo = 15;
            FuelRight.green_hi = 100;
            FuelRight.yellow_hi = 100;
            FuelRight.red_hi = 100;
            this.FuelLeft.init();
            this.FuelRight.init();


            return base.Activate();
        }

        private void SubscriptionHandler_OnValueChanged(TypeClasses.SubscriptionValue value)
        {
            if (value.displayName == this.DisplayName)
            {
                Debug.WriteLine($"{DateTime.Now} - Received update for Ref {value.dataRef.DataRef} with value {value.value}");
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

                    case string s when s.Contains("sim/cockpit2/fuel/fuel_level_indicated_left"):
                        this._buttons[$"FuelLeft"].value = value.value;
                        this.CommandImageChanged($"FuelLeft");
                        break;

                    case string s when s.Contains("sim/cockpit2/fuel/fuel_level_indicated_right"):
                        this._buttons[$"FuelRight"].value = value.value;
                        this.CommandImageChanged($"FuelRight");
                        break;

                    default:
                        break;

                }
            }
        }


        public int num_engines;
        private bool[] _btnShown = new bool[8] { false, false, false, false, false, false, false, false };
        private bool[] _starterEngaged = new bool[8] { false, false, false, false, false, false, false, false };
        Task toggleButtons;
        System.Threading.CancellationTokenSource _starterToken;
        public TypeClasses.Graph FuelLeft = new TypeClasses.Graph();
        public TypeClasses.Graph FuelRight = new TypeClasses.Graph();



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
            SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
            {
                dataRef = new DataRefElement
                {
                    DataRef = $"sim/cockpit2/fuel/fuel_level_indicated_left",
                    Frequency = 5
                },
                displayName = this.DisplayName
            });

            SupportClasses.SubscriptionHandler.subscribe(new TypeClasses.SubscriptionValue
            {
                dataRef = new DataRefElement
                {
                    DataRef = $"sim/cockpit2/fuel/fuel_level_indicated_right",
                    Frequency = 5
                },
                displayName = this.DisplayName
            });


            base.FillSubscriptions();
        }
        protected override void FillAdjustments()
        {
            Loupedeck.XplanePlugin.TypeClasses.Adjustment temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
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
                    this._adjustments.TryAdd(temp.id, temp);

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
                    this._adjustments.TryAdd(temp.id, temp);

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
                    this._adjustments.TryAdd(temp.id, temp);

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
                    this._buttons.TryAdd(showThrottleBtn.id, showThrottleBtn);
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
                    this._buttons.TryAdd(pumpBtn.id, pumpBtn);
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
                    this._buttons.TryAdd(engBtn.id, engBtn);
                }
                engBtn = new Loupedeck.XplanePlugin.TypeClasses.Button();

                var temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
                temp.id = $"FuelLeft";
                temp.caption = $"Fuel \r\n Left";
                temp.command = Commands.NoneNone;
                temp.unit = "%";
                temp.format = "n1";
                temp.GetImage = (size, btn) =>
                {
                    return SupportClasses.ButtonImages._standardImageGraph(size, BitmapColor.White, BitmapColor.Black, $"Fuel left:\r\n{btn.getDisplayValue()}", FuelLeft.getGraph(btn.value));

                };
                this._buttons.TryAdd(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

                temp.id = $"FuelRight";
                temp.caption = $"Fuel \r\n Right";
                temp.command = Commands.NoneNone;
                temp.unit = "%";
                temp.format = "n1";
                temp.GetImage = (size, btn) =>
                {
                    return SupportClasses.ButtonImages._standardImageGraph(size, BitmapColor.White, BitmapColor.Black, $"Fuel right:\r\n{btn.getDisplayValue()}", FuelRight.getGraph(btn.value));

                };
                this._buttons.TryAdd(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();


            }
            base.FillButtons();
        }




        private void toggleEngineButtons(int num, TypeClasses.Button parentBtn)
        {
            if(this.toggleButtons != null)
            {
                if (this.toggleButtons.Status == TaskStatus.Running)
                {
                    return;
                }
            }
            this.toggleButtons = Task.Run(() => {
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
                        XPlaneCommand engIgn = new XPlaneCommand($"sim/ignition/engage_starter_{parentBtn.loop + 1}", $"sim/ignition/engage_starter_{parentBtn.loop + 1}");
                        if (this._starterEngaged[parentBtn.loop])
                        {

                            this._starterToken.Cancel();
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

                            this._starterToken.CancelAfter(15000);

                        }

                    },
                });




                //Update Dictionary
                this.ButtonActionNamesChanged();



                if (!_btnShown[num])
                {
                    this._buttons = new ConcurrentDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button>();
                    this.FillButtons();
                    foreach (KeyValuePair<string, TypeClasses.Button> engineButton in engineButtons)
                    {
                        if (!this._buttons.ContainsKey(engineButton.Value.id))
                        {
                            this._buttons.TryAdd(engineButton.Key, engineButton.Value);
                        }
                    }
                    for (int i = 0; i < _btnShown.Length; i++)
                    {
                        this._btnShown[i] = false;
                    }

                    _btnShown[num] = true;
                    Debug.WriteLine("Extended Buttons");
                    foreach (TypeClasses.Button btn in this._buttons.Values)
                    {
                        Debug.WriteLine($"{btn.prio} {btn.id} - {btn.caption}");
                    }
                    SupportClasses.SubscriptionHandler.resetValues(this.DisplayName);

                    this.ButtonActionNamesChanged();
                    foreach (TypeClasses.Button btn in _buttons.Values)
                    {
                        this.CommandImageChanged(btn.id);
                    }



                }
                else
                {
                    this._buttons = new ConcurrentDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button>();
                    this.FillButtons();
                    Debug.WriteLine($"Standard-Buttons");
                    foreach (TypeClasses.Button btn in this._buttons.Values)
                    {
                        Debug.WriteLine($"{btn.prio} {btn.id} - {btn.caption}");
                    }
                    _btnShown[num] = false;
                    SupportClasses.SubscriptionHandler.resetValues(this.DisplayName);
                    this.ButtonActionNamesChanged();
                    foreach (TypeClasses.Button btn in _buttons.Values)
                    {
                        this.CommandImageChanged(btn.id);
                    }


                }
            });
        }


    }

}

