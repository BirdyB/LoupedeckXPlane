using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.SupportClasses
{
    public static class AirplaneData
    {
        public class PlaneValue
        {
            public string id { get; set; }
            public DataRefElement dataRef { get; set; }
            public bool isSet { get; set; } = false;
            public float value { get; set; } = 0;

        }

        public static bool complete = false;

        public delegate void receiveData(PlaneValue item);
        public static event receiveData OnReceiveData;

        public static List<PlaneValue> data = new List<PlaneValue>();

        private static XPlaneConnector.XPlaneConnector connector = new XPlaneConnector.XPlaneConnector();

        public static void init()
        {
            data.Add(new PlaneValue { id = "num_engines", dataRef = DataRefs.AircraftEngineAcfNumEngines});
            data.Add(new PlaneValue { id = "num_batteries", dataRef = DataRefs.AircraftElectricalNumBatteries});
            data.Add(new PlaneValue { id = "num_generators", dataRef = DataRefs.AircraftElectricalNumGenerators});
            data.Add(new PlaneValue { id = "num_inverters", dataRef = DataRefs.AircraftElectricalNumInverters });
            data.Add(new PlaneValue { id = "num_buses", dataRef = DataRefs.AircraftElectricalNumBuses});
            data.Add(new PlaneValue { id = "num_tanks", dataRef = DataRefs.AircraftOverflowAcfNumTanks });
            data.Add(new PlaneValue { id = "acf_en_type", dataRef = DataRefs.AircraftPropAcfEnType}); //engine type – read only in v11, but you should NEVER EVER write this in v10 or earlier. 0=recip carb, 1=recip injected, 2=free turbine, 3=electric, 4=lo bypass jet, 5=hi bypass jet, 6=rocket, 7=tip rockets, 8=fixed turbine
            data.Add(new PlaneValue { id = "acf_gear_retract", dataRef = DataRefs.AircraftGearAcfGearRetract });
            data.Add(new PlaneValue { id = "fdir_needed_to_engage_servos", dataRef = DataRefs.AircraftSystemsFdirNeededToEngageServos });
            data.Add(new PlaneValue { id = "battery_EQ", dataRef = DataRefs.CockpitElectricalBatteryEQ });
            data.Add(new PlaneValue { id = "avionics_EQ", dataRef = DataRefs.CockpitElectricalAvionicsEQ });
            data.Add(new PlaneValue { id = "generator_EQ", dataRef = DataRefs.CockpitElectricalGeneratorEQ });
            data.Add(new PlaneValue { id = "auto_fea_EQ", dataRef = DataRefs.AircraftEngineAcfAutoFeathereq});
        }

        public static void getData()
        {
            bool complete = false;
            AirplaneData.connector.Start();
            foreach (PlaneValue item in data)
            {
                AirplaneData.connector.Subscribe(item.dataRef, 5, (element, value) => {
                    AirplaneData.data.Where(item2 => item2.id == item.id).FirstOrDefault().value = value;
                    AirplaneData.data.Where(item2 => item2.id == item.id).FirstOrDefault().isSet = true;
                    OnReceiveData?.Invoke(AirplaneData.data.Where(item2 => item2.id == item.id).FirstOrDefault());
                });
            }
            while (!complete)
            {
                complete = true;
                int i = 0;
                foreach(PlaneValue item in data)
                {
                    i++;
                    if (!item.isSet)
                    {
                        complete = false;
                    }
                    System.Threading.Thread.Sleep(10);
                    Debug.WriteLine($"{DateTime.Now} - Plane-Data - Durchlauf {i} - Daten noch nicht vollständig");
                }
            }
            AirplaneData.complete = true;
            Debug.WriteLine($"{DateTime.Now} - Plane-Data - Daten vollständig - Unsubscribing");
            foreach(PlaneValue pv in data)
            {
                Console.WriteLine($"{pv.id}: \t {pv.value}");
            }

            Task.Run(() => {
                foreach (PlaneValue item in data)
                {
                    AirplaneData.connector.Unsubscribe(item.dataRef.DataRef);
                }
            });

        }

        public static float getValue(string id)
        {
            return AirplaneData.data.Where(item => item.id == id).FirstOrDefault().value;
        }
    }
}
