using Boxes.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Boxes.View
{
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer;
        double panelWidth;
        bool isHidden;

        private readonly Manager _manager;

        public MainWindow()
        {
            InitializeComponent();
            _manager = new Manager(50, 200);
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _timer.Tick += Timer_Tick;
            panelWidth = sidePanel.Width;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isHidden)
            {
                sidePanel.Width += 1;
                if (sidePanel.Width >= panelWidth)
                {
                    _timer.Stop();
                    isHidden = false;
                }
            }
            else
            {
                sidePanel.Width -= 1;
                if (sidePanel.Width <=60)
                {
                    _timer.Stop();
                    isHidden = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void PanelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
