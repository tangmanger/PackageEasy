using PackageEasy.Enums;
using PackageEasy.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common
{
    /// <summary>
    /// author:TT
    /// time:2023-03-26 16:33:48
    /// desc:TMessageBox
    /// </summary>
    public class TMessageBox
    {
        /// <summary>
        /// 弹窗
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="msg">信息</param>
        /// <param name="level">级别</param>
        /// <returns></returns>
        public static TMessageBoxResult ShowMsg(string caption, string msg, MessageLevel level = MessageLevel.Information)
        {
            MessageBox messageBox = new MessageBox(caption, msg, level);
            messageBox.ShowDialog();
            return messageBox.MessageBoxResult;
        }
        /// <summary>
        ///弹窗
        /// </summary>
        /// <param name="msg">信息级别</param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static TMessageBoxResult ShowMsg(string msg, MessageLevel level = MessageLevel.Information)
        {
            MessageBox messageBox = new MessageBox("", msg, level);
            messageBox.ShowDialog();
            return messageBox.MessageBoxResult;
        }
        /// <summary>
        /// 弹窗
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="msg">信息</param>
        /// <param name="level">级别</param>
        /// <returns></returns>
        public static TMessageBoxResult MainShowMsg(string caption, string msg, MessageLevel level)
        {
            TMessageBoxResult data = TMessageBoxResult.None;
            App.Current.Dispatcher.Invoke(() =>
            {
                MessageBox messageBox = new MessageBox(caption, msg, level);
                messageBox.ShowDialog();
                data = messageBox.MessageBoxResult;
            });
            return data;
        }
    }
}
