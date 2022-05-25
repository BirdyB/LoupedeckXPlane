using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Loupedeck.XplanePlugin.TypeClasses;
using Loupedeck.XplanePlugin.SupportClasses;


using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class TemplateFolder : PluginDynamicFolder
    {
        //X-Plane-Connector
        public XPlaneConnector.XPlaneConnector connector = SupportClasses.ConnectorHandler.connector;

        //Dictionarys for Adjustments, Buttons and Subscriptions
        //protected IDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment> _adjustments = new Dictionary<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment>();
        //protected IDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button> _buttons = new Dictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button>();
        //protected IDictionary<XPlaneConnector.DataRefElement, Action<DataRefElement, float>> _subscriptions = new Dictionary<XPlaneConnector.DataRefElement, Action<DataRefElement, float>>();

        protected ConcurrentDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment> _adjustments = new ConcurrentDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment>();
        protected ConcurrentDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button> _buttons = new ConcurrentDictionary<string, Loupedeck.XplanePlugin.TypeClasses.Button>();
        protected ConcurrentDictionary<XPlaneConnector.DataRefElement, Action<DataRefElement, float>> _subscriptions = new ConcurrentDictionary<XPlaneConnector.DataRefElement, Action<DataRefElement, float>>();

        //Variables for specific Page
        private float _multiplier = 1;

        public TemplateFolder()
        {
            this.DisplayName = "Template";
            this.GroupName = "Template";
            this.Navigation = PluginDynamicFolderNavigation.ButtonArea;

        }

        private string image { get; } = "aircraft.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }


        public override Boolean Load()
        {
            try
            {
                this.connector.Start();
                
            }catch(Exception e)
            {
                SupportClasses.DebugClass.ExceptionReceived(e);
            }
            return base.Load();
        }

        public override Boolean Activate() {
            this.connector.Start();
            this.FillSubscriptions();
            if (this._subscriptions.Count > 0)
            {
                foreach (KeyValuePair<XPlaneConnector.DataRefElement, Action<DataRefElement, float>> pair in this._subscriptions)
                {
                    this.connector.Subscribe(pair.Key, 5, pair.Value);

                }
            }

            this.FillAdjustments();
            if (this._adjustments.Count > 0)
            {
                foreach (KeyValuePair<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment> pair in this._adjustments)
                {
                    if (pair.Value.element != null && !this._adjustments.ContainsKey(pair.Key))
                    {
                        this.connector.Subscribe(pair.Value.element, 5, (e, v) =>
                        {
                            if (this._adjustments[pair.Key].freq != v)
                            {
                                this._adjustments[pair.Key].freq = v;
                                //Debug.WriteLine("Update für Key " + pair.Key.ToString() + " mit Wert " + v.ToString());
                                this.AdjustmentValueChanged(pair.Key);
                            }
                        });
                    }
                }
            }

            this.FillButtons();
            this.EncoderActionNamesChanged();
            this.ButtonActionNamesChanged();
            return base.Activate();
        }

        private void Connector_OnRawReceive(String raw)
        {
            //Debug.WriteLine(DateTime.Now + " - " + raw);
        }

        private void Connector_OnLog(String message)
        {
            // Debug.WriteLine(message);
        }

        protected virtual void FillSubscriptions()
        {
            //Determine number of Engines
            //this._subscriptions.Add(DataRefs.AircraftEngineAcfNumEngines, (e, v) =>
            //{
            //    this._engines = v;
            //    this.FirstValuesSet();
            //});
        }

        protected virtual void FillAdjustments()
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
            //    this._adjustments.TryAdd(temp.id, temp);
            //    temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            //}

        }

        protected virtual void FillButtons()
        {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new TypeClasses.Button();

            if (!this._buttons.ContainsKey("Multiplier"))
            {
                temp.id = "Multiplier";
                temp.command = Commands.NoneNone;
                temp.caption = "Multiplier";
                temp.sticky = true;
                temp.bgcolor = BitmapColor.White;
                temp.textcolor = BitmapColor.Black;
                temp.RunCommand = (connector, command) =>
                {
                    switch (this._multiplier)
                    {
                        case 1:
                            this._multiplier = 10;
                            break;
                        case 10:
                            this._multiplier = 100;
                            break;
                        case 100:
                            this._multiplier = 1;
                            break;
                        default:
                            this._multiplier = 1;
                            break;
                    }

                };
                temp.GetImage = (imageSize, button) =>
                {
                    return SupportClasses.ButtonImages.standardImage(imageSize, button.caption + "\r\n" + this._multiplier, "");
                };
                this._buttons.TryAdd(temp.id, temp);
                temp = new TypeClasses.Button();
            }

        }

        private XPlaneConnector.DataRefElement CreateDataRef(string DataRef, int freq)
        {
            try
            {
                XPlaneConnector.DataRefElement temp = new XPlaneConnector.DataRefElement
                {
                    DataRef = DataRef,
                    Frequency = freq
                };

                return temp;
            }
            catch
            {
                return null;
            }
        }


        public override IEnumerable<String> GetEncoderRotateActionNames()
        {
       
                List<string> freq = new List<string>();
            try
            {
                var temp = this._adjustments.OrderBy(x => x.Value.prio);
                //this._adjustments = temp.ToDictionary(pair2 => pair2.Key, pair2 => pair2.Value);
                //var temp2 = this._adjustments.ToList();

                //foreach (KeyValuePair<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment> pair in this._adjustments)
                foreach (KeyValuePair<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment> pair in temp)
                {
                    freq.Add(this.CreateAdjustmentName(pair.Key));
                }
                
            }
            catch(Exception e)
            {
                SupportClasses.DebugClass.ExceptionReceived(e);

            }
            return freq;
        }

        public override IEnumerable<String> GetEncoderPressActionNames()
        {

                List<string> freq = new List<string>();
            try
            {
                foreach (KeyValuePair<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment> pair in this._adjustments)
                {
                    freq.Add(this.CreateCommandName(pair.Key));
                }

            } catch (Exception e)
            {
                SupportClasses.DebugClass.ExceptionReceived(e);
            }
            return freq;
        }


        public override IEnumerable<string> GetButtonPressActionNames()
        {

                List<string> buttons = new List<string>();
            try
            {

                var btnUp = new Loupedeck.XplanePlugin.TypeClasses.Button
                {
                    id = PluginDynamicFolder.NavigateUpActionName,
                    GetImage = (size, btn) => {
                        return SupportClasses.ButtonImages.standardImage(size, "Up", "");
                    }
                    
                };

                var btnLeft = new Loupedeck.XplanePlugin.TypeClasses.Button
                {
                    id = PluginDynamicFolder.NavigateLeftActionName
                };

                var btnRight = new Loupedeck.XplanePlugin.TypeClasses.Button
                {
                    id = PluginDynamicFolder.NavigateRightActionName
                };

                var btnEmpty = new Loupedeck.XplanePlugin.TypeClasses.Button
                {
                    id = "Empty",
                    bgcolor = BitmapColor.Black,
                    textcolor = BitmapColor.Black,
                    GetImage = (size, btn) =>
                    {
                        var builder = new BitmapBuilder(size);
                        builder.Clear(BitmapColor.Black);
                        return builder.ToImage();
                    }


                };



                var kpUp = new KeyValuePair<string, TypeClasses.Button>(btnUp.id, btnUp);
                var kpLeft = new KeyValuePair<string, TypeClasses.Button>(btnLeft.id, btnLeft);
                var kpRight = new KeyValuePair<string, TypeClasses.Button>(btnRight.id, btnRight);
                var kpEmptyOrSticky = new KeyValuePair<string, TypeClasses.Button>(btnEmpty.id, btnEmpty);
                var kpSticky = new KeyValuePair<string, TypeClasses.Button>("", null);


                foreach (TypeClasses.Button btn in _buttons.Values)
                {
                    if (btn.sticky == true)
                    {
                        kpEmptyOrSticky = new KeyValuePair<string, TypeClasses.Button>(btn.id, btn);
                        break;
                    }
                }


                foreach (var btn in this._buttons)
                {
                    if (btn.Value.hideFromOverview == true & btn.Value.sticky == true)
                    {
                        //this._buttons.Remove(btn.Key); //Ggf. Prio ändern, wenn Button nicht dargestellt wird.
                        btn.Value.prio = 9999;
                    }
                }

                //var temp = this._buttons.OrderBy(x => x.Value.prio);
                //this._buttons = new ConcurrentDictionary<string, Button>(temp.ToDictionary(pair2 => pair2.Key, pair2 => pair2.Value));
                var temp3 = this._buttons.OrderBy(x => x.Value.prio);
                //var temp2 = this._buttons.ToList();
                var temp2 = temp3.ToList();

                int ctr = 0;
                do
                {
                    temp2.Insert(ctr, kpUp);
                    temp2.Insert(ctr + 1, kpEmptyOrSticky);
                    temp2.Insert(ctr + 2, kpLeft);
                    temp2.Insert(ctr + 3, kpRight);
                    ctr = ctr + 12;
                } while (ctr < temp2.Count);


                //foreach (KeyValuePair<string, Loupedeck.XplanePlugin.TypeClasses.Button> pair in this._buttons)
                foreach (KeyValuePair<string, Loupedeck.XplanePlugin.TypeClasses.Button> pair in temp2)
                {
                    switch (pair.Key)
                    {
                        case string s when s.Equals(PluginDynamicFolder.NavigateLeftActionName):
                            buttons.Add(PluginDynamicFolder.NavigateLeftActionName);
                            break;
                        case string s when s.Equals(PluginDynamicFolder.NavigateRightActionName):
                            buttons.Add(PluginDynamicFolder.NavigateRightActionName);
                            break;
                        case string s when s.Equals(PluginDynamicFolder.NavigateUpActionName):
                            buttons.Add(PluginDynamicFolder.NavigateUpActionName);
                            break;
                        default:
                            buttons.Add(this.CreateCommandName(pair.Key));
                            break;
                    }
                    //buttons.Add(this.CreateCommandName(pair.Key));
                }

            }catch(Exception e)
            {
                string message = $"Class: {this.GetType().Name}";
                SupportClasses.DebugClass.ExceptionReceived(e, message);
            }
            return buttons;
        }


        public override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter == "Empty")
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(BitmapColor.Transparent);
                return builder.ToImage();
            }
            if (actionParameter == "")
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(BitmapColor.Transparent);
                return builder.ToImage();
            }
            if (this._buttons[actionParameter].GetImage(imageSize, this._buttons[actionParameter]) != null)
            {
                return this._buttons[actionParameter].GetImage(imageSize, this._buttons[actionParameter]);
            } else
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(BitmapColor.Transparent);
                return builder.ToImage();
            }

        }

        public override String GetAdjustmentValue(String actionParameter)
        {
                return this._adjustments[actionParameter].getValue(this._adjustments[actionParameter]);
        }

        public override BitmapImage GetAdjustmentImage(String actionParameter, PluginImageSize imageSize) {

            return this._adjustments[actionParameter].GetImage(imageSize, this._adjustments[actionParameter]);
        }

        public override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            this._adjustments[actionParameter].apply(this.connector, diff, this._multiplier, this._adjustments[actionParameter]);
            this.AdjustmentValueChanged(actionParameter);

        }

        public override void RunCommand(String commandParameter)
        {
            Task.Run(() =>
            {
                try
                {
                    Debug.WriteLine($"{DateTime.Now} - Run Command for btn {commandParameter}");
                    if (this._adjustments.ContainsKey(commandParameter))
                    {
                        this.connector.SendCommand(this._adjustments[commandParameter].command);
                        this.AdjustmentValueChanged(commandParameter);
                    }

                    if (this._buttons.ContainsKey(commandParameter))
                    {
                        this._buttons[commandParameter].RunCommand(this.connector, this._buttons[commandParameter]);
                        this.CommandImageChanged(commandParameter);

                    }
                }
                catch (Exception e)
                {
                    string message = $"Class: {this.GetType().Name} - command parameter: {commandParameter}";
                    SupportClasses.DebugClass.ExceptionReceived(e, message);
                }
            });
        }

        public Loupedeck.XplanePlugin.TypeClasses.Adjustment getEmptyAdjustment(string id1)
        {
            var temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment {
                id = id1
            };
        
            return temp;
        }
    }
}