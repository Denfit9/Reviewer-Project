﻿using ReviewerProject.Models;

namespace ReviewerProject.ViewModels
{
    public class EditReviewViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Rating { get; set; }
        public List<int> Ratings { get; set; }
        public Guid? ReviewingTypeKey { get; set; }
        public List<ReviewingType> ReviewingTypes { get; set; }
    }
}
