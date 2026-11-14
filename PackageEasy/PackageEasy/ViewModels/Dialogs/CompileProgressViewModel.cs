using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common.Data;
using PackageEasy.Domain.Common;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace PackageEasy.ViewModels.Dialogs
{
    /// <summary>
    /// author:TT
    /// time:2024-05-20 10:00:00
    /// desc:编译日志行
    /// </summary>
    public class CompileLogItem
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; } = DateTime.Now.ToString("HH:mm:ss");

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError { get; set; }
    }

    /// <summary>
    /// author:TT
    /// time:2024-05-20 10:00:00
    /// desc:编译进度窗口视图模型
    /// </summary>
    public class CompileProgressViewModel : ObservableObject
    {
        /// <summary>
        /// 日志上限，超过后移除最早的行
        /// </summary>
        private const int MaxLogCount = 4000;

        /// <summary>
        /// 未完成时的进度上限
        /// </summary>
        private const double MaxRunningProgress = 92;

        public CompileProgressViewModel(string projectName)
        {
            Title = string.Format(CommonSettings.CompileProgressTitle.GetLangText(), projectName);
            StateText = CommonSettings.CompileStateCompiling.GetLangText();
        }

        /// <summary>
        /// 请求关闭窗口
        /// </summary>
        public event EventHandler CloseRequested;

        #region 属性

        private string title;
        private string stateText;
        private double progress;
        private bool isFinished;
        private bool isFailed;
        private string outputPath;
        private string outputSize;
        private string errorSummary;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StateText
        {
            get => stateText;
            set
            {
                stateText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 进度 0-100
        /// </summary>
        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsFinished
        {
            get => isFinished;
            set
            {
                isFinished = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否失败
        /// </summary>
        public bool IsFailed
        {
            get => isFailed;
            set
            {
                isFailed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath
        {
            get => outputPath;
            set
            {
                outputPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 输出文件大小
        /// </summary>
        public string OutputSize
        {
            get => outputSize;
            set
            {
                outputSize = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 错误摘要
        /// </summary>
        public string ErrorSummary
        {
            get => errorSummary;
            set
            {
                errorSummary = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 日志列表
        /// </summary>
        public ObservableCollection<CompileLogItem> Logs { get; } = new ObservableCollection<CompileLogItem>();

        #endregion

        #region 方法

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="isError">是否错误</param>
        public void AppendLog(string text, bool isError = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            Logs.Add(new CompileLogItem()
            {
                Text = text,
                IsError = isError
            });
            while (Logs.Count > MaxLogCount)
            {
                Logs.RemoveAt(0);
            }
            if (!IsFinished && Progress < MaxRunningProgress)
            {
                Progress += 0.6;
            }
        }

        /// <summary>
        /// 编译完成
        /// </summary>
        /// <param name="path">输出文件</param>
        public void SetFinished(string path)
        {
            Progress = 100;
            IsFinished = true;
            IsFailed = false;
            StateText = CommonSettings.CompileStateSuccess.GetLangText();
            OutputPath = path;
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                {
                    OutputSize = FormatSize(new FileInfo(path).Length);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 编译失败
        /// </summary>
        /// <param name="message">错误摘要</param>
        public void SetFailed(string message)
        {
            IsFinished = true;
            IsFailed = true;
            StateText = CommonSettings.CompileStateFailed.GetLangText();
            ErrorSummary = message;
        }

        /// <summary>
        /// 打开输出目录
        /// </summary>
        /// <returns></returns>
        public bool OpenOutputFolder()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OutputPath))
                    return false;
                FileInfo fileInfo = new FileInfo(OutputPath);
                if (fileInfo.Directory == null || !fileInfo.Directory.Exists)
                    return false;
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = fileInfo.Directory.FullName
                };
                process.Start();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string FormatSize(long length)
        {
            if (length < 1024)
                return $"{length} B";
            if (length < 1024 * 1024)
                return $"{length / 1024d:F1} KB";
            if (length < 1024L * 1024 * 1024)
                return $"{length / 1024d / 1024d:F1} MB";
            return $"{length / 1024d / 1024d / 1024d:F2} GB";
        }

        #endregion

        #region 命令

        /// <summary>
        /// 打开输出目录
        /// </summary>
        public RelayCommand OpenOutputFolderCommand => new RelayCommand(() =>
        {
            OpenOutputFolder();
        });

        /// <summary>
        /// 关闭
        /// </summary>
        public RelayCommand CloseCommand => new RelayCommand(() =>
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        });

        #endregion
    }
}
