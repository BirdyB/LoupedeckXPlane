using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.SupportClasses
{
    public static class SubscriptionHandler
    {


        public static List<TypeClasses.SubscriptionValue> _subscriptions = new List<TypeClasses.SubscriptionValue>();


        public delegate void ValueChanged(TypeClasses.SubscriptionValue value);
        public static event ValueChanged OnValueChanged;

        public static void init()
        {
         
        }

        public static void subscribe(TypeClasses.SubscriptionValue sub) {
            //Debug.WriteLine($"Adding subscription for {sub.dataRef.DataRef} for module {sub.displayName}");
            _subscriptions.Add(sub);
            SupportClasses.ConnectorHandler.connector.Subscribe(sub.dataRef, 5, (element, value) => {
            //Debug.WriteLine($"Received value {value} for DataRef { sub.dataRef.DataRef}");
                if(sub.value != value)
                {
                    //Debug.WriteLine($"Value old: {sub.value} \t Value new: {value}");
                    sub.value = value;
                    OnValueChanged?.Invoke(sub);
                }

            });

        }

        public static void resetValues(string DisplayName) {
            foreach(TypeClasses.SubscriptionValue sub in SubscriptionHandler._subscriptions)
            {
                if(sub.displayName == DisplayName)
                {
                    sub.value = -1;
                }
            }
        }



    }
}
