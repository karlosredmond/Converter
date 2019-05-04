using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converter
{
    public interface IUtility
    {
        bool CheckMatchTimeInput(string matchTimeStr);

        string DisplayTime();

        void SetCorrectTime(string matchTimeStr);
    }

}
