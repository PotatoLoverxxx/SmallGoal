using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SmallGoal
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        private ViewModels.MyGoalViewModel _myViewModel;
        public ViewModels.MyGoalViewModel myViewModel { get { return this._myViewModel; } set { this._myViewModel = value; } }
        public static SQLiteConnection database;
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            database = new SQLiteConnection("SmallGoalDB.db");
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;
            myViewModel = new ViewModels.MyGoalViewModel();
            //从数据库中加载数据
            LoadDatabase();

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            //保存数据
            SaveDatabase();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }

        /*-------------------------数据库部分----------------------------*/
        // 加载数据
        private void LoadDatabase()
        {
            // 如果表不存在则创建表，加载数据
            string sql = @"CREATE TABLE IF NOT EXISTS GoalItem (name    VARCHAR( 20 ),
                                                                type    INTEGER,
                                                                startYear    INTEGER,
                                                                startMonth    INTEGER,
                                                                startDay    INTEGER,
                                                                startHour    INTEGER,
                                                                startMinute    INTEGER,
                                                                endYear    INTEGER,
                                                                endMonth    INTEGER,
                                                                endDay    INTEGER,
                                                                endHour    INTEGER,
                                                                endMinute    INTEGER,
                                                                note VARCHAR( 200 ),
                                                                isFinished    INTEGER,
                                                                usedYear    INTEGER,
                                                                usedMonth    INTEGER,
                                                                usedDay    INTEGER,
                                                                usedHour    INTEGER,
                                                                usedMinute    INTEGER,
                                                                usedSecond    INTEGER
                                                                );";
            using (var statement = App.database.Prepare(sql))
            {
                statement.Step();
            }
            // 将数据从数据库加载到程序中来
            using (var statement = database.Prepare("SELECT * FROM GoalItem"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    
                    myViewModel.AddMyGoalItem((string)statement[0], Convert.ToInt32((long)statement[1]), Convert.ToInt32((long)statement[2]), Convert.ToInt32((long)statement[3]),
                        Convert.ToInt32((long)statement[4]), Convert.ToInt32((long)statement[5]),Convert.ToInt32((long)statement[6]), Convert.ToInt32((long)statement[7]), Convert.ToInt32((long)statement[8]),
                        Convert.ToInt32((long)statement[9]), Convert.ToInt32((long)statement[10]), Convert.ToInt32((long)statement[11]), (string)statement[12], Convert.ToInt32((long)statement[13]),
                        Convert.ToInt32((long)statement[14]), Convert.ToInt32((long)statement[15]), Convert.ToInt32((long)statement[16]), Convert.ToInt32((long)statement[17]), Convert.ToInt32((long)statement[18]), Convert.ToInt32((long)statement[19]));
                }
            }

            // 这里是只是一些小测试，加载了数据后把这些删除
            //myViewModel.AddMyGoalItem("日计划", 0, 2017, 5, 9, 10, 0, 2017, 5, 9, 22, 0, "日计划的详情", 0, 0, 0, 0, 9, 12, 0);
            //myViewModel.AddMyGoalItem("月计划", 1, 2017, 5, 9, 10, 0, 2017, 5, 10, 22, 0, "月计划的详情", 0, 0, 0, 0, 9, 12, 0);
            //myViewModel.AddMyGoalItem("年计划", 2, 2016, 5, 9, 10, 0, 2016, 6, 9, 22, 0, "年计划的详情", 0, 0, 0, 0, 9, 12, 0);
        }

        // 保存数据
        private void SaveDatabase()
        {
            deleteDataFromDataBase();
            for (int i = 0; i < myViewModel.allItems.Count; i++)
            {
                insertDataToDataBase(i);
            }
        }

        // 插入数据到数据库中
        private void insertDataToDataBase(int i)
        {
            using (var newGoalItem = database.Prepare("INSERT INTO GoalItem (name, type, startYear, startMonth, startDay, startHour, startMinute, endYear, endMonth, endDay, endHour, endMinute, note, isFinished, usedYear, usedMonth, usedDay, usedHour, usedMinute, usedSecond) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"))
            {
                newGoalItem.Bind(1, myViewModel.allItems[i].name);
                newGoalItem.Bind(2, myViewModel.allItems[i].type);
                newGoalItem.Bind(3, myViewModel.allItems[i].startYear);
                newGoalItem.Bind(4, myViewModel.allItems[i].startMonth);
                newGoalItem.Bind(5, myViewModel.allItems[i].startDay);
                newGoalItem.Bind(6, myViewModel.allItems[i].startHour);
                newGoalItem.Bind(7, myViewModel.allItems[i].startMinute);
                newGoalItem.Bind(8, myViewModel.allItems[i].endYear);
                newGoalItem.Bind(9, myViewModel.allItems[i].endMonth);
                newGoalItem.Bind(10, myViewModel.allItems[i].endDay);
                newGoalItem.Bind(11, myViewModel.allItems[i].endHour);
                newGoalItem.Bind(12, myViewModel.allItems[i].endMinute);
                newGoalItem.Bind(13, myViewModel.allItems[i].note);
                newGoalItem.Bind(14, myViewModel.allItems[i].isFinished);
                newGoalItem.Bind(15, myViewModel.allItems[i].usedYear);
                newGoalItem.Bind(16, myViewModel.allItems[i].usedMonth);
                newGoalItem.Bind(17, myViewModel.allItems[i].usedDay);
                newGoalItem.Bind(18, myViewModel.allItems[i].usedHour);
                newGoalItem.Bind(19, myViewModel.allItems[i].usedMinute);
                newGoalItem.Bind(20, myViewModel.allItems[i].usedSecond);
                newGoalItem.Step();
            }
        }

        // 删除所有item
        private void deleteDataFromDataBase()
        {
            using (var statement = App.database.Prepare("DELETE FROM GoalItem"))
            {
                statement.Step();
            }
        }

    }
}
