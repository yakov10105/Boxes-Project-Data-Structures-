using Boxes.Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Boxes.UiView
{
    public partial class MainWindow : Window
    {

        private readonly DispatcherTimer _timer;
        private readonly double _panelWidth;
        private bool _isHidden;
        private readonly IManager _iManager;

        public MainWindow()
        {
            InitializeComponent();
            _iManager = new Manager(50, 200, 2);
            _ = new StockInitializer(_iManager);
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1)
            };
            _timer.Tick += Timer_Tick;
            _panelWidth = sidePanel.Width;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isHidden)
            {
                sidePanel.Width += 5;
                AddGrid.Width -= 5;
                BuyGrid.Width -= 5;
                ShowGrid.Width -= 5;
                if (sidePanel.Width >= _panelWidth)
                {
                    _timer.Stop();
                    _isHidden = false;
                }
            }
            else
            {
                sidePanel.Width -= 5;
                AddGrid.Width += 5;
                BuyGrid.Width += 5;
                ShowGrid.Width += 5;
                if (sidePanel.Width <= 60)
                {
                    _timer.Stop();
                    _isHidden = true;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e) => _timer.Start();
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _iManager.CheckExpires();
            this.Close();
        }
        private void PanelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView lv)
            {
                if (lv.SelectedItem is ListViewItem lvi)
                {
                    switch (lvi.Name)
                    {
                        case "lviAdd":
                            AddGrid.Visibility = Visibility.Visible;
                            BuyGrid.Visibility = Visibility.Collapsed;
                            ShowGrid.Visibility = Visibility.Collapsed;
                            break;
                        case "lviBuy":
                            BuyGrid.Visibility = Visibility.Visible;
                            AddGrid.Visibility = Visibility.Collapsed;
                            ShowGrid.Visibility = Visibility.Collapsed;
                            break;
                        case "lviShow":
                            ShowGrid.Visibility = Visibility.Visible;
                            BuyGrid.Visibility = Visibility.Collapsed;
                            AddGrid.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tbx)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(tbx.Text, "[^0-9.]"))
                {

                    tbx.Text = tbx.Text.Remove(tbx.Text.Length - 1);
                }
            }

        }

        #region IManager Events
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var width = double.Parse(tbxWidthAdd.Text);
                var height = double.Parse(tbxHeightAdd.Text);
                var qty = int.Parse(tbxQtyAdd.Text);
                if (width != default && height != default)
                {
                    if (qty != default)
                        _iManager.AddBoxes(width, height, qty);
                    else
                        _iManager.AddBoxes(width, height);
                    tbxWidthAdd.Text = "";
                    tbxHeightAdd.Text = "";
                    tbxQtyAdd.Text = "";
                }
            }
            catch (Exception)
            {
                tbxWidthAdd.Text = "";
                tbxHeightAdd.Text = "";
                tbxQtyAdd.Text = "";
                MessageBox.Show("You must enter valid Numbers !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var width = double.Parse(tbxWidthBuy.Text);
                var height = double.Parse(tbxHeightBuy.Text);
                var qty = int.Parse(tbxQtyBuy.Text);
                if (width != default && height != default)
                {
                    if (qty != default)
                        _iManager.FindBoxesToBuy(width, height, qty);
                    else
                        _iManager.FindBoxesToBuy(width, height);
                    tbxWidthBuy.Text = "";
                    tbxHeightBuy.Text = "";
                    tbxQtyBuy.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You must enter valid Numbers !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbxWidthBuy.Text = "";
                tbxHeightBuy.Text = "";
                tbxQtyBuy.Text = "";
                return;
            }
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lvSearch.Items.Clear();
                var width = double.Parse(tbxWidthSearch.Text);
                var height = double.Parse(tbxHeightSearch.Text);
                if (width != default && height != default)
                {
                    var item = _iManager.ShowBox(width, height);
                    if (item != null)
                    {
                        lvSearch.Items.Add(item);
                        tbxHeightSearch.Text = "";
                        tbxWidthSearch.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Item Not Found ...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        tbxHeightSearch.Text = "";
                        tbxWidthSearch.Text = "";
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You must enter valid Numbers !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbxHeightSearch.Text = "";
                tbxWidthSearch.Text = "";
                return;
            }
        }
        #endregion
    }
}
