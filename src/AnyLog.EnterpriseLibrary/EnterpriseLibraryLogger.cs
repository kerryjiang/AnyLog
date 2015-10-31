using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using AnyLog;
using System.Collections.Generic;

namespace AnyLog.EnterpriseLibrary
{
    public class EnterpriseLibraryLogger : ILog
    {
        private string m_Category;

        private const string m_MessageTemplate2 = "{0}\r\n{1}";
        private const string m_MessageTemplate3 = "{0}\r\n{1}\r\n{2}";

        private LogWriter m_LogWriter;

        private static Dictionary<string, TraceEventType> s_EventTypeDict;

        static EnterpriseLibraryLogger()
        {
            s_EventTypeDict = new Dictionary<string, TraceEventType>(StringComparer.OrdinalIgnoreCase);
            s_EventTypeDict.Add("Alert", TraceEventType.Warning);
            s_EventTypeDict.Add("All", TraceEventType.Information);
            s_EventTypeDict.Add("Critical", TraceEventType.Critical);
            s_EventTypeDict.Add("Debug", TraceEventType.Verbose);
            s_EventTypeDict.Add("Emergency", TraceEventType.Critical);
            s_EventTypeDict.Add("Error", TraceEventType.Error);
            s_EventTypeDict.Add("Fatal", TraceEventType.Critical);
            s_EventTypeDict.Add("Fine", TraceEventType.Information);
            s_EventTypeDict.Add("Finer", TraceEventType.Information);
            s_EventTypeDict.Add("Finest", TraceEventType.Information);
            s_EventTypeDict.Add("Info", TraceEventType.Information);
            s_EventTypeDict.Add("Notice", TraceEventType.Information);
            s_EventTypeDict.Add("Severe", TraceEventType.Critical);
            s_EventTypeDict.Add("Trace", TraceEventType.Verbose);
            s_EventTypeDict.Add("Verbose", TraceEventType.Verbose);
            s_EventTypeDict.Add("Warn", TraceEventType.Warning);
        }

        public EnterpriseLibraryLogger(LogWriter logWriter, string category)
        {
            m_LogWriter = logWriter;
            m_Category = category;
        }

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        private void Write(object message, string cactegory, TraceEventType logSeverity)
        {
            m_LogWriter.Write(message, cactegory, -1, 1, logSeverity, string.Empty, null);
        }

        public void Debug(object message)
        {
            Write(message, m_Category, System.Diagnostics.TraceEventType.Verbose);
        }

        public void Debug(object message, Exception exception)
        {
            Write(string.Format(m_MessageTemplate3, message, exception.Message, exception.StackTrace), m_Category, TraceEventType.Verbose);
        }

        public void DebugFormat(string format, object arg0)
        {
            Write(string.Format(format, arg0), m_Category, TraceEventType.Verbose);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Write(string.Format(format, args), m_Category, TraceEventType.Verbose);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Write(string.Format(provider, format, args), m_Category, TraceEventType.Verbose);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0, arg1), m_Category, TraceEventType.Verbose);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2), m_Category, TraceEventType.Verbose);
        }

        public void Error(object message)
        {
            Write(message, m_Category, TraceEventType.Error);
        }

        public void Error(object message, Exception exception)
        {
            Write(string.Format(m_MessageTemplate3, message, exception.Message, exception.StackTrace), m_Category, TraceEventType.Error);
        }

        public void ErrorFormat(string format, object arg0)
        {
            Write(string.Format(format, arg0), m_Category, TraceEventType.Error);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Write(string.Format(format, args), m_Category, TraceEventType.Error);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Write(string.Format(provider, format, args), m_Category, TraceEventType.Error);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0, arg1), m_Category, TraceEventType.Error);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2), m_Category, TraceEventType.Error);
        }

        public void Fatal(object message)
        {
            Write(message, m_Category, TraceEventType.Critical);
        }

        public void Fatal(object message, Exception exception)
        {
            Write(string.Format(m_MessageTemplate3, message, exception.Message, exception.StackTrace), m_Category, TraceEventType.Critical);
        }

        public void FatalFormat(string format, object arg0)
        {
            Write(string.Format(format, arg0), m_Category, TraceEventType.Critical);
        }

        public void FatalFormat(string format, params object[] args)
        {
            Write(string.Format(format, args), m_Category, TraceEventType.Critical);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Write(string.Format(provider, format, args), m_Category, TraceEventType.Critical);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0), m_Category, TraceEventType.Critical);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2), m_Category, TraceEventType.Critical);
        }

        public void Info(object message)
        {
            Write(message, m_Category, TraceEventType.Information);
        }

        public void Info(object message, Exception exception)
        {
            Write(string.Format(m_MessageTemplate3, message, exception.Message, exception.StackTrace), m_Category, TraceEventType.Information);
        }

        public void InfoFormat(string format, object arg0)
        {
            Write(string.Format(format, arg0), m_Category, TraceEventType.Information);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Write(string.Format(format, args), m_Category, TraceEventType.Information);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Write(string.Format(format, args), m_Category, TraceEventType.Information);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0, arg1), m_Category, TraceEventType.Information);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2), m_Category, TraceEventType.Information);
        }

        public void Warn(object message)
        {
            Write(message, m_Category, TraceEventType.Warning);
        }

        public void Warn(object message, Exception exception)
        {
            Write(string.Format(m_MessageTemplate3, message, exception.Message, exception.StackTrace), m_Category, TraceEventType.Warning);
        }

        public void WarnFormat(string format, object arg0)
        {
            Write(string.Format(format, arg0), m_Category, TraceEventType.Warning);
        }

        public void WarnFormat(string format, params object[] args)
        {
            Write(string.Format(format, args), m_Category, TraceEventType.Warning);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Write(string.Format(provider, format, args), m_Category, TraceEventType.Warning);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0, arg1), m_Category, TraceEventType.Warning);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2), m_Category, TraceEventType.Warning);
        }


        public void Log(LoggingData loggingData)
        {
            var logEntry = new LogEntry();

            logEntry.Message = loggingData.Message;
            logEntry.AppDomainName = loggingData.Domain;
            logEntry.AddErrorMessage(loggingData.ExceptionString);
            logEntry.Categories = new string[] { loggingData.LoggerName };

            TraceEventType eventType;

            if (!s_EventTypeDict.TryGetValue(loggingData.Level, out eventType))
                eventType = TraceEventType.Information;

            logEntry.Severity = eventType;

            logEntry.ManagedThreadName = loggingData.ThreadName;
            logEntry.TimeStamp = loggingData.TimeStamp;

            m_LogWriter.Write(logEntry);
        }
    }
}
