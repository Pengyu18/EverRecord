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

namespace EverRecord
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();          

            //菜单最小化、最大化、关闭
            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnClose.Click += async (s, e) =>
            {
                //var dialogResult = await dialogHostService.Question("温馨提示", "确认退出系统?");
                //if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                this.Close();
            };

            //鼠标
            //鼠标拖动事件
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };
            //双击最大化
            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };


            //导航栏UI
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };

            //导航窗口切换
            frame.Content = new PartialViews.IndexView();
            indexBtn.Click += (s, e) => {
                //frame.Source = new Uri("PartialViews.IndexView.xaml", UriKind.Relative);
                frame.Content = new PartialViews.IndexView();
            };
            entireBtn.Click += (s, e) => {
                frame.Content = new PartialViews.EntireView();
            };
            searchBtn.Click += (s, e) => {
                frame.Content = new PartialViews.SearchView();
            };

        }
    }
}
