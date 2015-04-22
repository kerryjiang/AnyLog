using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace AnyLog.NLog
{
    public class NLogLog : ILog
    {
        private Logger m_Logger;

        private static readonly IDictionary<string, LogLevel> s_LevelDict;

        static NLogLog()
        {
            s_LevelDict = new Dictionary<string, LogLevel>(StringComparer.OrdinalIgnoreCase);
            s_LevelDict.Add("Alert", LogLevel.Warn);
            s_LevelDict.Add("All", LogLevel.Info);
            s_LevelDict.Add("Critical", LogLevel.Fatal);
            s_LevelDict.Add("Debug", LogLevel.Debug);
            s_LevelDict.Add("Emergency", LogLevel.Fatal);
            s_LevelDict.Add("Error", LogLevel.Error);
            s_LevelDict.Add("Fatal", LogLevel.Fatal);
            s_LevelDict.Add("Fine", LogLevel.Info);
            s_LevelDict.Add("Finer", LogLevel.Info);
            s_LevelDict.Add("Finest", LogLevel.Info);
            s_LevelDict.Add("Info", LogLevel.Info);
            s_LevelDict.Add("Notice", LogLevel.Info);
            s_LevelDict.Add("Severe", LogLevel.Fatal);
            s_LevelDict.Add("Trace", LogLevel.Trace);
            s_LevelDict.Add("Verbose", LogLevel.Trace);
            s_LevelDict.Add("Warn", LogLevel.Warn);
        }

        public NLogLog(Logger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            m_Logger = logger;
        }

        public bool IsDebugEnabled
        {
            get { return m_Logger.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return m_Logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return m_Logger.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return m_Logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return m_Logger.IsWarnEnabled; }
        }

        public void Debug(object message)
        {
            m_Logger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            m_Logger.Debug("" + message, exception);
        }

        public void DebugFormat(string format, object arg0)
        {
            m_Logger.Debug(format, arg0);
        }

        public void DebugFormat(string format, params object[] args)
        {
            m_Logger.Debug(format, args);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            m_Logger.Debug(provider, format, args);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            m_Logger.Debug(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            m_Logger.Debug(format, arg0, arg1, arg2);
        }

        public void Error(object message)
        {
            m_Logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            m_Logger.Error("" + message, exception);
        }

        public void ErrorFormat(string format, object arg0)
        {
            m_Logger.Error(format, arg0);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            m_Logger.Error(format, args);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            m_Logger.Error(provider, format, args);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            m_Logger.Error(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            m_Logger.Error(format, arg0, arg1, arg2);
        }

        public void Fatal(object message)
        {
            m_Logger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            m_Logger.Fatal("" + message, exception);
        }

        public void FatalFormat(string format, object arg0)
        {
            m_Logger.Fatal(format, arg0);
        }

        public void FatalFormat(string format, params object[] args)
        {
            m_Logger.Fatal(format, args);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            m_Logger.Fatal(provider, format, args);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            m_Logger.Fatal(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            m_Logger.Fatal(format, arg0, arg1, arg2);
        }

        public void Info(object message)
        {
            m_Logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            m_Logger.Info("" + message, exception);
        }

        public void InfoFormat(string format, object arg0)
        {
            m_Logger.Info(format, arg0);
        }

        public void InfoFormat(string format, params object[] args)
        {
            m_Logger.Info(format, args);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            m_Logger.Info(provider, format, args);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            m_Logger.Info(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            m_Logger.Info(format, arg0, arg1, arg2);
        }

        public void Warn(object message)
        {
            m_Logger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            m_Logger.Warn("" + message, exception);
        }

        public void WarnFormat(string format, object arg0)
        {
            m_Logger.Warn(format, arg0);
        }

        public void WarnFormat(string format, params object[] args)
        {
            m_Logger.Warn(format, args);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            m_Logger.Warn(provider, format, args);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            m_Logger.Warn(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            m_Logger.Warn(format, arg0, arg1, arg2);
        }

        public void Log(LoggingData loggingData)
        {
            var logEvent = new LogEventInfo();
            logEvent.LoggerName = loggingData.LoggerName;
            logEvent.Message = loggingData.Message;
            logEvent.TimeStamp = loggingData.TimeStamp;

            //logEvent.Exception = loggingData.ExceptionString;
            //logEvent.Properties = loggingData.Properties;

            LogLevel level;

            if (!s_LevelDict.TryGetValue(loggingData.Level, out level))
                level = LogLevel.Info;

            logEvent.Level = level;

            m_Logger.Log(logEvent);
        }
    }
}
