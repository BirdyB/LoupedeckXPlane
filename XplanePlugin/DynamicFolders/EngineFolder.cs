using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;


using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class EngineFolder : TemplateFolder
    {
        //Variables for specific Page
        private float _engines;
        private float[] _engineRunning = new float[8];
        private bool[] _starterEngaged = new bool[8];
        private System.Threading.CancellationTokenSource[] token = new System.Threading.CancellationTokenSource[8];

        public delegate void FirstValueReceive();
        public event FirstValueReceive OnFirstValueReceive;

        public EngineFolder()
        {
            this._FirstValueReceived = false;
            this.DisplayName = "Engine";
            this.GroupName = "X-Plane";
            OnFirstValueReceive += this.EngineFolder_OnFirstValueReceive;

        }

        private string image { get; } = "propeller.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        private void EngineFolder_OnFirstValueReceive()
        {
            this._FirstValueReceived = true;
        }

        private void Connector_OnRawReceive(String raw)
        {
            Debug.WriteLine(DateTime.Now + " - " + raw);
        }

        private void Connector_OnDataRefReceived(DataRefElement dataRef)
        {
            Debug.WriteLine($"{DateTime.Now} - Received Data for {dataRef.DataRef} with value {dataRef.Value}");
        }


        public override Boolean Load()
        {
            //this.connector.OnLog += this.Connector_OnLog;
            //this.connector.OnDataRefReceived += this.Connector_OnDataRefReceived;
            //this.connector.OnRawReceive += this.Connector_OnRawReceive;
            return base.Load();
        }

        private void Connector_OnLog(String message)
        {
            Debug.WriteLine(message);
        }

        protected override void FillSubscriptions()
        {
            //Determine number of Engines
            this._subscriptions.Add(DataRefs.AircraftEngineAcfNumEngines, (e, v) =>
             {
                 if (this._engines != v)
                 {
                     this._engines = v;
                     OnFirstValueReceive?.Invoke();
                 }
             });

            for (int i = 0; i < 8; i++)
            {
                string temp = $"sim/flightmodel/engine/ENGN_running[{i}]";
                DataRefElement tempelement = new DataRefElement { DataRef=temp,
                    Frequency = 5 };
                this._subscriptions.Add(tempelement, (e, v) =>
                  {
                      if (this._engineRunning[i] != v)
                      {
                          this._engineRunning[i] = v;
                          if (this._engineRunning[i] == 1)
                          {
                              this.token[i].Cancel();
                          }
                          this.CommandImageChanged($"Igniter {i}");
                      }
                  }
                );
                temp = "";
                tempelement = new DataRefElement();
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

            for (int i = 0; i < this._engines; i++)
            {
                string engineref = $"sim/flightmodel/engine/ENGN_thro[{i}]";
                XPlaneConnector.DataRefElement element = new DataRefElement { DataRef = engineref, Frequency= 5 };
                try
                {
                    temp.id = $"Engine {i}";
                    temp.showPicture = false;
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
            for (int i = 0; i < this._engines; i++)
            {
                string engineref = $"sim/flightmodel/engine/ENGN_mixt[{i}]";
                XPlaneConnector.DataRefElement element = new DataRefElement { DataRef = engineref, Frequency = 5 };
                try
                {
                    temp.id = $"Mixture {i}";
                    temp.showPicture = false;
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

        }

        protected override void FillButtons()
        {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new TypeClasses.Button();
            for (int i = 0; i < this._engines; i++)
            {
                Debug.WriteLine($"Creating Button for Engine {i}");
                string engineref = $"sim/cockpit/engine/igniters_on[{i}]";
           
                XPlaneConnector.DataRefElement element = new DataRefElement { DataRef = engineref, Frequency = 5 };
                try
                {
                    temp.loop = i;
                    temp.id = $"Igniter {temp.loop}";
                    temp.command = null;
                    temp.caption = $"Igniter {temp.loop}";
                    temp.bgcolor = BitmapColor.White;
                    temp.textcolor = BitmapColor.Black;
                    temp.RunCommand = (connector, command) =>
                    {
                        if (this._engineRunning[temp.loop] == 0)
                        {
                            this.igniteEngine(temp.loop);
                        }
                    };
                    temp.GetImage = (imageSize, button) =>
                    {
                        this.connector.Subscribe(new DataRefElement { DataRef = $"sim/flightmodel/engine/ENGN_running[{temp.loop}]", Frequency= 5 }, 5, (e, v) =>
                            {
                                if (this._engineRunning[temp.loop] != v)
                                {
                                    this._engineRunning[temp.loop] = v;
                                    this.CommandImageChanged(temp.id);
                                }
                            });
                        var builder = new BitmapBuilder(imageSize);
                        var red = new BitmapColor(100, 0, 0);
                        var green = new BitmapColor(0, 100, 0);
                        if (this._engineRunning[temp.loop] == 1)
                        {
                            builder.Clear(green);
                        }
                        else
                        {
                            builder.Clear(red);
                        }

                        builder.DrawText(button.caption + "\r\n" + this._engineRunning[temp.loop].ToString(), button.textcolor);

                        return builder.ToImage();

                    };
                    this._buttons.Add(temp.id, temp);

                }
                catch
                {

                }
                temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            }






        }


        private void FirstValuesSet()
        {
            this._FirstValueReceived = true;
            Debug.WriteLine("Erste Subscription erhalten");
        }


        private void igniteEngine(int engine)
        {
            engine--;
            Debug.WriteLine($"Igniting engine {engine.ToString()}");
            if (!this._starterEngaged[engine])
            {
                this._starterEngaged[engine] = true;
                System.Threading.Tasks.Task.Run(() =>
                {
                    int numberofcomm = 0;
                    switch (engine)
                    {
                        case 0:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter1);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        case 1:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter2);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        case 2:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter3);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        case 3:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter4);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        case 4:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter5);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        case 5:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter6);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        case 6:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter7);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        case 7:
                            while (this._engineRunning[engine] != 1)
                            {
                                this.connector.SendCommand(Commands.StartersEngageStarter8);
                                System.Threading.Thread.Sleep(10);
                                numberofcomm++;
                                if (this.token[engine].IsCancellationRequested)
                                {
                                    Debug.WriteLine($"Starter release after {numberofcomm} Commands");
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }


                    this._starterEngaged[engine] = false;
                    Debug.WriteLine("Starter released");


                }, this.token[engine].Token);
                this.token[engine].CancelAfter(15000);
            }

        }
    }
}