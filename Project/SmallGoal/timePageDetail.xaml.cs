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
