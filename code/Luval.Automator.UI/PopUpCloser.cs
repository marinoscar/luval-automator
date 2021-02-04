using Luval.Automator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Luval.Automator.UI
{
    public class PopUpCloser
    {
        public PopUpCloser()
        {
            Configs = new List<PopUpConfig>();
        }

        private class WindowItem { public WindowElement Window { get; set; } public bool IsClosed { get; set; } }
        private List<WindowItem> _windows;
        public string ProcessName { get; set; }
        public List<PopUpConfig> Configs { get; set; }

        public void Execute()
        {
            ProcessName = "OUTLOOK";
            Configs.Add(new PopUpConfig() { ButtonxPath = @"//Element[@Type='button' and @Name='No']", TitleRegEx = "Microsoft Outlook" });
            Configs.Add(new PopUpConfig() { ButtonxPath = @"//Element[@Type='button' and @Name='OK']", TitleRegEx = "Microsoft Outlook" });
            //Don't save changes
            LoadAllWindows();
            CloseMainWindow();
            Thread.Sleep(2000);
            DoClosePopUps(10, DateTime.UtcNow);
        }

        private void DoClosePopUps(int minsToWait, DateTime startUtc)
        {
            LoadAllWindows();
            _windows = _windows.Where(i => i.Window.Item.Current.LocalizedControlType.ToLowerInvariant().Equals("dialog")).ToList();
            if (!_windows.Any() || DateTime.UtcNow.Subtract(startUtc).TotalMinutes > minsToWait) return;
            var win = _windows.First();
            foreach (var config in Configs)
            {
                if (ClosePopUp(win.Window, config))
                {
                    win.IsClosed = true;
                    break;
                }
            }
            if (win.IsClosed) DoClosePopUps(minsToWait, startUtc);
        }


        private void ClosePopUps()
        {
            var dialogs = GetDialogs();
            while (true)
            {
                if (dialogs.Any())
                {
                    foreach (var config in Configs)
                    {
                        ClosePopUp(dialogs[0], config);
                    }
                }
                dialogs = GetDialogs();
                Thread.Sleep(3000);
            }
        }

        private List<WindowElement> GetWindows()
        {
            return new WindowSelector().FindAllForProcess(ProcessName).Where(i => i.Item.Current.LocalizedControlType.ToLowerInvariant() != "dialog").ToList();
        }

        private List<WindowElement> GetDialogs()
        {
            return new WindowSelector().FindAllForProcess(ProcessName).Where(i => i.Item.Current.LocalizedControlType.ToLowerInvariant() == "dialog").ToList();
        }

        private bool ClosePopUp(WindowElement window, PopUpConfig config)
        {
            if (!string.IsNullOrWhiteSpace(config.LabelTextRegEx) && !IsTitleAMatch(config.TitleRegEx, window)) return false;
            if (!string.IsNullOrWhiteSpace(config.LabelxPath))
            {
                var label = window.Query(config.LabelxPath).FirstOrDefault();
                if (label == null) return false;
                if (!string.IsNullOrWhiteSpace(config.LabelTextRegEx) && !Regex.IsMatch(label.Item.Current.Name, config.LabelTextRegEx)) return false;
            }
            if (string.IsNullOrWhiteSpace(config.ButtonxPath)) return false;
            var btn = window.Query(config.ButtonxPath).FirstOrDefault();
            if (btn == null) return false;
            Thread.Sleep(100);
            btn.Click();
            Thread.Sleep(400);
            return true;
        }

        private bool IsTitleAMatch(string regEx, WindowElement window)
        {
            return Regex.IsMatch(window.Item.Current.Name, regEx, RegexOptions.IgnoreCase);
        }


        private void Init()
        {

        }

        private void CloseMainWindow()
        {
            var regEx = "@ey.com - Outlook";
            var mainWin = _windows.Where(i => Regex.IsMatch(i.Window.Item.Current.Name, regEx) &&
                                        i.Window.Item.Current.LocalizedControlType.ToLowerInvariant().Equals("window")).First();
            CloseWindow(mainWin.Window);
            mainWin.IsClosed = true;
        }

        private void CloseWindow(WindowElement win)
        {
            var closeBtn = win.Query(@"//Element[@Type='title bar']/Children/Element[@Type='button' and @Name='Close']").FirstOrDefault();
            closeBtn.Click();
        }

        private void LoadAllWindows()
        {
            _windows = new WindowSelector().FindAllForProcess(ProcessName).Select(i => new WindowItem() { Window = i }).ToList();
        }

    }
}
