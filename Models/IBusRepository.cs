using System.Collections.Generic;

namespace bussedly.Models
{
    public interface IBusRepository
    {
        IEnumerable<Bus> GetAllBusesByArea(Area area);
        Bus GetBus(string id);
    }
}
