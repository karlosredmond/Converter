using Converter.Enums;
using Converter.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace Converter.Common
{
    public class Utility : IUtility
    {
        private IMapPeriod _periodMapper;
        private const int FULL_TIME = 90;
        private const int HALF_TIME = 45;
        protected string _period;
        protected string[] _minSecMillisec;
        protected string[] _overTime;

        public Utility(IMapPeriod periodMapper)
        {
            _periodMapper = periodMapper;
        }

        public bool CheckMatchTimeInput(string matchTimeStr = "")
        {
            return Regex.IsMatch(matchTimeStr, @"\[(PM|H1|HT|H2|FT)\][ ]{1}\d{1,2}[:]\d{2}[.]\d{3}")
                ? true
                : false;
        }

        public string DisplayTime()
        {
            if (!_overTime[0].ToString().Equals(string.Empty))
            {
                _overTime[0] = $" +{_overTime[0].PadLeft(2, '0')}:";
                _overTime[1] = $"{_overTime[1].PadLeft(2, '0')}";
            } 
            
            return string.Format($"{_minSecMillisec[0].PadLeft(2,'0')}:{_minSecMillisec[1].PadLeft(2, '0')}" +
                    $"{_overTime[0]}{_overTime[1]} - {_periodMapper.GetPeriod(_period)}");
        }
        public void SetCorrectTime(string matchTimeStr)
        {
            SetPeriodAndMatchTime(matchTimeStr);
            CheckForOvertime();
            _minSecMillisec[1] = RoundUp(_minSecMillisec[1], _minSecMillisec[2]);
        }

        private string RoundUp(string second, string milli)
        {
            if(Int32.Parse(milli[0].ToString()) >= 5)
            {
                second = (Int32.Parse(second) + 1).ToString();
            }
            return second;
        }

        private void CheckForOvertime()
        {
            _overTime = _overTime ?? new string[3];
            Array.Copy(_minSecMillisec, _overTime, 3);

            var minute = Int32.Parse(_minSecMillisec[0].ToString());

            if (_periodMapper.GetPeriod(_period).Equals(Period.FIRST_HALF) && minute >= HALF_TIME)
            {
                _overTime[0] = $"{(Int32.Parse(_overTime[0].ToString()) - HALF_TIME).ToString()}";
                _overTime[1] = RoundUp(_overTime[1], _overTime[2]);
                SetHalfTimeFullTime(HALF_TIME);
            }
            else if (_periodMapper.GetPeriod(_period).Equals(Period.FULL_TIME) 
                || _periodMapper.GetPeriod(_period).Equals(Period.SECOND_HALF) && minute >= FULL_TIME)
            {
                _overTime[0] = (Int32.Parse(_overTime[0].ToString()) - FULL_TIME).ToString();
                _overTime[1] = RoundUp(_overTime[1], _overTime[2]);
                SetHalfTimeFullTime(FULL_TIME);
            }
            else
            {
                _overTime[0] = _overTime[1] = _overTime[2] = "";
            }
        }

        private void SetPeriodAndMatchTime(string matchTimeStr)
        {
            _minSecMillisec= matchTimeStr.Substring(5).Replace('.', ':').Split(':');
            _period = matchTimeStr.Substring(0, 4);
        }

        private void SetHalfTimeFullTime(int halfTimeFullTime)
        {
            _minSecMillisec[0] = halfTimeFullTime.ToString();
            _minSecMillisec[1] = "00";
            _minSecMillisec[2] = "00";
        }
    }
}
