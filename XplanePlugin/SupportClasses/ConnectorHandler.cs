using System;

using XPlaneConnector;

namespace Loupedeck.XplanePlugin.SupportClasses
{
    public static class ConnectorHandler
    {
        public static XPlaneConnector.XPlaneConnector connector = new XPlaneConnector.XPlaneConnector();

        public static void init() {
            ConnectorHandler.connector.Start();
        }
    }
}
