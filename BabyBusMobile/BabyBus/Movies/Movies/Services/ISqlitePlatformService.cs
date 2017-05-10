using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Interop;

namespace CoolBeans.Services
{
    public interface ISqlitePlatformService
    {
        ISQLitePlatform GetLitePlatform();
    }
}
