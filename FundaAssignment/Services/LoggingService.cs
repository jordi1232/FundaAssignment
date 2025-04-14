using System.Windows.Controls;

namespace FundaAssignment.Services
{
    /// <summary>
    /// This is a singleton so it is accessible everywhere without 
    /// </summary>
    public sealed class LoggingService
    {
        private static LoggingService? instance = null;
        private static readonly object padlock = new object();

        private TextBox? _textBox;

        private LoggingService()
        {
        }

        public static LoggingService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new LoggingService();
                    }
                    return instance;
                }
            }
        }

        public void SetTextbox(TextBox box)
        {
            _textBox = box;
        }

        public void Log(string message)
        {
            if (_textBox != null)
            {
                _textBox.Dispatcher.Invoke(() =>
                {
                    _textBox.Text += message + Environment.NewLine;
                });
            }
        }

        public void Clear()
        {
            if (_textBox != null)
            {
                _textBox.Dispatcher.Invoke(() =>
                {
                    _textBox.Clear();
                });
            }
        }
    }
}
