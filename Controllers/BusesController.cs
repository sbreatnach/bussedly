using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using bussedly.Models;

namespace bussedly.Controllers
{
    public class BusesController : ApiController
    {
        // TODO: use DI to inject this repository when needed
        static readonly IBusRepository repository = new BusEireannRepository();

        public IEnumerable<Bus> GetAllBuses()
        {
            return repository.GetAllBuses();
        }

        public IEnumerable<Bus> GetBusesByArea(double left, double right,
                                               double top, double bottom)
        {
            var area = new Area(left, right, top, bottom);
            try
            {
                return repository.GetAllBusesByArea(area);
            }
            catch (BussedException be)
            {
                throw new HttpResponseException(be.StatusCode);
            }
        }

    }
}
