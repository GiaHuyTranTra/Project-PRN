using System.Collections.Generic;

namespace SportCenterManament_PRN.Services.MemberListService
{
    public class MemberPageResult
    {
        public List<MemberListItem> Items { get; set; } = new List<MemberListItem>();
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
