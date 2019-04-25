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
                goalItem.deltaS = goalItem.countedSeconds;

                await Task.Delay(1000);
                while (goalItem.isCountingTime)
                {
                    goalItem.countedSeconds = goalItem.deltaS + DateTimeOffset.UtcNow.ToUnixTimeSeconds() - goalItem.countingStart;
                    goalItem.changeString();
                    TimeWillSpend.Text = goalItem.totalGoalString;
                    TimeNeedToSpend.Text = goalItem.needGoalString;
                    await Task.Delay(1000);
                }

            }
            else
            { // paused
                goalItem.isCountingTime = false;
                countTimeButton.Icon = new SymbolIcon(Symbol.Play);
            }
        }

        /* more timekeeping utilities */


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            int index = ((App)App.Current).myViewModel.allItems.IndexOf(((App)App.Current).myViewModel.selectedItem);
            ((App)App.Current).myViewModel.allItems[index] = goalItem;
            ((App)App.Current).myViewModel.selectedItem = null;
        }

        // 计时
        private async void _LostFocus(object sender, RoutedEventArgs e)
        {
            int count = 0;
            int sec, min, hour, day;
            string warningMessage = "";

            
            if (int.TryParse(usedSecondTextBox.Text, out count) && int.TryParse(usedMinuteTextBox.Text, out count)
                && int.TryParse(usedHourTextBox.Text, out count) && int.TryParse(usedDayTextBox.Text, out count))
            {
                sec = int.Parse(usedSecondTextBox.Text);
                min = int.Parse(usedMinuteTextBox.Text);
                hour = int.Parse(usedHourTextBox.Text);
                day = int.Parse(usedDayTextBox.Text);

                if (!(0 <= sec && sec < 60 && 0 <= min && min < 60 && 0 <= hour && hour < 24 && day >= 0))
                {
                    warningMessage = "输入有误，请重试";
                }
                else
                {
                    goalItem.countingStart = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    goalItem.deltaS += sec + min * 60 + hour * 3600 + day * 86400 - goalItem.countedSeconds;
                    goalItem.countedSeconds = sec + min * 60 + hour * 3600 + day * 86400;
                    goalItem.changeString();
                    TimeWillSpend.Text = goalItem.totalGoalString;
                    TimeNeedToSpend.Text = goalItem.needGoalString;
                }
            }
            else
            {
                warningMessage = "非法输入！";
            }

            if (warningMessage != "")
            {
                usedSecondTextBox.Text = goalItem.usedSecond.ToString();
                usedMinuteTextBox.Text = goalItem.usedMinute.ToString();
                usedHourTextBox.Text = goalItem.usedHour.ToString();
                usedDayTextBox.Text = goalItem.usedDay.ToString();

                ContentDialog dialog = new ContentDialog()
                {
                    Title = "提示",
                    Content = warningMessage,
                    PrimaryButtonText = "确定"
                };
                ContentDialogResult result = await dialog.ShowAsync();
            }
        }




        /* --- media --- */
        bool isVolumeSlideShow = false;
        private async void pickFileButton_Click(object sender, RoutedEventArgs e)
        {

            // Create and open the file picker
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            openPicker.FileTypeFilter.Add(".mp3");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                mediaPlayerElement.SetSource(stream, file.ContentType);
                mediaPlayerElement.Play();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerElement.Pause();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerElement.Play();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerElement.Stop();
        }

        private void VoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (isVolumeSlideShow == false)
            {
                volumeSlider.Opacity = 1;
                isVolumeSlideShow = true;
            }
            else
            {
                volumeSlider.Opacity = 0;
                isVolumeSlideShow = false;
            }
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (mediaPlayerElement != null) mediaPlayerElement.Volume = (double)(volumeSlider.Value / 100);
        }

        private void mediaPlayerElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            timelineSlider.Value = 0;
            timelineSlider.Maximum = mediaPlayerElement.NaturalDuration.TimeSpan.TotalSeconds;
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
