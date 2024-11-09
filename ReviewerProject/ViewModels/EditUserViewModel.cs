namespace ReviewerProject.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Password { get; set; }
    }
}
