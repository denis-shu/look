using Microsoft.AspNet.Identity;
using MyFirstProject.Controllers.DTO;
using MyFirstProject.Models;
using MyFirstProject.Stabil;
using System.Linq;
using System.Web.Http;

namespace MyFirstProject.Controllers.Api
{
    public class FollowController : ApiController
    {
        private readonly IBitOfWork _bitOfWork;

        public FollowController(IBitOfWork bitOfWork)
        {
            _bitOfWork = bitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowDTO dto)
        {
            var userid = User.Identity.GetUserId();
            if (_bitOfWork.Followings.CheckFollow(userid, dto))
                return BadRequest("Follow already exsist");

            var fol = new Following
            {
                FollowerId = userid,
                FolloweeId = dto.FolloweeId

            };

            _bitOfWork.Followings.Add(fol);
            _bitOfWork.Complete();
            return Ok();

        }
        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = _bitOfWork.Followings.GetUnfollow(id, userId);
            if (following == null)
                return NotFound();

            _bitOfWork.Followings.Remove(following);
            _bitOfWork.Complete();
            
            return Ok(id);
        }
    }
}
