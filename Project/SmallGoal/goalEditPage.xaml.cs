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
using SmallGoal.ViewModels;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace SmallGoal
{
    /// <summary>
    /// 目标编辑页面
    /// </summary>
    public sealed partial class goalEditPage : Page
    {
        private int type;   // 目标类型. 0：日目标. 1：月目标. 2：年目标

        public goalEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = ((App)App.Current).myViewModel.selectedItem;
            if (item == null)   // 创建新目标
            {
                deleteButton.Label = "取消";
                deleteButton.Icon = new SymbolIcon(Symbol.Cancel);
            }
            else  // 修改目标
            {
                deleteButton.Label = "删除";
                deleteButton.Icon = new SymbolIcon(Symbol.Delete);

                TargetNameEditor.Text = item.name;

                switch (item.type)
                {
                    case 0: DayTarget.IsChecked = true;
                        StartDate.Date = new DateTimeOffset(new DateTime(item.startYear, item.startMonth, item.startDay));
                        StartTime.Time = new TimeSpan(item.startHour, item.startMinute, 0);
                        EndDate.Date = new DateTimeOffset(new DateTime(item.endYear, item.endMonth, item.endDay));
                        EndTime.Time = new TimeSpan(item.endHour, item.endMinute, 0);
                        break;
                    case 1: MonthTarget.IsChecked = true;
                        StartDate.Date = new DateTimeOffset(new DateTime(item.startYear, item.startMonth, item.startDay));
                        EndDate.Date = new DateTimeOffset(new DateTime(item.endYear, item.endMonth, item.endDay));
                        break;
                    case 2: YearTarget.IsChecked = true;
                        StartDate.Date = new DateTimeOffset(new DateTime(item.startYear, item.startMonth, 1));
                        EndDate.Date = new DateTimeOffset(new DateTime(item.endYear, item.endMonth, 1));
                        break;
                    default: break;
                }

                TargetNote.Text = item.note;
            }
        }

        // 删除目标、取消
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteButton.Label == "取消") // 取消编辑并回退页面
            {
                TargetNameEditor.Text = "";
                DayTarget.IsChecked = true;
                StartDate.Date = DateTimeOffset.Now;
                StartTime.Time = new TimeSpan(DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second);
                EndDate.Date = DateTimeOffset.Now;
                EndTime.Time = new TimeSpan(DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second);
                TargetNote.Text = "";
            }
            else   // 删除目标
            {
                ((App)App.Current).myViewModel.RemoveMyGoalItem();
            }
            ((App)App.Current).myViewModel.selectedItem = null;
            Frame.Navigate(typeof(MainPage));
        }

        // 更新目标、新建目标
        

        // 目标类型改动
        

        /*-----------------------------导航--------------------------------*/
        private void navigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).myViewModel.selectedItem = null;
            Frame.Navigate(typeof(MainPage));
        }

        private void mainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "");
        }

        private void timePageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(timePage), "");
        }

        private void planPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(planPage), "");
        }

    }
}
