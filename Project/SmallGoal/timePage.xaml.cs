using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”   项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace SmallGoal
{
    /// <summary>
    /// 时间管理页面
    /// </summary>
    public sealed partial class timePage : Page
    {
        // viewmodel在导航进入页面时已经传递进来，大家可以直接使用
        ViewModels.MyGoalViewModel myViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //  将viewmodel传递，这是给大家提供的viewmodel
            myViewModel = ((App)App.Current).myViewModel;
        }

        public timePage()
        {
            this.InitializeComponent();
        }

        private void mainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "");
        }

        private void planPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(planPage), "");
        }

        private void GoalItem_ItemClicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(timePageDetail), "");
        }
    }
}
