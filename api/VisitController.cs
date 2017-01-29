using Microsoft.AspNet.Identity;
using MyFirstProject.Controllers.DTO;
using MyFirstProject.Models;
using MyFirstProject.Stabil;
using System.Linq;
using System.Web.Http;

namespace MyFirstProject.Controllers.Api
{

    [Authorize]
    public class VisitController : ApiController
    {
        private readonly IBitOfWork _bitOfWork;
        public VisitController(IBitOfWork bitOfWork)
        {
            _bitOfWork = bitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Visit(VisitDataTransport dto)
        {
            var visitorId = User.Identity.GetUserId();
            var exist = _bitOfWork.Visits.VisitR(dto,  visitorId); 
                //_context.Visits.Any(v => v.VisitorId == visitorId && v.ProjectId == dto.ProjId);
            //
            if (exist)
                return BadRequest("Visitor allready exist.");

            var visit = new Visit
            {
                ProjectId = dto.ProjId,
                VisitorId = visitorId

            };
            _bitOfWork.Visits.Add(visit);
            _bitOfWork.Complete();
            //_context.Visits.Add(visit);
            //_context.SaveChanges();

            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult DeleteVisit(int id)
        {
            var userid = User.Identity.GetUserId();
            var visit = _bitOfWork.Visits.DeleteVisitR(id, userid);
            if (visit == null)
                return NotFound();
            _bitOfWork.Visits.Remove(visit);
            _bitOfWork.Complete();

//            _context.Visits.Remove(visit);
            //_context.SaveChanges();
            return Ok(id);
        }
    }
}
