using Domain.Constants;

namespace Application.Common.Responses.Views
{
    public class ProjectIndexView
    {
        public List<ImageView> Slides { get; set; } = new List<ImageView>();
        public List<ProjectGroup> Apartment { get; set; } = new List<ProjectGroup>();
        public List<ProjectGroup> Ground { get; set; } = new List<ProjectGroup>();
        public List<ProjectGroup> ResortRealEstate { get; set; } = new List<ProjectGroup>();
        public List<ProjectGroup> Villa { get; set; } = new List<ProjectGroup>();
    }

    public class ProjectGroup
    {
        public string State { get; set; } = ProjectStateConstant.Implementing;
        public string StateName { get; set; } = string.Empty;
        public List<ProjectView> Projects { get; set; } = new List<ProjectView>();
    }
}
