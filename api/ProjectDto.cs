using System;

namespace MyFirstProject.Controllers.Api
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public bool IsCanceled { get; set; }
        public UserDto Master { get; set; }
        public DateTime DateTime { get; set; }
        public string Venue { get; set; }
        public GenreDto Genre { get; set; }



    }
}