using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Media.Playback;

namespace SmallGoal
{
    /// <summary>
    /// 时间管理详情页面
    /// </summary>
    public sealed partial class timePageDetail : Page
    {
        Models.MyGoalItem goalItem;

        public timePageDetail()
        {
            this.InitializeComponent();
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

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(timePage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            goalItem = ((App)App.Current).myViewModel.selectedItem;
            goalItem.TimeToString();
            goalItem.changeString();
            TimeWillSpend.Text = goalItem.totalGoalString;
            TimeNeedToSpend.Text = goalItem.needGoalString;
            countTimeButton.Icon = new SymbolIcon(goalItem.isCountingTime ? Symbol.Stop : Symbol.Play); // play button
        }


        private async void countTime_Click(object sender, RoutedEventArgs e)
        {
            if (!goalItem.isCountingTime)  // count
            {
                goalItem.isCountingTime = true;
                countTimeButton.Icon = new SymbolIcon(Symbol.Stop);
                goalItem.countingStart = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                await Task.Delay(1000);
                long seconds = goalItem.countedSeconds;

                while (goalItem.isCountingTime)
                {
                    seconds = goalItem.countedSeconds + DateTimeOffset.UtcNow.ToUnixTimeSeconds() - goalItem.countingStart;
                    goalItem.usedSecond = (int)(seconds % 60);
                    goalItem.usedMinute = (int)(seconds / 60) % 60;
                    goalItem.usedHour = (int)(seconds / 3600) % 24;
                    goalItem.usedDay = (int)(seconds / 86400);
                    goalItem.changeString();
                    TimeWillSpend.Text = goalItem.totalGoalString;
                    TimeNeedToSpend.Text = goalItem.needGoalString;
                    await Task.Delay(1000);
                }
                goalItem.countedSeconds = seconds;

            } else { // paused
                goalItem.isCountingTime = false;
                countTimeButton.Icon = new SymbolIcon(Symbol.Play);
            }
        }
    }


    /*------------------------- 转换器 ---------------------------*/
    class DoubleToTimeSpan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.FromSeconds((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).TotalSeconds;
        }
    }

}
