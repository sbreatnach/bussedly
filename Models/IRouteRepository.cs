using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussedly.Models
{
    public interface IRouteRepository
    {
        IEnumerable<Route> GetAllRoutes();
        IEnumerable<Route> GetAllRoutesByArea(Area area);
        Route GetRoute(string id);
    }
}
