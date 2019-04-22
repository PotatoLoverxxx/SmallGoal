using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmallGoal.ViewModels
{
    /// <summary>
    /// ViewModel提供Model和前端的接口
    /// </summary>
    public class MyGoalViewModel : INotifyPropertyChanged
    {
        public MyGoalViewModel() {}
        private ObservableCollection<Models.MyGoalItem> _allItems = new ObservableCollection<Models.MyGoalItem>();
        // 供给外面的goalItem集合
        public ObservableCollection<Models.MyGoalItem> allItems { get { return this._allItems; } set { this._allItems = value; this.NotifyPropertyChanged(); } }

        private Models.MyGoalItem _selectedItem = default(Models.MyGoalItem);

        // 供给外面的selectedItem被选中item
        public Models.MyGoalItem selectedItem { get { return _selectedItem; } set { this._selectedItem = value; this.NotifyPropertyChanged(); } }

        /*----------------------以下为给大家使用的增删改查操作-------------------------*/
        // 添加goalItem
        public void AddMyGoalItem(string name, int type, int startYear, int startMonth, int startDay, int startHour, int startMinute,
            int endYear, int endMonth, int endDay, int endHour, int endMinute, string note, int isFinished,
            int usedYear, int usedMonth, int usedDay, int usedHour, int usedMinute, int usedSecond)
        {
            this._allItems.Add(new Models.MyGoalItem(name, type, startYear, startMonth, startDay, startHour, startMinute,
            endYear, endMonth, endDay, endHour, endMinute, note, isFinished, usedYear, usedMonth, usedDay, usedHour, usedMinute, usedSecond));
        }

        /*--------------------删除------------------*/
        // 删除选中的goalItem
        public void RemoveMyGoalItem()
        {
            this._allItems.Remove(selectedItem);
            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        /*------------------更新-------------------*/
        // 更新选中的goalItem的name属性
        public void nameChange(string name)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].name = name;
        }

        // 更新选中的goalItem的type属性
        public void typeChange(int type)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].type = type;
        }

        // 更新选中的goalItem的startYear属性
        public void startYearChange(int startYear)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].startYear = startYear;
        }

        // 更新选中的goalItem的startMonth属性
        public void startMonthChange(int startMonth)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].startMonth = startMonth;
        }

        // 更新选中的goalItem的startDay属性
        public void startDayChange(int startDay)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].startDay = startDay;
        }

        // 更新选中的goalItem的startHour属性
        public void startHourChange(int startHour)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].startHour = startHour;
        }

        // 更新选中的goalItem的startMinute属性
        public void startMinuteChange(int startMinute)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].startMinute = startMinute;
        }

        // 更新选中的goalItem的endYear属性
        public void endYearChange(int endYear)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].endYear = endYear;
        }

        // 更新选中的goalItem的endMonth属性
        public void endMonthChange(int endMonth)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].endMonth = endMonth;
        }

        // 更新选中的goalItem的endDay属性
        public void endDayChange(int endDay)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].endDay = endDay;
        }

        // 更新选中的goalItem的endHour属性
        public void endHourChange(int endHour)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].endHour = endHour;
        }

        // 更新选中的goalItem的endMinute属性
        public void endMinuteChange(int endMinute)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].endMinute = endMinute;
        }

        // 更新选中的goalItem的usedYear属性
        public void usedYearChange(int usedYear)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].usedYear = usedYear;
        }

        // 更新选中的goalItem的usedMonth属性
        public void usedMonthChange(int usedMonth)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].usedMonth = usedMonth;
        }

        // 更新选中的goalItem的usedDay属性
        public void usedDayChange(int usedDay)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].usedDay = usedDay;
        }

        // 更新选中的goalItem的usedHour属性
        public void usedHourChange(int usedHour)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].usedHour = usedHour;
        }

        // 更新选中的goalItem的usedMinute属性
        public void usedMinuteChange(int usedMinute)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].usedMinute = usedMinute;
        }

        // 更新选中的goalItem的usedSecond属性
        public void usedSecondChange(int usedSecond)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].usedSecond = usedSecond;
        }

        // 更新选中的goalItem的note属性
        public void noteChange(string note)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].note = note;
        }

        // 更新选中的goalItem的isFinished属性
        public void isFinishedChange(int isFinished)
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].isFinished = isFinished;
        }

        // 更新选中的goalItem的startTimeString和endTimeString
        public void timeStringChange()
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].TimeToString();
            this._allItems[index].changeString();
        }

        // 当修改涉及到string改变时需要调用这个函数更新一下string
        public void changeString()
        {
            var index = this._allItems.IndexOf(selectedItem);
            this._allItems[index].DayGoalString = this._allItems[index].startHour.ToString() + "时" + this._allItems[index].startMinute.ToString() + "分 - " + this._allItems[index].endHour.ToString() + "时" + this._allItems[index].endMinute.ToString() + "分";
            if (this._allItems[index].type == 0)
            {
                // 日目标
                TimeSpan startTime = new TimeSpan(this._allItems[index].startHour, this._allItems[index].startMinute, 0);
                TimeSpan endTime = new TimeSpan(this._allItems[index].endHour, this._allItems[index].endMinute, 0);
                TimeSpan usedTime = new TimeSpan(this._allItems[index].usedHour, this._allItems[index].usedMinute, this._allItems[index].usedSecond);
                TimeSpan totalTime = endTime - startTime;
                TimeSpan needTime = totalTime - usedTime;
                this._allItems[index].totalGoalString = totalTime.Hours.ToString() + "小时" + totalTime.Hours.ToString() + "分钟";
                this._allItems[index].usedGoalString = usedTime.Hours.ToString() + "小时" + usedTime.Hours.ToString() + "分钟";
                this._allItems[index].needGoalString = needTime.Hours.ToString() + "小时" + needTime.Hours.ToString() + "分钟";
            }
            if (this._allItems[index].type == 1 || this._allItems[index].type == 2)
            {
                // 月目标以及年目标
                DateTime startTime = new DateTime(this._allItems[index].startYear, this._allItems[index].startMonth, this._allItems[index].startDay, this._allItems[index].startHour, this._allItems[index].startMinute, 0);
                DateTime endTime = new DateTime(this._allItems[index].endYear, this._allItems[index].endMonth, this._allItems[index].endDay, this._allItems[index].endHour, this._allItems[index].endMinute, 0);
                TimeSpan usedTime = new TimeSpan(this._allItems[index].usedHour, this._allItems[index].usedMinute, this._allItems[index].usedSecond);
                TimeSpan totalTime = endTime - startTime;
                TimeSpan needTime = totalTime - usedTime;
                this._allItems[index].totalGoalString = totalTime.Hours.ToString() + "天" + totalTime.Hours.ToString() + "小时";
                this._allItems[index].usedGoalString = usedTime.Hours.ToString() + "天" + usedTime.Hours.ToString() + "小时";
                this._allItems[index].needGoalString = needTime.Hours.ToString() + "天" + needTime.Hours.ToString() + "小时";
            }
        }

        /*----------------------查询-------------------------*/
        // 查询：按照year,month,day来查询日目标，返回日目标item集合
        public ObservableCollection<Models.MyGoalItem> findDayGoalCollection(int year, int month, int day)
        {
            ObservableCollection<Models.MyGoalItem> DayGoalCollection = new ObservableCollection<Models.MyGoalItem>();
            for (int i = 0; i < this._allItems.Count; i++)
            {
                if (this._allItems[i].type == 0 && this._allItems[i].startYear == year &&
                    this._allItems[i].startMonth == month && this._allItems[i].startDay == day)
                {
                    DayGoalCollection.Add(this._allItems[i]);
                }
            }
            return DayGoalCollection;
        }
        // 查询：按照year,month来查询月目标，返回月目标item集合
        public ObservableCollection<Models.MyGoalItem> findMonthGoalCollection(int year, int month, int isFinished)
        {
            ObservableCollection<Models.MyGoalItem> MonthGoalCollection = new ObservableCollection<Models.MyGoalItem>();
            for (int i = 0; i < this._allItems.Count; i++)
            {
                if (this._allItems[i].type == 1 && this._allItems[i].startYear == year &&
                    this._allItems[i].startMonth == month && this._allItems[i].isFinished == isFinished)
                {
                    MonthGoalCollection.Add(this._allItems[i]);
                }
            }
            return MonthGoalCollection;
        }

        // 查询：按照year来查询年目标，返回年目标item集合
        public ObservableCollection<Models.MyGoalItem> findYearGoalCollection(int year, int month, int isFinished)
        {
            ObservableCollection<Models.MyGoalItem> YearGoalCollection = new ObservableCollection<Models.MyGoalItem>();
            for (int i = 0; i < this._allItems.Count; i++)
            {
                if (this._allItems[i].type == 2 && this._allItems[i].startYear == year
                     && this._allItems[i].isFinished == isFinished)
                {
                    YearGoalCollection.Add(this._allItems[i]);
                }
            }
            return YearGoalCollection;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
