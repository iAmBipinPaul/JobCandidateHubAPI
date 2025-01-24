using JobCandidateHubAPI.Dtos.Candidates;

namespace JobCandidateHubAPI.Interfaces
{
    public interface ICandidateService
    {
        Task<CreateOrUpdateResult> CreateOrUpdate(CreateOrUpdateCandidateRequestInput requestInput);
    }
}
