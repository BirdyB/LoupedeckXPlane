using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.SupportClasses
{
    public class BoundariesData
    {
        public class BoundaryValue
        {
            public string id { get; set; }
            public DataRefElement dataRef { get; set; }
            public bool isSet { get; set; } = false;
            public float value { get; set; } = 0;
        }

        public static bool complete = false;

        public static List<BoundaryValue> data = new List<BoundaryValue>();

        private static XPlaneConnector.XPlaneConnector connector = new XPlaneConnector.XPlaneConnector();


        public BoundariesData()
        {
        }

        public static void init()
        {
            data.Add(new BoundaryValue { id = "green_lo_MP", dataRef=DataRefs.AircraftLimitsGreenLoMP });
            data.Add(new BoundaryValue { id = "green_hi_MP", dataRef = DataRefs.AircraftLimitsGreenHiMP });
            data.Add(new BoundaryValue { id = "yellow_lo_MP", dataRef = DataRefs.AircraftLimitsYellowLoMP });
            data.Add(new BoundaryValue { id = "yellow_hi_MP", dataRef = DataRefs.AircraftLimitsYellowHiMP });
            data.Add(new BoundaryValue { id = "red_lo_MP", dataRef = DataRefs.AircraftLimitsRedLoMP });
            data.Add(new BoundaryValue { id = "red_hi_MP", dataRef = DataRefs.AircraftLimitsRedHiMP });

            data.Add(new BoundaryValue { id = "green_lo_BatAmp", dataRef = DataRefs.AircraftLimitsGreenLoBatAmp });
            data.Add(new BoundaryValue { id = "green_hi_BatAmp", dataRef = DataRefs.AircraftLimitsGreenHiBatAmp });
            data.Add(new BoundaryValue { id = "yellow_lo_BatAmp", dataRef = DataRefs.AircraftLimitsYellowLoBatAmp});
            data.Add(new BoundaryValue { id = "yellow_hi_BatAmp", dataRef = DataRefs.AircraftLimitsYellowHiBatAmp});
            data.Add(new BoundaryValue { id = "red_lo_BatAmp", dataRef = DataRefs.AircraftLimitsRedLoBatAmp });
            data.Add(new BoundaryValue { id = "red_hi_BatAmp", dataRef = DataRefs.AircraftLimitsRedHiBatAmp });

            data.Add(new BoundaryValue { id = "green_lo_BatVolts", dataRef = DataRefs.AircraftLimitsGreenLoBatVolt });
            data.Add(new BoundaryValue { id = "green_hi_BatVolts", dataRef = DataRefs.AircraftLimitsGreenHiBatVolt });
            data.Add(new BoundaryValue { id = "yellow_lo_BatVolts", dataRef = DataRefs.AircraftLimitsYellowLoBatVolt });
            data.Add(new BoundaryValue { id = "yellow_hi_BatVolts", dataRef = DataRefs.AircraftLimitsYellowHiBatVolt });
            data.Add(new BoundaryValue { id = "red_lo_BatVolts", dataRef = DataRefs.AircraftLimitsRedLoBatVolt });
            data.Add(new BoundaryValue { id = "red_hi_BatVolts", dataRef = DataRefs.AircraftLimitsRedHiBatVolt });

            data.Add(new BoundaryValue { id = "green_lo_CHT", dataRef = DataRefs.AircraftLimitsGreenLoCHT });
            data.Add(new BoundaryValue { id = "green_hi_CHT", dataRef = DataRefs.AircraftLimitsGreenHiCHT });
            data.Add(new BoundaryValue { id = "yellow_lo_CHT", dataRef = DataRefs.AircraftLimitsYellowLoCHT });
            data.Add(new BoundaryValue { id = "yellow_hi_CHT", dataRef = DataRefs.AircraftLimitsYellowHiCHT });
            data.Add(new BoundaryValue { id = "red_lo_CHT", dataRef = DataRefs.AircraftLimitsRedLoCHT });
            data.Add(new BoundaryValue { id = "red_hi_CHT", dataRef = DataRefs.AircraftLimitsRedHiCHT });

            data.Add(new BoundaryValue { id = "green_lo_EGT", dataRef = DataRefs.AircraftLimitsGreenLoEGT });
            data.Add(new BoundaryValue { id = "green_hi_EGT", dataRef = DataRefs.AircraftLimitsGreenHiEGT });
            data.Add(new BoundaryValue { id = "yellow_lo_EGT", dataRef = DataRefs.AircraftLimitsYellowLoEGT });
            data.Add(new BoundaryValue { id = "yellow_hi_EGT", dataRef = DataRefs.AircraftLimitsYellowHiEGT });
            data.Add(new BoundaryValue { id = "red_lo_EGT", dataRef = DataRefs.AircraftLimitsRedLoEGT });
            data.Add(new BoundaryValue { id = "red_hi_EGT", dataRef = DataRefs.AircraftLimitsRedHiEGT });

            data.Add(new BoundaryValue { id = "green_lo_EPR", dataRef = DataRefs.AircraftLimitsGreenLoEPR });
            data.Add(new BoundaryValue { id = "green_hi_EPR", dataRef = DataRefs.AircraftLimitsGreenHiEPR });
            data.Add(new BoundaryValue { id = "yellow_lo_EPR", dataRef = DataRefs.AircraftLimitsYellowLoEPR });
            data.Add(new BoundaryValue { id = "yellow_hi_EPR", dataRef = DataRefs.AircraftLimitsYellowHiEPR });
            data.Add(new BoundaryValue { id = "red_lo_EPR", dataRef = DataRefs.AircraftLimitsRedLoEPR });
            data.Add(new BoundaryValue { id = "red_hi_EPR", dataRef = DataRefs.AircraftLimitsRedHiEPR });

            data.Add(new BoundaryValue { id = "green_lo_FF", dataRef = DataRefs.AircraftLimitsGreenLoFF });
            data.Add(new BoundaryValue { id = "green_hi_FF", dataRef = DataRefs.AircraftLimitsGreenHiFF });
            data.Add(new BoundaryValue { id = "yellow_lo_FF", dataRef = DataRefs.AircraftLimitsYellowLoFF });
            data.Add(new BoundaryValue { id = "yellow_hi_FF", dataRef = DataRefs.AircraftLimitsYellowHiFF });
            data.Add(new BoundaryValue { id = "red_lo_FF", dataRef = DataRefs.AircraftLimitsRedLoFF });
            data.Add(new BoundaryValue { id = "red_hi_FF", dataRef = DataRefs.AircraftLimitsRedHiFF });

            data.Add(new BoundaryValue { id = "green_lo_Fuelp", dataRef = DataRefs.AircraftLimitsGreenLoFuelp });
            data.Add(new BoundaryValue { id = "green_hi_Fuelp", dataRef = DataRefs.AircraftLimitsGreenHiFuelp });
            data.Add(new BoundaryValue { id = "yellow_lo_Fuelp", dataRef = DataRefs.AircraftLimitsYellowLoFuelp });
            data.Add(new BoundaryValue { id = "yellow_hi_Fuelp", dataRef = DataRefs.AircraftLimitsYellowHiFuelp });
            data.Add(new BoundaryValue { id = "red_lo_Fuelp", dataRef = DataRefs.AircraftLimitsRedLoFuelp });
            data.Add(new BoundaryValue { id = "red_hi_Fuelp", dataRef = DataRefs.AircraftLimitsRedHiFuelp });

            data.Add(new BoundaryValue { id = "green_lo_GenAmp", dataRef = DataRefs.AircraftLimitsGreenLoGenAmp });
            data.Add(new BoundaryValue { id = "green_hi_GenAmp", dataRef = DataRefs.AircraftLimitsGreenHiGenAmp });
            data.Add(new BoundaryValue { id = "yellow_lo_GenAmp", dataRef = DataRefs.AircraftLimitsYellowLoGenAmp });
            data.Add(new BoundaryValue { id = "yellow_hi_GenAmp", dataRef = DataRefs.AircraftLimitsYellowHiGenAmp });
            data.Add(new BoundaryValue { id = "red_lo_GenAmp", dataRef = DataRefs.AircraftLimitsRedLoGenAmp });
            data.Add(new BoundaryValue { id = "red_hi_GenAmp", dataRef = DataRefs.AircraftLimitsRedHiGenAmp });

            data.Add(new BoundaryValue { id = "green_lo_ITT", dataRef = DataRefs.AircraftLimitsGreenLoITT });
            data.Add(new BoundaryValue { id = "green_hi_ITT", dataRef = DataRefs.AircraftLimitsGreenHiITT });
            data.Add(new BoundaryValue { id = "yellow_lo_ITT", dataRef = DataRefs.AircraftLimitsYellowLoITT });
            data.Add(new BoundaryValue { id = "yellow_hi_ITT", dataRef = DataRefs.AircraftLimitsYellowHiITT });
            data.Add(new BoundaryValue { id = "red_lo_ITT", dataRef = DataRefs.AircraftLimitsRedLoITT });
            data.Add(new BoundaryValue { id = "red_hi_ITT", dataRef = DataRefs.AircraftLimitsRedHiITT });

            data.Add(new BoundaryValue { id = "green_lo_N1", dataRef = DataRefs.AircraftLimitsGreenLoN1 });
            data.Add(new BoundaryValue { id = "green_hi_N1", dataRef = DataRefs.AircraftLimitsGreenHiN1 });
            data.Add(new BoundaryValue { id = "yellow_lo_N1", dataRef = DataRefs.AircraftLimitsYellowLoN1 });
            data.Add(new BoundaryValue { id = "yellow_hi_N1", dataRef = DataRefs.AircraftLimitsYellowHiN1 });
            data.Add(new BoundaryValue { id = "red_lo_N1", dataRef = DataRefs.AircraftLimitsRedLoN1 });
            data.Add(new BoundaryValue { id = "red_hi_N1", dataRef = DataRefs.AircraftLimitsRedHiN1 });

            data.Add(new BoundaryValue { id = "green_lo_N2", dataRef = DataRefs.AircraftLimitsGreenLoN2 });
            data.Add(new BoundaryValue { id = "green_hi_N2", dataRef = DataRefs.AircraftLimitsGreenHiN2 });
            data.Add(new BoundaryValue { id = "yellow_lo_N2", dataRef = DataRefs.AircraftLimitsYellowLoN2 });
            data.Add(new BoundaryValue { id = "yellow_hi_N2", dataRef = DataRefs.AircraftLimitsYellowHiN2 });
            data.Add(new BoundaryValue { id = "red_lo_N2", dataRef = DataRefs.AircraftLimitsRedLoN2 });
            data.Add(new BoundaryValue { id = "red_hi_N2", dataRef = DataRefs.AircraftLimitsRedHiN2 });

            data.Add(new BoundaryValue { id = "green_lo_Oilp", dataRef = DataRefs.AircraftLimitsGreenLoOilp });
            data.Add(new BoundaryValue { id = "green_hi_Oilp", dataRef = DataRefs.AircraftLimitsGreenHiOilp });
            data.Add(new BoundaryValue { id = "yellow_lo_Oilp", dataRef = DataRefs.AircraftLimitsYellowLoOilp });
            data.Add(new BoundaryValue { id = "yellow_hi_Oilp", dataRef = DataRefs.AircraftLimitsYellowHiOilp });
            data.Add(new BoundaryValue { id = "red_lo_Oilp", dataRef = DataRefs.AircraftLimitsRedLoOilp });
            data.Add(new BoundaryValue { id = "red_hi_Oilp", dataRef = DataRefs.AircraftLimitsRedHiOilp });

            data.Add(new BoundaryValue { id = "green_lo_Oilt", dataRef = DataRefs.AircraftLimitsGreenLoOilt });
            data.Add(new BoundaryValue { id = "green_hi_Oilt", dataRef = DataRefs.AircraftLimitsGreenHiOilt });
            data.Add(new BoundaryValue { id = "yellow_lo_Oilt", dataRef = DataRefs.AircraftLimitsYellowLoOilt });
            data.Add(new BoundaryValue { id = "yellow_hi_Oilt", dataRef = DataRefs.AircraftLimitsYellowHiOilt });
            data.Add(new BoundaryValue { id = "red_lo_Oilt", dataRef = DataRefs.AircraftLimitsRedLoOilt });
            data.Add(new BoundaryValue { id = "red_hi_Oilt", dataRef = DataRefs.AircraftLimitsRedHiOilt });

            data.Add(new BoundaryValue { id = "green_lo_TRQ", dataRef = DataRefs.AircraftLimitsGreenLoTRQ });
            data.Add(new BoundaryValue { id = "green_hi_TRQ", dataRef = DataRefs.AircraftLimitsGreenHiTRQ });
            data.Add(new BoundaryValue { id = "yellow_lo_TRQ", dataRef = DataRefs.AircraftLimitsYellowLoTRQ });
            data.Add(new BoundaryValue { id = "yellow_hi_TRQ", dataRef = DataRefs.AircraftLimitsYellowHiTRQ });
            data.Add(new BoundaryValue { id = "red_lo_TRQ", dataRef = DataRefs.AircraftLimitsRedLoTRQ });
            data.Add(new BoundaryValue { id = "red_hi_TRQ", dataRef = DataRefs.AircraftLimitsRedHiTRQ });

            data.Add(new BoundaryValue { id = "green_lo_Vac", dataRef = DataRefs.AircraftLimitsGreenLoVac });
            data.Add(new BoundaryValue { id = "green_hi_Vac", dataRef = DataRefs.AircraftLimitsGreenHiVac });
            data.Add(new BoundaryValue { id = "yellow_lo_Vac", dataRef = DataRefs.AircraftLimitsYellowLoVac });
            data.Add(new BoundaryValue { id = "yellow_hi_Vac", dataRef = DataRefs.AircraftLimitsYellowHiVac });
            data.Add(new BoundaryValue { id = "red_lo_Vac", dataRef = DataRefs.AircraftLimitsRedLoVac });
            data.Add(new BoundaryValue { id = "red_hi_Vac", dataRef = DataRefs.AircraftLimitsRedHiVac });
        }

        public static void getData()
        {
            bool complete = false;
            BoundariesData.connector.Start();
            foreach (BoundaryValue item in data)
            {
                BoundariesData.connector.Subscribe(item.dataRef, 5, (element, value) => {
                    BoundariesData.data.Where(item2 => item2.id == item.id).FirstOrDefault().value = value;
                    BoundariesData.data.Where(item2 => item2.id == item.id).FirstOrDefault().isSet = true;
                });
            }
            while (!complete)
            {
                complete = true;
                int i = 0;
                foreach (BoundaryValue item in data)
                {
                    i++;
                    if (!item.isSet)
                    {
                        complete = false;
                    }
                    System.Threading.Thread.Sleep(10);
                }
            }
            BoundariesData.complete = true;

            Task.Run(() => {
                foreach (BoundaryValue item in data)
                {
                    BoundariesData.connector.Unsubscribe(item.dataRef.DataRef);
                }
            });

        }

        public static float getValue(string id)
        {
            return BoundariesData.data.Where(item => item.id == id).FirstOrDefault().value;
        }
    }
}
