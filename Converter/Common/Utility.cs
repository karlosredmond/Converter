using Converter.Enums;
using Converter.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Converter.Common
{
    public class Utility : IUtility
    {
        private IMapPeriod _periodMapper;
        private const int FULL_TIME = 90;
        private const int HALF_TIME = 45;
        private const int PAD_LEFT = 2;
        private const char PAD_VALUE = '0';
        private const int MINUTE = 0;
        private const int SECOND = 1;
        private const int MILLISEC = 2;
        private const string REGEX = @"\[(PM|H1|HT|H2|FT)\][ ]{1}\d{1,2}[:][0-5][0-9][.]\d{3}$";
        protected Period _period;
        protected string[] _minSecMillisec;
        protected string[] _overTime;

        public Utility(IMapPeriod periodMapper)
        {
            _periodMapper = periodMapper;
        }

        public bool CheckMatchTimeInput(string matchTimeStr = "")
        {
            if(!Regex.IsMatch(matchTimeStr, REGEX))
            {
                return false;
            }

            SetPeriodAndMatchTime(matchTimeStr);

            return CheckTimeAndPeriod();
        }

        private bool CheckTimeAndPeriod()
        {
            if(_period.Equals(Period.PRE_MATCH) && _minSecMillisec.Any(x => (Int32.Parse(x.ToString())) > 0))
            {
                return false;
            }
            
            if(_period.Equals(Period.SECOND_HALF) && !(Int32.Parse(_minSecMillisec[MINUTE].ToString()) >= 45))
            {
                return false;
            }

            return true;
        }

        public string DisplayTime()
        {
            if (!_overTime[MINUTE].ToString().Equals(string.Empty))
            {
                _overTime[MINUTE] = $" +{_overTime[MINUTE].PadLeft(PAD_LEFT, PAD_VALUE)}:";
                _overTime[SECOND] = $"{_overTime[SECOND].PadLeft(PAD_LEFT, PAD_VALUE)}";
            } 
            
            return string.Format($"{_minSecMillisec[MINUTE].PadLeft(PAD_LEFT, PAD_VALUE)}:{_minSecMillisec[SECOND].PadLeft(PAD_LEFT, PAD_VALUE)}" +
                    $"{_overTime[MINUTE]}{_overTime[SECOND]} - {_period.ToString()}");
        }
        public void SetCorrectTime(string matchTimeStr)
        {
            CheckForOvertime();
            _minSecMillisec[SECOND] = RoundUp(_minSecMillisec[SECOND], _minSecMillisec[MILLISEC]);
        }

        private string RoundUp(string second, string milli)
        {
            return Int32.Parse(milli[0].ToString()) >= 5
                    ? (Int32.Parse(second) + 1).ToString()
                    : second;
        }

        private void CheckForOvertime()
        {
            _overTime = _overTime ?? new string[3];
            Array.Copy(_minSecMillisec, _overTime, 3);

            var minute = Int32.Parse(_minSecMillisec[MINUTE].ToString());

            if (_period.Equals(Period.FIRST_HALF) && minute >= HALF_TIME)
            {
                _overTime[MINUTE] = $"{(Int32.Parse(_overTime[MINUTE].ToString()) - HALF_TIME).ToString()}";
                _overTime[SECOND] = RoundUp(_overTime[SECOND], _overTime[MILLISEC]);
                SetHalfTimeFullTime(HALF_TIME);
            }
            else if (_period.Equals(Period.FULL_TIME) 
                || _period.Equals(Period.SECOND_HALF) && minute >= FULL_TIME)
            {
                _overTime[MINUTE] = (Int32.Parse(_overTime[MINUTE].ToString()) - FULL_TIME).ToString();
                _overTime[SECOND] = RoundUp(_overTime[SECOND], _overTime[MILLISEC]);
                SetHalfTimeFullTime(FULL_TIME);
            }
            else
            {
                _overTime[MINUTE] = _overTime[SECOND] = _overTime[MILLISEC] = "";
            }
        }

        private void SetPeriodAndMatchTime(string matchTimeStr)
        {
            _minSecMillisec = matchTimeStr.Substring(5).Replace('.', ':').Split(':');
            _period = _periodMapper.GetPeriod(matchTimeStr.Substring(0, 4));
        }

        private void SetHalfTimeFullTime(int halfTimeFullTime)
        {
            _minSecMillisec[MINUTE] = halfTimeFullTime.ToString();
            _minSecMillisec[SECOND] = "00";
            _minSecMillisec[MILLISEC] = "00";
            if (Int32.Parse(_overTime[SECOND].ToString()) == 60)
            {
                _overTime[MINUTE] = (Int32.Parse(_overTime[MINUTE].ToString()) + 1).ToString();
                _overTime[SECOND] = (Int32.Parse(_overTime[SECOND].ToString()) - 60).ToString();
            }
        }
    }
}
