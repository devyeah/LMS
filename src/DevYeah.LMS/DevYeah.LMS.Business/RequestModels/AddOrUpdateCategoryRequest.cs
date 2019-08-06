using System;

namespace DevYeah.LMS.Business.RequestModels
{
    public class AddOrUpdateCategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
