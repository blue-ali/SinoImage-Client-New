using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Xml;

namespace DocScanner.LibCommon
{
    public class FileLogger
    {
        // Fields
        private static ILog _logger = LogManager.GetLogger("logger");
        private static string logFilePath = (SystemHelper.GetAssemblesDirectory() + "log4Net.xml");

        // Methods
        public static void Debug(string msg, Exception e)
        {
            if ((_logger != null) && _logger.IsDebugEnabled)
            {
                _logger.Debug(msg, e);
            }
        }

        public static void Error(string msg, Exception err)
        {
            if ((_logger != null) && _logger.IsErrorEnabled)
            {
                _logger.Error(msg, err);
            }
        }

        public static void Info(string msg)
        {
            Info(msg);
            if ((_logger != null) && _logger.IsInfoEnabled)
            {
                _logger.Info(msg);
            }
        }

        public static void LogFilePath(string path)
        {
            if (!File.Exists(logFilePath))
            {
                logFilePath = "log4net.xml";
            }
            XmlDocument document = new XmlDocument();
            document.Load(logFilePath);
            document.SelectSingleNode("log4net").SelectSingleNode("appender").SelectSingleNode("file").Attributes["value"].Value = path;
            document.Save(logFilePath);
        }

        public static void LogMessage(EMessageType type, string msg)
        {
        }

        public static void SetConfig(string configPath)
        {
            LogFilePath(configPath + "TigEra.DocScaner.log");
            FileInfo configFile = new FileInfo(logFilePath);
            XmlConfigurator.Configure(configFile);
        }

        public static void Warn(string msg)
        {
            if ((_logger != null) && _logger.IsWarnEnabled)
            {
                _logger.Warn(msg);
            }
        }
    }

}