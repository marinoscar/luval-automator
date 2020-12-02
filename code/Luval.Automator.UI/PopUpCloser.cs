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
        public string ProcessName { get; set; }
        public List<PopUpConfig> Configs { get; set; }

        public void Execute()
        {
            Init();
            _windows = GetWindows();
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var winTask = new Task(() => CloseWindows());
            var dialogTask = new Task(() => ClosePopUps(), token);
            winTask.Start();
            dialogTask.Start();
            winTask.Wait(60000 * 5);
            tokenSource.Cancel();
            Debug.WriteLine("Task");
            dialogTask.Dispose();
        }

        private void CloseWindows()
        {
            var windows = GetWindows();
            var count = windows.Count;
            while(count > 0)
            {
                windows[0].Close();
                windows = GetWindows();
                count = windows.Count;
                Thread.Sleep(1000);
            }
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

        private void CloseWindow(WindowElement win)
        {
            var closeBtn = win.Query(@"//Element[@Type='title bar']/Children/Element[@Type='button' and @Name='Close']").FirstOrDefault();
            closeBtn.Click();
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


        private List<WindowElement> _windows;

        private void Init()
        {
            ProcessName = "OUTLOOK";
            Configs.Add(new PopUpConfig() { ButtonxPath = @"//Element[@Type='button' and @Name='No']", TitleRegEx = "Microsoft Outlook" });
        }
    }
}
