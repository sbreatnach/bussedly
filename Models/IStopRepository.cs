using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussedly.Models
{
    interface IStopRepository
    {
        IEnumerable<Stop> GetAllStops();
        IEnumerable<Stop> GetAllStopsByArea(Area area);
        Stop GetStop(string id);
        IEnumerable<Prediction> GetStopPredictions(string id);
    }
}
