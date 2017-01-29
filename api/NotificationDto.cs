using MyFirstProject.Models;
using System;

namespace MyFirstProject.Controllers.Api
{
    public class NotificationDto
    {
        public NotifType Type { get;  set; }
        public DateTime DateTime { get;  set; }
        public DateTime? OriginalDateTime { get;  set; }
        public string OriginalVenue { get;  set; }        
        public ProjectDto Project { get;  set; }

    }
}