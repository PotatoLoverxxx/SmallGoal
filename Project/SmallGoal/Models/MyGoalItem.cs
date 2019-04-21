using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmallGoal.Models
{
    /// <summary>
    /// Model数据
    /// </summary>
    public class MyGoalItem : INotifyPropertyChanged
    {
        // 私有变量们
        // 目标名称
        private string _name;
        // 目标类型：0->日目标，1->月目标，2->年目标
        private int _type;
        // 目标开始时间（用户设定的），年月日时分秒均用int分开表示
        private int _startYear;
        private int _startMonth;
        private int _startDay;
        private int _startHour;
        private int _startMinute;
        // 目标结束时间（用户设定的），年月日时分秒均用int分开表示
        private int _endYear;
        private int _endMonth;
        private int _endDay;
        private int _endHour;
        private int _endMinute;
        // 目标备注
        private string _note;
        // 目标是否完成：0->未完成，1->已完成
        private int _isFinished;
        // 目标已经完成的时间
        private int _usedYear;
        private int _usedMonth;
        private int _usedDay;
        private int _usedHour;
        private int _usedMinute;
        private int _usedSecond;

        // 提供的日计划string串
        private string _DayGoalString;
        //  预计花费时间
        private string _totalGoalString;
        //  已花费时间
        private string _usedGoalString;
        //  还需花费时间
        private string _needGoalString;
        // 开始时间
        private string _startTimeString;
        // 结束时间
        private string _endTimeString;
        private bool _isCountingTime;

        private long _countingStart;
        private long _countedSeconds;

        // 供viewmodel调用的公有接口们
        // 目标名称
        public string name { get { return this._name; } set { this._name = value; this.NotifyPropertyChanged(); } }
        // 目标类型：0->日目标，1->月目标，2->年目标
        public int type { get { return this._type; } set { this._type = value; this.NotifyPropertyChanged(); } }
        // 目标开始时间（用户设定的），年月日时分秒均用int分开表示
        public int startYear { get { return this._startYear; } set { this._startYear = value; this.NotifyPropertyChanged(); } }
        public int startMonth { get { return this._startMonth; } set { this._startMonth = value; this.NotifyPropertyChanged(); } }
        public int startDay { get { return this._startDay; } set { this._startDay = value; this.NotifyPropertyChanged(); } }
        public int startHour { get { return this._startHour; } set { this._startHour = value; this.NotifyPropertyChanged(); } }
        public int startMinute { get { return this._startMinute; } set { this._startMinute = value; this.NotifyPropertyChanged(); } }
        // 目标结束时间（用户设定的），年月日时分秒均用int分开表示
        public int endYear { get { return this._endYear; } set { this._endYear = value; this.NotifyPropertyChanged(); } }
        public int endMonth { get { return this._endMonth; } set { this._endMonth = value; this.NotifyPropertyChanged(); } }
        public int endDay { get { return this._endDay; } set { this._endDay = value; this.NotifyPropertyChanged(); } }
        public int endHour { get { return this._endHour; } set { this._endHour = value; this.NotifyPropertyChanged(); } }
        public int endMinute { get { return this._endMinute; } set { this._endMinute = value; this.NotifyPropertyChanged(); } }
        // 目标备注
        public string note { get { return this._note; } set { this._note = value; this.NotifyPropertyChanged(); } }
        // 目标是否完成：0->未完成，1->已完成
        public int isFinished { get { return this._isFinished; } set { this._isFinished = value; this.NotifyPropertyChanged(); } }
        // 目标已用时间
        public int usedYear { get { return this._usedYear; } set { this._usedYear = value; this.NotifyPropertyChanged(); } }
        public int usedMonth { get { return this._usedMonth; } set { this._usedMonth = value; this.NotifyPropertyChanged(); } }
        public int usedDay { get { return this._usedDay; } set { this._usedDay = value; this.NotifyPropertyChanged(); } }
        public int usedHour { get { return this._usedHour; } set { this._usedHour = value; this.NotifyPropertyChanged(); } }
        public int usedMinute { get { return this._usedMinute; } set { this._usedMinute = value; this.NotifyPropertyChanged(); } }
        public int usedSecond { get { return this._usedSecond; } set { this._usedSecond = value; this.NotifyPropertyChanged(); } }

        public string DayGoalString { get { return this._DayGoalString; } set { this._DayGoalString = value; this.NotifyPropertyChanged(); } }
        //  预计花费时间
        public string totalGoalString { get { return this._totalGoalString; } set { this._totalGoalString = value; this.NotifyPropertyChanged(); } }

        public string usedGoalString { get { return this._usedGoalString; } set { this._usedGoalString = value; this.NotifyPropertyChanged(); } }

        public string needGoalString { get { return this._needGoalString; } set { this._needGoalString = value; this.NotifyPropertyChanged(); } }

        public string startTimeString { get { return this._startTimeString; } set { this._startTimeString = value;  this.NotifyPropertyChanged(); } }
        public string endTimeString { get { return this._endTimeString; } set { this._endTimeString = value;  this.NotifyPropertyChanged(); } }

        public bool isCountingTime { get { return this._isCountingTime; } set { this._isCountingTime = value; this.NotifyPropertyChanged(); } }

        public long countingStart { get { return this._countingStart; } set { this._countingStart = value; this.NotifyPropertyChanged(); } }
        public long countedSeconds { get { return this._countedSeconds; } set { this._countedSeconds = value; this.NotifyPropertyChanged(); } }

        public MyGoalItem(string name, int type, int startYear, int startMonth, int startDay, int startHour, int startMinute,
            int endYear, int endMonth, int endDay, int endHour, int endMinute, string note, int isFinished,
            int usedYear, int usedMonth, int usedDay, int usedHour, int usedMinute, int usedSecond)
        {
            _name = name;
            _type = type;
            _startYear = startYear;
            _startMonth = startMonth;
            _startDay = startDay;
            _startHour = startHour;
            _startMinute = startMinute;
            _endYear = endYear;
            _endMonth = endMonth;
            _endDay = endDay;
            _endHour = endHour;
            _endMinute = endMinute;
            _note = note;
            _isFinished = isFinished;
            _usedYear = usedYear;
            _usedMonth = usedMonth;
            _usedDay = usedDay;
            _usedHour = usedHour;
            _usedMinute = usedMinute;
            _usedSecond = usedSecond;
            _isCountingTime = false;
            _DayGoalString = startHour.ToString() + "时" + startMinute.ToString() + "分 - " + endHour.ToString() + "时" + endMinute.ToString() + "分";
            if (type == 0)
            {
                // 日目标
                _usedDay = 0;
                TimeSpan startTime = new TimeSpan(startHour, startMinute, 0);
                TimeSpan endTime = new TimeSpan(endHour, endMinute, 0);
                TimeSpan usedTime = new TimeSpan(usedHour, usedMinute, usedSecond);
                TimeSpan totalTime = endTime - startTime;
                TimeSpan needTime = totalTime - usedTime;
                _totalGoalString = totalTime.Hours.ToString() + "小时" + totalTime.Minutes.ToString() + "分钟";
                _usedGoalString = usedTime.Hours.ToString() + "小时" + usedTime.Minutes.ToString() + "分钟";
                _needGoalString = needTime.Hours.ToString() + "小时" + needTime.Minutes.ToString() + "分钟";
            } if (type == 1 || type == 2)
            {
                // 月目标以及年目标
                DateTime startTime = new DateTime(startYear, startMonth, startDay,startHour, startMinute, 0);
                DateTime endTime = new DateTime(endYear, endMonth, endDay, endHour, endMinute, 0);
                TimeSpan usedTime = new TimeSpan(usedDay, usedHour, usedMinute, usedSecond);
                TimeSpan totalTime = endTime - startTime;
                TimeSpan needTime = totalTime - usedTime;
                _totalGoalString = totalTime.Days.ToString() + "天" + totalTime.Hours.ToString() + "小时";
                _usedGoalString = usedTime.Days.ToString() + "天" + usedTime.Hours.ToString() + "小时";
                _needGoalString = needTime.Days.ToString() + "天" + needTime.Hours.ToString() + "小时";
            }
            TimeToString();
        }

        // 当修改涉及到string改变时需要调用这个函数更新一下string
        public void changeString()
        {
            this.DayGoalString = this.startHour.ToString() + "时" + this.startMinute.ToString() + "分 - " + this.endHour.ToString() + "时" + this.endMinute.ToString() + "分";
            if (type == 0)
            {
                // 日目标
                _usedDay = 0;
                TimeSpan startTime = new TimeSpan(startHour, startMinute, 0);
                TimeSpan endTime = new TimeSpan(endHour, endMinute, 0);
                TimeSpan usedTime = new TimeSpan(usedHour, usedMinute, usedSecond);
                TimeSpan totalTime = endTime - startTime;
                TimeSpan needTime = totalTime - usedTime;
                _totalGoalString = totalTime.Hours.ToString() + "小时" + totalTime.Minutes.ToString() + "分钟";
                _usedGoalString = usedTime.Hours.ToString() + "小时" + usedTime.Minutes.ToString() + "分钟";
                _needGoalString = needTime.Hours.ToString() + "小时" + needTime.Minutes.ToString() + "分钟";
            }
            if (type == 1 || type == 2)
            {
                // 月目标以及年目标
                DateTime startTime = new DateTime(startYear, startMonth, startDay, startHour, startMinute, 0);
                DateTime endTime = new DateTime(endYear, endMonth, endDay, endHour, endMinute, 0);
                TimeSpan usedTime = new TimeSpan(usedHour, usedMinute, usedSecond);
                TimeSpan totalTime = endTime - startTime;
                TimeSpan needTime = totalTime - usedTime;
                _totalGoalString = totalTime.Days.ToString() + "天" + totalTime.Hours.ToString() + "小时";
                _usedGoalString = usedTime.Days.ToString() + "天" + usedTime.Hours.ToString() + "小时";
                _needGoalString = needTime.Days.ToString() + "天" + needTime.Hours.ToString() + "小时";
            }
        }

        // 更新startTimeString和endTimeString
        public void TimeToString()
        {
                switch (type)
                {
                    case 0:
                        TimeSpan time = new TimeSpan(startHour, startMinute, 0);
                        startTimeString = startYear + "年" + startMonth + "月" +
                            startDay + "日 " + time.ToString().Remove(5);
                        time = new TimeSpan(endHour, endMinute, 0);
                        endTimeString = endYear + "年" + endMonth + "月" + endDay +
                            "日 " + time.ToString().Remove(5);
                        break;
                    case 1:
                        startTimeString = startYear + "年" + startMonth + "月" + startDay + "日";
                        endTimeString = endYear + "年" + endMonth + "月" + endDay + "日";
                        break;
                    case 2:
                        startTimeString = startYear + "年" + startMonth + "月";
                        endTimeString = endYear + "年" + endMonth + "月";
                        break;
                    default:
                        startTimeString = endTimeString = "";
                        break;
                }
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
