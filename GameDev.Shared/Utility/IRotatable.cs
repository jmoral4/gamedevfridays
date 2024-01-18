using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDev.Shared.Utility
{
    public interface IRotatable
    {
        float elapsedMs { get; set; }
        float rotation { get; set; }
    }

}
