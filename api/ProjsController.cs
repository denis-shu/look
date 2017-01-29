using Microsoft.AspNet.Identity;
using MyFirstProject.Stabil;
using System.Web.Http;

namespace MyFirstProject.Controllers.API
{
    [Authorize]
    public class ProjsController : ApiController
    {
        private readonly IBitOfWork _bitOfWork;
        public ProjsController(IBitOfWork bitOfWork)
        {
            _bitOfWork = bitOfWork;
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userid = User.Identity.GetUserId();
            var pe = _bitOfWork.Projects.CancelProject(userid, id);
                //_context.Projects
                //.Include(p=>p.Visits.Select(v=>v.Visitor))
                //.Single(p => p.Id == id && p.MasterId == userid);
            if (pe.IsCanceled)
                return NotFound();
            
            pe.Cancel();
            _bitOfWork.Complete();           // _context.SaveChanges();
            return Ok();
        }
    }
}
