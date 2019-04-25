using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace SmallGoal
{
    /// <summary>
    /// 计划管理页面
    /// </summary>
    public sealed partial class planPage : Page
    {
        // viewmodel在导航进入页面时已经传递进来，大家可以直接使用
        ViewModels.MyGoalViewModel myViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //  将viewmodel传递，这是给大家提供的viewmodel
            myViewModel = ((App)App.Current).myViewModel;
        }

        public planPage()
        {
            this.InitializeComponent();
        }

        /*------------------- 点击函数 ------------------------*/
        private void mainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "");
        }

        private void timePageButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Models.MyGoalItem> items = myViewModel.findDayGoalCollection(2017, 5, 8);
            time.DataContext = items;
            Frame.Navigate(typeof(timePage), "");
        }

        /*-------------通过动态绑定显示右侧的item-----------------*/
        private void calendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (calendar.SelectedDates.Count == 0) return;
            DateTimeOffset sourceTime = calendar.SelectedDates[0];
            ObservableCollection<Models.MyGoalItem> items = myViewModel.findDayGoalCollection(calendar.SelectedDates[0].Year, calendar.SelectedDates[0].Month, calendar.SelectedDates[0].Day);
            time.ItemsSource = items;
            name.ItemsSource = items;
            totaltime.ItemsSource = items;

            // 待完成的月计划
            ObservableCollection<Models.MyGoalItem> monthItems = myViewModel.findMonthGoalCollection(calendar.SelectedDates[0].Year, calendar.SelectedDates[0].Month, 0);
            monthPlan.ItemsSource = monthItems;
            // 实际已完成的月计划
            ObservableCollection<Models.MyGoalItem> realmonthItems = myViewModel.findMonthGoalCollection(calendar.SelectedDates[0].Year, calendar.SelectedDates[0].Month, 1);
            realMonthPlan.ItemsSource = realmonthItems;

            // 待完成的年计划
            ObservableCollection<Models.MyGoalItem> yearItems = myViewModel.findYearGoalCollection(calendar.SelectedDates[0].Year, calendar.SelectedDates[0].Month, 0);
            yearPlan.ItemsSource = yearItems;
            // 实际完成的年计划
            ObservableCollection<Models.MyGoalItem> realYealItems = myViewModel.findYearGoalCollection(calendar.SelectedDates[0].Year, calendar.SelectedDates[0].Month, 1);
            realYealPlan.ItemsSource = realYealItems;
        }

        /*---------------- 根据计划类型改变右侧内容 -------------------*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dayPlanList.Visibility = Visibility.Collapsed;
            monthPlanList.Visibility = Visibility.Collapsed;
            yearPlanList.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dayPlanList.Visibility = Visibility.Collapsed;
            monthPlanList.Visibility = Visibility.Visible;
            yearPlanList.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dayPlanList.Visibility = Visibility.Visible;
            monthPlanList.Visibility = Visibility.Collapsed;
            yearPlanList.Visibility = Visibility.Collapsed;
        }

        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            dayPlanList.Visibility = Visibility.Visible;
            monthPlanList.Visibility = Visibility.Collapsed;
            yearPlanList.Visibility = Visibility.Collapsed;
        }
    }
}
