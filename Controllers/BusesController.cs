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
        private readonly IBusRepository repository;

        public BusesController(IBusRepository repository)
        {
            this.repository = repository;
        }

        [AcceptVerbs("GET")]
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
