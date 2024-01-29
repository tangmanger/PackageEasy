using log4net;
using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Config/log4net.config", ConfigFileExtension = "config", Watch = true)]

namespace PackageEasy.Common.Logs
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:46:45
    /// desc:Log
    /// </summary>
    public class Log
    {

        public static void Init()
        {
            //var log1 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "RocGene");
            //log1.Info("---------------");
            _LogWriter = log4net.LogManager.GetLogger("PackageEasy");

        }

        private static ILog _LogWriter = null;
        /// <summary>
        /// 日志帮助助手
        /// </summary>
        public static ILog LogWriter
        {
            get
            {
                return _LogWriter;
            }
        }
        /// <summary>
        /// 写入日志(带异常）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="logLevelEnum"></param>
        public static void Write(string content, Exception ex, LogLevelType logLevelEnum = LogLevelType.Error, int rowNumber = 0, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine(content + ex.Message + ex.StackTrace);
#endif
            content = content + ex.Message + ex.StackTrace;
            content = $"方法:{methodName} 行号:{rowNumber} {content}";
            switch (logLevelEnum)
            {
                case LogLevelType.Info:
                    {
                        LogWriter.Info(content, ex);
                    }
                    break;
                case LogLevelType.Waring:
                    {
                        LogWriter.Warn(content, ex);
                    }
                    break;
                case LogLevelType.Error:
                    {
                        LogWriter.Error(content, ex);
                    }
                    break;
                case LogLevelType.Debug:
                    {
                        LogWriter.Debug(content, ex);
                    }
                    break;
                case LogLevelType.Fatal:
                    {
                        LogWriter.Fatal(content, ex);
                    }
                    break;
            }
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="logLevelEnum"></param>
        public static void Write(string content, LogLevelType logLevelEnum = LogLevelType.Info, [CallerLineNumber] int rowNumber = 0, [CallerMemberName] string methodName = "")
        {
            content = $"方法:{methodName} 行号:{rowNumber} {content}";
#if DEBUG
            Console.WriteLine(content);
#endif
            if (LogWriter == null) return;
            switch (logLevelEnum)
            {
                case LogLevelType.Info:
                    {
                        LogWriter.Info(content);
                    }
                    break;
                case LogLevelType.Waring:
                    {
                        LogWriter.Warn(content);
                    }
                    break;
                case LogLevelType.Error:
                    {
                        LogWriter.Error(content);
                    }
                    break;
                case LogLevelType.Debug:
                    {
                        LogWriter.Debug(content);
                    }
                    break;
                case LogLevelType.Fatal:
                    {
                        LogWriter.Fatal(content);
                    }
                    break;
            }
        }
    }
}
