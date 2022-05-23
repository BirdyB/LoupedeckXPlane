using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class AutopilotFolder : TemplateFolder
    {
        public AutopilotFolder()
        {
            this.DisplayName = "Autopilot";
            this.GroupName = "X-Plane";
            this.Navigation = PluginDynamicFolderNavigation.ButtonArea;
            OnAutopilotStateChange += this.AutopilotFolder_OnAutopilotStateChange;
        }

        private string image { get; } = "pin.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        private float _autopilotAct;
        private float _fdir1State;
        private float _autothrottleOn;
        private float _autothrottleMode;
        private float _autopilotState = 0;
        private bool[] _autopilotArray = new bool[24];

        public delegate void AutopilotStateChange(float state);
        public event AutopilotStateChange OnAutopilotStateChange;

        private void AutopilotFolder_OnAutopilotStateChange(float state)
        {
            int curpow;
            try
            {
                this.updateArray(state);
                for (int i = 0; i <= this._autopilotArray.Length; i++)
                {
                    Debug.WriteLine(i);
                    //Debug.WriteLine($"Index {i} - {this._autopilotArray[i]}");
                }
                Debug.WriteLine("Array fertig, Meister");

                for (int i = 0; i <= this._autopilotArray.Length; i++)
                {
                    curpow = 0;
                    curpow = Convert.ToInt32(Math.Pow(2, i));
                    var curitems = this._buttons.Where(item => item.Value.bincode.Equals(curpow)).Select(item => item.Key);
                    foreach (var curitem in curitems)
                    {
                        this.CommandImageChanged(curitem);
                    }

                }
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                Debug.WriteLine($"{e.Message} - {e.Data} - {e.Source} - {frame} - {line}");
            }
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

            if (!this._adjustments.ContainsKey("Speed"))
            {
                temp.id = "Speed";
                temp.element = DataRefs.CockpitAutopilotAirspeed;
                temp.command = Commands.AutopilotAutothrottleToggle;
                temp.format = "";
                temp.unit = "knots";
                temp.round = 0;
                temp.divider = 1;
                temp.showPicture = false;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("Heading"))
            {
                temp.id = "Heading";
                temp.element = DataRefs.CockpitAutopilotHeading;
                temp.command = Commands.AutopilotHeadingHold;
                temp.format = "N0";
                temp.unit = "";
                temp.divider = 1;
                temp.showPicture = false;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("Alt"))
            {
                temp.id = "Alt";
                temp.element = DataRefs.CockpitAutopilotAltitude;
                temp.command = Commands.AutopilotAltitudeHold;
                temp.format = "N0";
                temp.unit = "ft";
                temp.divider = 1;
                temp.showPicture = false;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("VSpeed"))
            {
                temp.id = "VSpeed";
                temp.element = DataRefs.CockpitAutopilotVerticalVelocity;
                temp.command = Commands.AutopilotVerticalSpeedPreSel;
                temp.format = "N0";
                temp.unit = "ft/min";
                temp.divider = 1;
                temp.showPicture = false;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            base.FillAdjustments();
        }

        protected override void FillButtons()
        {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new TypeClasses.Button();
            /*   if (!this._buttons.ContainsKey("NAVopen"))
               {
                   temp.id = "NAVopen";
                   temp.command = Commands.GPSG430n1Popup;
                   temp.caption = "GPS\r\nPopup";
                   temp.bgcolor = BitmapColor.White;
                   temp.image = "headphones.png";
                   temp.textcolor = BitmapColor.Black;

                   this._buttons.Add(temp.id, temp);
                   temp = new TypeClasses.Button();
               }

               */
            if (!this._buttons.ContainsKey("Servos"))
            {
                temp.id = "Servos";
                temp.sticky = true;
                temp.hideFromOverview = true;
                temp.command = Commands.AutopilotServosToggle;
                temp.caption = "Autopilot";
                temp.bgcolor = BitmapColor.White;
                temp.image = null;
                temp.textcolor = BitmapColor.Black;
                temp.GetImage = (size, btn) =>
                {
                    this.connector.Subscribe(DataRefs.CockpitAutopilotAutopilotState, 5, (e, v) =>
                    {
                        if (this._autopilotState != v)
                        {
                            this._autopilotState = v;
                            Debug.WriteLine(DateTime.Now + " - " + "Event APChanged fired with value: " + v);
                            OnAutopilotStateChange?.Invoke(v);
                        }
                    });

                    this.connector.Subscribe(DataRefs.CockpitAutopilotAutopilotMode, 5, (e, v) =>
                    {
                        if (this._autopilotAct != v)
                        {
                            this._autopilotAct = v;
                            CommandImageChanged(temp.id);
                        }

                    });


                    var builder = new BitmapBuilder(size);
                    if (this._autopilotAct != 0)
                    {
                        builder.Clear(new BitmapColor(0, 255, 0));
                        builder.DrawText("Autopilot\r\nOn", BitmapColor.White);
                    }
                    else
                    {
                        builder.Clear(new BitmapColor(255, 255, 255));
                        builder.DrawText("Autopilot\r\nOff", BitmapColor.Black);
                    }

                    return builder.ToImage();
                };

                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "TEST";
                temp.caption = "TEST";
                temp.command = Commands.NoneNone;
                temp.prio = 1;
                temp.RunCommand = (conn, btn) =>
                {
                    Debug.WriteLine("Setting Value");
                    conn.SetDataRefValue(new DataRefElement { DataRef = "sim/cockpit/autopilot/autopilot_state" }, 512);
                };

                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "AltVS";
                temp.caption = "VS Alt Arm";
                temp.bincode = 32;
                temp.command = Commands.AutopilotAltVs;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };

                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "AltArm";
                temp.caption = "Alt Arm";
                temp.command = Commands.AutopilotAltitudeArm;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };

                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "FDIR";
                temp.caption = "Flight \r\n Director";
                temp.command = Commands.AutopilotFdirToggle;
                temp.GetImage = (size, btn) =>
                {
                    this.connector.Subscribe(DataRefs.Cockpit2AutopilotFlightDirectorMode, 5, (e, v) =>
                    {
                        if (this._fdir1State != v)
                        {
                            this._fdir1State = v;
                            CommandImageChanged(temp.id);
                        }

                    });
                    var builder = new BitmapBuilder(size);
                    if (this._fdir1State != 0)
                    {
                        builder.Clear(new BitmapColor(0, 255, 0));
                        builder.DrawText($"Flight Dir\r\nOn({this._fdir1State})", BitmapColor.White);
                    }
                    else
                    {
                        builder.Clear(new BitmapColor(255, 255, 255));
                        builder.DrawText($"Flight Dir\r\nOff({this._fdir1State})", BitmapColor.Black);
                    }

                    return builder.ToImage();
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();



                temp.id = "HSINav1";
                temp.caption = "HSI\r\nNav1";
                temp.command = Commands.AutopilotHsiSelectNav1;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "HSINav2";
                temp.caption = "HSI\r\nNav2";
                temp.command = Commands.AutopilotHsiSelectNav2;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "HSIGps";
                temp.caption = "HSI\r\nGPS";
                temp.command = Commands.AutopilotHsiSelectGps;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "Autothrottle";
                temp.caption = "Autothrottle";
                temp.command = Commands.AutopilotAutothrottleToggle;
                temp.GetImage = (size, btn) =>
                {
                    this.connector.Subscribe(DataRefs.Cockpit2AutopilotAutothrottleOn, 5, (e, v) =>
                    {
                        if (this._autothrottleOn != v)
                        {
                            this._autothrottleOn = v;
                            CommandImageChanged(temp.id);
                        }

                    });

                    this.connector.Subscribe(DataRefs.Cockpit2AutopilotAutothrottleEnabled, 5, (e, v) =>
                    {
                        if (this._autothrottleMode != v)
                        {
                            this._autothrottleMode = v;
                            CommandImageChanged(temp.id);
                        }

                    });

                    var builder = new BitmapBuilder(size);
                    string text;
                    if (this._autothrottleOn == 1)
                    {
                        text = "Autothrottle On";
                    }
                    else
                    {
                        text = "Autothrottle Off";
                    }

                    switch (this._autothrottleMode)
                    {
                        case 0:
                            text += "\r\n Mode Off";
                            break;
                        case 1:
                            text += "\r\n Mode On";
                            break;
                        case 2:
                            text += "\r\n Mode N1EPR";
                            break;
                        default:
                            break;
                    }

                    if (this._autothrottleOn != 0)
                    {
                        builder.Clear(new BitmapColor(0, 255, 0));
                        builder.DrawText(text, BitmapColor.White);
                    }
                    else
                    {
                        builder.Clear(new BitmapColor(255, 255, 255));
                        builder.DrawText(text, BitmapColor.Black);
                    }

                    return builder.ToImage();
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "Autothrottle N1";
                temp.caption = "Autothrottle\r\nEPR/N1";
                temp.command = Commands.AutopilotAutothrottleN1eprToggle;
                temp.GetImage = (size, btn) =>
                {
                    this.connector.Subscribe(DataRefs.Cockpit2AutopilotAutothrottleEnabled, 5, (e, v) =>
                    {
                        if (this._autothrottleMode != v)
                        {
                            this._autothrottleMode = v;
                            CommandImageChanged(temp.id);
                        }

                    });
                    var builder = new BitmapBuilder(size);
                    if (this._autothrottleMode == 2)
                    {
                        builder.Clear(new BitmapColor(0, 255, 0));
                        builder.DrawText("Autothrottle\r\nEPR/N1 active", BitmapColor.White);
                    }
                    else
                    {
                        builder.Clear(new BitmapColor(255, 255, 255));
                        builder.DrawText("Autothrottle\r\nEPR/N1 inactive", BitmapColor.Black);
                    }
                    return builder.ToImage();
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "Heading Select";
                temp.caption = "Heading\r\nSelect";
                temp.command = Commands.AutopilotHeading;
                temp.bincode = 2;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "Heading Hold";
                temp.caption = "Heading\r\nHold";
                temp.bincode = 1048576;
                temp.command = Commands.AutopilotHeadingHold;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "AP Track";
                temp.caption = "Track\r\nSelect";
                temp.bincode = 4194304;
                temp.command = Commands.AutopilotTrack;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "HDG Nav";
                temp.caption = "HDG\r\nNAV";
                temp.command = Commands.AutopilotHdgNav;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "Nav";
                temp.caption = "NAV\r\nArm";
                temp.command = Commands.AutopilotNAV;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "VSI";
                temp.caption = "Keep\r\nVSI";
                temp.command = Commands.AutopilotVerticalSpeed;
                temp.bincode = 16;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "Glideslope";
                temp.caption = "Glide-\r\nslope";
                temp.command = Commands.AutopilotGlideSlope;
                temp.bincode = 1024; //add 2048 for engaged, 1024=armed
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "GPS Steering";
                temp.caption = "GPS\r\nSteering";
                temp.command = Commands.AutopilotGpss;
                temp.bincode = 524288;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "GPS Climb";
                temp.caption = "GPS\r\nClimb";
                temp.command = Commands.AutopilotClimb;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

                temp.id = "GPS Descend";
                temp.caption = "GPS\r\nDescend";
                temp.command = Commands.AutopilotDescend;
                temp.GetImage = (size, btn) =>
                {
                    return getAPImage(size, btn);
                };
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();

            }
            base.FillButtons();
        }

        protected override void FillSubscriptions()
        {
            this._subscriptions.Add(DataRefs.CockpitAutopilotAutopilotMode, (e, v) =>
            {
                if (this._autopilotAct != v)
                {
                    this._autopilotAct = v;
                    this.CommandImageChanged("Servos");
                }
            });

            this._subscriptions.Add(DataRefs.CockpitAutopilotAutopilotState, (e, v) =>
            {
                if (this._autopilotState != v)
                {
                    this._autopilotState = v;
                    Debug.WriteLine(DateTime.Now + " - " + "Event APChanged fired with value: " + v);
                    OnAutopilotStateChange?.Invoke(v);
                }
            });
            base.FillSubscriptions();
        }

        private IList<int> getPow2(float input)
        {
            IList<int> potenzen = new List<int>();
            int temp = 1;
            int temp2;
            string strhex = "";
            temp2 = Convert.ToInt32(input);
            strhex = Convert.ToString(temp2, 2);
            Debug.WriteLine("Binary: " + strhex);
            for (int i = 1; i <= strhex.Length; i++)
            {
                if ((strhex.Substring(strhex.Length - i, 1)) == "1")
                {
                    potenzen.Add(temp);
                    Debug.WriteLine(temp);
                }
                temp *= 2;
            }
            return potenzen;
        }

        private void updateArray(float input)
        {
            Debug.WriteLine("Y");
            int inputInt = Convert.ToInt32(input);
            string strhex = Convert.ToString(inputInt, 2);
            strhex = strhex.PadLeft(23, '0');
            int temp = 1;
            for (int i = 0; i <= strhex.Length; i++)
            {
                int j = (i + 1);
                if (j > strhex.Length)
                {
                    j = strhex.Length;
                }
                if ((strhex.Substring(strhex.Length - j, 1)) == "1")
                {
                    this._autopilotArray[i] = true;
                    Debug.WriteLine($"Durchlauf {i} - Potenz {temp} - Value {this._autopilotArray[i]}");
                }
                else
                {
                    this._autopilotArray[i] = false;
                    Debug.WriteLine($"Durchlauf {i} - Potenz {temp} - Value {this._autopilotArray[i]}");
                }
                temp *= 2;
            }
        }

        private BitmapImage getAPImage(PluginImageSize size, Loupedeck.XplanePlugin.TypeClasses.Button btn)
        {
            var builder = new BitmapBuilder(size);
            bool active = false;
            if (btn.bincode != 0)
            {
                int index = Convert.ToInt32(Math.Log(btn.bincode, 2));
                try
                {
                    active = this._autopilotArray[index];
                }
                catch { }

            }
            if (active)
            {
                builder.Clear(new BitmapColor(0, 255, 0));
                builder.DrawText(btn.caption, BitmapColor.White);
            }
            else
            {
                builder.Clear(btn.bgcolor);
                builder.DrawText(btn.caption, btn.textcolor);
            }

            return builder.ToImage();
        }

    }
}