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
        private readonly IStopRepository _repository;

        public StopsController(IStopRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<Prediction> GetStopPredictions(string id)
        {
            IEnumerable<Prediction> predictions = null;
            try
            {
                predictions = _repository.GetStopPredictions(id);
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
                return _repository.GetAllStopsByArea(area);
            }
            catch(BussedException be)
            {
                throw new HttpResponseException(be.StatusCode);
            }
        }
    }
}
