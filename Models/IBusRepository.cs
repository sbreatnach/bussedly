using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussedly.Models
{
    public interface IBusRepository
    {
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBusesByArea(Area area);
        Bus GetBus(string id);
    }
}
