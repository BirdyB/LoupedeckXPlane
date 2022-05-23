using System;
using System.Diagnostics;
namespace Loupedeck.XplanePlugin.SupportClasses
{
    public static class DebugClass
    {

        public delegate void LogReceived(Exception e);
        public static event LogReceived OnLogReceived;

        public delegate void LogMessageReceived(string message);
        public static event LogMessageReceived OnLogMessageReceived;

        public static void init() {
            OnLogReceived += DebugClass_OnLogReceived;
            OnLogMessageReceived += DebugClass_OnLogMessageReceived;

        }

        private static void DebugClass_OnLogMessageReceived(string message)
        {
            Debug.WriteLine($"{DateTime.Now} - Received Message: {message}");
        }

        private static void DebugClass_OnLogReceived(Exception e)
        {
            var st = new StackTrace(e, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            Debug.WriteLine($"{DateTime.Now} - Received Exception: {e.Message} - {e.Data} - {e.Source} - {frame} - {line}");
        }

        public static void ExceptionReceived(Exception e, string message="") {
            var st = new StackTrace(e, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            Debug.WriteLine($"{DateTime.Now} \t Received Exception: {e.Message} \t {e.Data} \t {e.Source} \t {frame} \t {line} \t with message \t {message}");
        }

        public static void MessageReceived(string message) {
            Debug.WriteLine($"{DateTime.Now} - Received Message: {message}");
        }
    }
}
