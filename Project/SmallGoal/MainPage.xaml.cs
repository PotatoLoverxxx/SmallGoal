using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Windows.UI.Popups;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace SmallGoal
{
    /// <summary>
    /// 主页面
    /// </summary>   
    public sealed partial class MainPage : Page
    {
        private int type;   // 目标类型. 0：日目标. 1：月目标. 2：年目标

        public MainPage()
        {
            this.InitializeComponent();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataRequested;
        }

        /*----------------------------网络访问：获取天气部分----------------------------*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            weather.Text = "";
            temperature.Text = "";
            GetWeather(cityName.Text);

        }
        private async void GetWeather(string tel)
        {
            try
            {
                // 创建一个HTTP client实例对象
                HttpClient httpClient = new HttpClient();

                // Add a user-agent header to the GET request. 

                /*默认情况下，HttpClient对象不会将用户代理标头随 HTTP 请求一起发送到 Web 服务。
                某些 HTTP 服务器（包括某些 Microsoft Web 服务器）要求从客户端发送的 HTTP 请求附带用户代理标头。
                如果标头不存在，则 HTTP 服务器返回错误。
                在 Windows.Web.Http.Headers 命名空间中使用类时，需要添加用户代理标头。
                我们将该标头添加到 HttpClient.DefaultRequestHeaders 属性以避免这些错误。*/

                var headers = httpClient.DefaultRequestHeaders;

                // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                // especially if the header value is coming from user input.
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }
                // API 链接
                string getCityCode = "http://api.avatardata.cn/Weather/Query?key=e81ecb57345c46ef99a9c74bfcdb5d0b&cityname=" + cityName.Text;

                //发送GET请求
                HttpResponseMessage response = await httpClient.GetAsync(getCityCode);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                // 因为返回的字节流中含有中文，传输过程中，所以需要编码后才可以正常显示
                // “\u5e7f\u5dde”表示“广州”，\u表示Unicode
                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码，为了保障中文的显示
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);

                JsonTextReader json1 = new JsonTextReader(new StringReader(result));   // 实例化json数据
                string flag = "";
                while (json1.Read())  // 读取json数据，赋值给flag
                {
                    flag += json1.Value;
                }
                int error1 = 0;
                int.TryParse(flag.IndexOf("error_code").ToString(), out error1);
                string error = flag.Substring(error1 + 10, 1);
                if (error == "0")
                {
                    int m1 = 0, m2 = 0;
                    int.TryParse(flag.IndexOf("info").ToString(), out m1);
                    int.TryParse(flag.IndexOf("temperature").ToString(), out m2);
                    //m2 = flag.IndexOf("mobiletype").ToString();
                    string Weatherinfo = flag.Substring(m1 + 4, m2 - m1 - 4);

                    int m3 = 0;
                    int.TryParse(flag.IndexOf("dataUptime").ToString(), out m3);
                    string Temperature = flag.Substring(m2 + 11, m3 - m2 - 11);
                    weather.Text = Weatherinfo;
                    temperature.Text = Temperature + "℃";

                }
                else
                {
                    var i = new MessageDialog("");
                    i.Content = "Please input the correct city name(using Chinese name)";
                    await i.ShowAsync();
                }


            }
            catch (HttpRequestException ex1)
            {
                weather.Text = "出问题了哟，请重新输入~";
                Debug.WriteLine(ex1.ToString());
            }
            catch (Exception ex2)
            {
                weather.Text = "请重新输入正确城市哦~";
                Debug.WriteLine(ex2.ToString());
            }
        }
        // viewmodel在导航进入页面时已经传递进来，大家可以直接使用
        ViewModels.MyGoalViewModel myViewModel { get; set; }


        /*--------------------------------- 磁贴更新----------------------------------------*/
        public delegate void notificationqueueCycle(object sender, EventArgs e);
        public event notificationqueueCycle cycle;

        private void Add(object sender, EventArgs e)
        {
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            for (int i = 0; i < myViewModel.allItems.Count; i++)
            {
                string t = myViewModel.allItems[i].name;
                string d = myViewModel.allItems[i].note;
                UpdateTile(t, d);
            }
        }

        public void UpdateTile(string name, string note)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(File.ReadAllText("Tile.xml"));
            XmlNodeList texts = xml.GetElementsByTagName("text");
            for (int i = 0; i < texts.Count; i++)
            {
                ((XmlElement)texts[i]).InnerText = name;
                i++;
                ((XmlElement)texts[i]).InnerText = note;
            }
            TileNotification notification = new TileNotification(xml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
        /*------------------------------- 磁贴更新 ---------------------------------------*/



        /*------------------------------导航部分-------------------------------*/
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            myViewModel = ((App)App.Current).myViewModel;
            // 磁贴更新操作
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            this.cycle += new notificationqueueCycle(Add);
            Add(this, EventArgs.Empty);
            //  将viewmodel传递，这是给大家提供的viewmodel

            /*****************************************************
             * 目标编辑部分
             *****************************************************/
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
                    case 0:
                        DayTarget.IsChecked = true;
                        StartDate.Date = new DateTimeOffset(new DateTime(item.startYear, item.startMonth, item.startDay));
                        StartTime.Time = new TimeSpan(item.startHour, item.startMinute, 0);
                        EndDate.Date = new DateTimeOffset(new DateTime(item.endYear, item.endMonth, item.endDay));
                        EndTime.Time = new TimeSpan(item.endHour, item.endMinute, 0);
                        break;
                    case 1:
                        MonthTarget.IsChecked = true;
                        StartDate.Date = new DateTimeOffset(new DateTime(item.startYear, item.startMonth, item.startDay));
                        EndDate.Date = new DateTimeOffset(new DateTime(item.endYear, item.endMonth, item.endDay));
                        break;
                    case 2:
                        YearTarget.IsChecked = true;
                        StartDate.Date = new DateTimeOffset(new DateTime(item.startYear, item.startMonth, 1));
                        EndDate.Date = new DateTimeOffset(new DateTime(item.endYear, item.endMonth, 1));
                        break;
                    default: break;
                }

                TargetNote.Text = item.note;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            this.cycle -= new notificationqueueCycle(Add);
            DataTransferManager.GetForCurrentView().DataRequested -= DataRequested;
        }

        /*---------------------------目标编辑部分--------------------------------*/
        private void cleanPage()
        {
            TargetNameEditor.Text = "";
            DayTarget.IsChecked = true;
            StartDate.Date = DateTimeOffset.Now;
            StartTime.Time = new TimeSpan(DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second);
            EndDate.Date = DateTimeOffset.Now;
            EndTime.Time = new TimeSpan(DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second);
            TargetNote.Text = "";
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            cleanPage();
            if (deleteButton.Label == "删除") // 删除目标
            { 
                ((App)App.Current).myViewModel.RemoveMyGoalItem();
                cleanPage();
                deleteButton.Icon = new SymbolIcon(Symbol.Cancel);
                deleteButton.Label = "取消";
            }
            ((App)App.Current).myViewModel.selectedItem = null;
        }

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

    }

    /*------------------转换器---------------------------*/
    public class CConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if ((int)value == 0) return false;
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            if ((bool)value == false) return 0;
            return 1;
        }
    }


}