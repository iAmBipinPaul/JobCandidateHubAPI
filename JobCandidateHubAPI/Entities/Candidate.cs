namespace JobCandidateHubAPI.Entities
{
    public class Candidate
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public TimeOnly? IntervalStateTime { get; set; }
        public TimeOnly? IntervalEndTime { get; set; }
        public string? LinkedInProfileUrl { get; set; }
        public string? GitHubProfileUrl { get; set; }
        public string? Comments { get; set; }
    }
}
