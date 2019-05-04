using Converter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converter.Interfaces
{
    public interface IMapPeriod
    {
        Period GetPeriod(string value);
    }
}
