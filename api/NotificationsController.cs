using Microsoft.AspNet.Identity;
using MyFirstProject.Models;
using MyFirstProject.Stabil;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace MyFirstProject.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly IBitOfWork _bitOfWork;


        public NotificationsController(IBitOfWork bitOfWork)
        {
            _bitOfWork = bitOfWork;
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userid = User.Identity.GetUserId();
            var notifications = _bitOfWork.UserNotifs.GetNotifications(userid);
                //_context.UserNotifs
                //.Where(u => u.UserId == userid && !u.IsRead)
                //.Select(u => u.Notification)
                //.Include(n => n.Project.Master)
                //.ToList();

            //Mapper.Map<ApplicationUser, UserDto>();

            // Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, UserDto>());
            // Mapper.Initialize(cfg => cfg.CreateMap<Project, ProjectDto>());

            // Mapper.Initialize(cfg => cfg.CreateMap<Notification, NotificationDto>());

            //return notifications.Select(Mapper.Map<Notification,NotificationDto>);
            return notifications.Select(n => new NotificationDto()
            {
                DateTime = n.DateTime,
                Project = new ProjectDto
                {
                    Master = new UserDto
                    {
                        Id = n.Project.Master.Id,
                        Name = n.Project.Master.Name
                    },
                    DateTime = n.Project.DateTime,
                    Id = n.Project.Id,
                    IsCanceled = n.Project.IsCanceled,
                    Venue = n.Project.Venue
                },
                OriginalDateTime = n.OriginalDateTime,
                OriginalVenue = n.OriginalVenue,
                Type = n.Type

            });                       

        }
        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userid = User.Identity.GetUserId();
            var notifications = _bitOfWork.UserNotifs.GetNotificationsAsRead(userid);
                //_context.UserNotifs.Where(u => u.UserId == userid && !u.IsRead)
                //.ToList();

            notifications.ForEach(n => n.Read());
            _bitOfWork.Complete();
            return Ok();


        }
    }
}
