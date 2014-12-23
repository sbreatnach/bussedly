using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using bussedly.Models;

namespace bussedly.Controllers
{
    public class StopsController : ApiController
    {
        // TODO: use DI to inject this repository when needed
        static readonly IStopRepository repository = new BusEireannRepository();

        public IEnumerable<Stop> GetAllStops()
        {
            return repository.GetAllStops();
        }

        public IEnumerable<Prediction> GetStopPredictions(long id)
        {
            IEnumerable<Prediction> predictions = null;
            try
            {
                predictions = repository.GetStopPredictions(id);
            }
            catch(BussedException be)
            {
                throw new HttpResponseException(be.StatusCode);
            }
            if (predictions == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return predictions;
        }

        public IEnumerable<Stop> GetStopsByArea(double left, double right,
                                                double top, double bottom)
        {
            var area = new Area(left, right, top, bottom);
            try
            {
                return repository.GetAllStopsByArea(area);
            }
            catch(BussedException be)
            {
                throw new HttpResponseException(be.StatusCode);
            }
        }
    }
}
