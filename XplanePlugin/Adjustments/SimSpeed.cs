using System;
using System.Diagnostics;

using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.Adjustments
{
    public class SimSpeed : PluginDynamicAdjustment
    {
        public SimSpeed() : base("SimSpeed", "Sets Sim Speed", "Sim", true)
        {
            connector.Start();
        }

        public XPlaneConnector.XPlaneConnector connector = new XPlaneConnector.XPlaneConnector();

        public float _simSpeed = 1;
        public float _simSpeedActual;

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            try
            {
                this._simSpeed += Convert.ToSingle(ticks)/100; // increase or decrease counter on the number of ticks
                this.connector.SetDataRefValue(new DataRefElement { DataRef = "sim/time/sim_speed", Frequency = 5 }, this._simSpeed);
                this.AdjustmentValueChanged();
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

        protected override void RunCommand(String actionParameter)
        {
            try
            {
                this._simSpeed = 1; // reset counter to 0
                this.connector.SetDataRefValue(new DataRefElement { DataRef = "sim/time/sim_speed", Frequency = 5 }, this._simSpeed);
                this.AdjustmentValueChanged();
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

        protected override String GetAdjustmentValue(String actionParameter)
        {
            string output = "";
            try
            {
                this.connector.Subscribe(DataRefs.TimeSimSpeed, 5, (e, v) =>
                {
                    this._simSpeed = v;

                });
                this.connector.Subscribe(DataRefs.TimeSimSpeedActual, 5, (e, v) =>
                {
                    this._simSpeedActual = v;

                });
                output = this._simSpeed.ToString("n2") + "\r\n" + this._simSpeedActual.ToString("n2");
                return output;
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                Debug.WriteLine($"{e.Message} - {e.Data} - {e.Source} - {frame} - {line}");
                return "Error";
            }
        }
    }
}
