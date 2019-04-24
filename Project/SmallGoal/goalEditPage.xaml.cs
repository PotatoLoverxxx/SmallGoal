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
        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            string warningMessage = "";

            if (TargetNameEditor.Text.Length > 16) warningMessage = "目标的名称不能够超过15字符长度哦~";
            if (TargetNote.Text.Length > 101) warningMessage = "目标的备注不能够超过100字符长度哦~";

            switch (type)
            {
                case 0:
                    if (StartDate.Date.Day != EndDate.Date.Day || StartDate.Date.Month != EndDate.Date.Month ||
                        StartDate.Date.Year != EndDate.Date.Year)
                    {
                        warningMessage = "日目标只能设置在同一天哦(⊙o⊙)…\n请改为月（年）目标吧(^_^)";
                    }
                    break;
                case 1:
                    if (StartDate.Date.Month != EndDate.Date.Month || StartDate.Date.Year != EndDate.Date.Year)
                    {
                        warningMessage = "月目标只能设置在同一月哦(⊙o⊙)…\n请改为年目标吧(^_^)";
                    }
                    break;
                case 2:
                    if (StartDate.Date.Year != EndDate.Date.Year)
                    {
                        warningMessage = "年目标只能设置在同一年哦(⊙o⊙)…\n请创建多个年目标吧(^_^)";
                    }
                    break;
            }

            if (StartDate.Date > EndDate.Date || StartTime.Time > EndTime.Time)
            {
                warningMessage = "结束早过开始啦(⊙o⊙)…";
            }

            if (warningMessage == "")
            {
                if (deleteButton.Label == "取消")   // 新建目标
                {
                    ((App)App.Current).myViewModel.AddMyGoalItem(TargetNameEditor.Text, type, StartDate.Date.Year,
                        StartDate.Date.Month, StartDate.Date.Day, StartTime.Time.Hours, StartTime.Time.Minutes,
                        EndDate.Date.Year, EndDate.Date.Month, EndDate.Date.Day, EndTime.Time.Hours, EndTime.Time.Minutes,
                        TargetNote.Text, 0, 0, 0, 0, 0, 0, 0);
                }
                else         // 修改目标
                {
                    ((App)App.Current).myViewModel.nameChange(TargetNameEditor.Text);
                    ((App)App.Current).myViewModel.typeChange(type);

                    ((App)App.Current).myViewModel.startHourChange(StartTime.Time.Hours);
                    ((App)App.Current).myViewModel.startMinuteChange(StartTime.Time.Minutes);
                    ((App)App.Current).myViewModel.startYearChange(StartDate.Date.Year);
                    ((App)App.Current).myViewModel.startMonthChange(StartDate.Date.Month);
                    ((App)App.Current).myViewModel.startDayChange(StartDate.Date.Day);

                    ((App)App.Current).myViewModel.endHourChange(EndTime.Time.Hours);
                    ((App)App.Current).myViewModel.endMinuteChange(EndTime.Time.Minutes);
                    ((App)App.Current).myViewModel.endYearChange(EndDate.Date.Year);
                    ((App)App.Current).myViewModel.endMonthChange(EndDate.Date.Month);
                    ((App)App.Current).myViewModel.endDayChange(EndDate.Date.Day);

                    ((App)App.Current).myViewModel.noteChange(TargetNote.Text);
                    ((App)App.Current).myViewModel.timeStringChange();
                }

                ((App)App.Current).myViewModel.selectedItem = null;
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "提示",
                    Content = warningMessage,
                    PrimaryButtonText = "确定"
                };
                ContentDialogResult result = await dialog.ShowAsync();
            }
        }

        // 目标类型改动
        private void TargetType_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            type = 0;  // 0：日目标. 1：月目标. 2：年目标
            if (rb != null)
            {
                string dateType = rb.Tag.ToString();

                // 初始化起始时间和结束时间
                if (StartTime != null && StartDate != null && EndTime != null && EndDate != null)
                {
                    StartDate.Date = EndDate.Date = DateTimeOffset.Now;
                    StartTime.Time = EndTime.Time = new TimeSpan(DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second);
                }

                switch (dateType)
                {
                    case "DayTarget":
                        if (StartTime != null && StartDate != null && EndTime != null && EndDate != null)
                        {
                            StartTime.Visibility = Visibility.Visible;
                            StartDate.DayVisible = true;
                            EndTime.Visibility = Visibility.Visible;
                            EndDate.DayVisible = true;
                        }
                        type = 0;
                        break;
                    case "MonthTarget":
                        StartTime.Visibility = EndTime.Visibility = Visibility.Collapsed;
                        StartDate.DayVisible = EndDate.DayVisible = true;
                        type = 1;
                        break;
                    case "YearTarget":
                        StartTime.Visibility = EndTime.Visibility = Visibility.Collapsed;
                        StartDate.DayVisible = EndDate.DayVisible = false;
                        type = 2;
                        break;
                }
            }
        }

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
