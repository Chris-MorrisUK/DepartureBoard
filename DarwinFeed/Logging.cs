using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace DarwinFeed
{
    public class Logging
    {
        public static readonly string EVENTLOGSOURCE = @"DarwinFeed";

        public static void InitialiseLogging()
        {
            if (!EventLog.SourceExists(EVENTLOGSOURCE))
            {
                EventLog.CreateEventSource(EVENTLOGSOURCE, string.Empty);
            }

            if (theLogger == null)
                lock (theLoggerLock)
                {
                    if (theLogger != null) return;
                    EventLog toUse = new EventLog();
                    toUse.Source = EVENTLOGSOURCE;
                    theLogger = new Logging(toUse);
                }
        }

        private Logging(EventLog theLog)
        {
            theEventLog = theLog;
            theEventLog.WriteEntry("Application started; log created", EventLogEntryType.Information, 1);
        }

        private readonly EventLog theEventLog;
        private static Logging theLogger;
        private static object theLoggerLock = new object();

        public void LogInformationMessage(string message, int eventID)
        {
            theEventLog.WriteEntry(message, EventLogEntryType.Information, eventID);

        }

        public static void LogMessage(string message, int eventID)
        {
            if (theLogger == null)
                InitialiseLogging();
            theLogger.LogInformationMessage(message, eventID);
        }

        public void LogWarningMessage(string message, int eventID)
        {
            theEventLog.WriteEntry(message, EventLogEntryType.Warning, eventID);

        }

        public static void LogWarning(string message, int eventID)
        {
            if (theLogger == null)
                InitialiseLogging();
            theLogger.LogWarningMessage(message, eventID);
        }

        public void LogErrorMessage(string message, int eventID)
        {
            theEventLog.WriteEntry(message, EventLogEntryType.Error, eventID);
        }

        public static void LogError(string message, int eventID)
        {
            if (theLogger == null)
                InitialiseLogging();
            theLogger.LogErrorMessage(message, eventID);
        }

        public static void LogError(Exception ex, string auxMessage, int eventID)
        {
            if (theLogger == null)
                InitialiseLogging();

            StringBuilder Message = new StringBuilder();
            Message.AppendLine(auxMessage);
            Message.AppendLine(ex.Message);
            Message.AppendLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Message.AppendLine("Inner: ");
                Message.AppendLine(ex.InnerException.ToString());
            }

            theLogger.LogErrorMessage(Message.ToString(), eventID);
#if DEBUG
            Console.WriteLine(Message);
#endif
        }

        /// <summary>
        /// Use this for more complex logging not handled by the other public functions
        /// </summary>
        /// <returns></returns>
        public static EventLog GetEventLog()
        {
            if (theLogger == null)
                InitialiseLogging();
            return theLogger.theEventLog;
        }




    }
}