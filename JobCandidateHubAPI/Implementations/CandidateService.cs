using JobCandidateHubAPI.Dtos.Candidates;
using JobCandidateHubAPI.Interfaces;

namespace JobCandidateHubAPI.Implementations
{
    public class CandidateService(JobCandidateDbContext dbContext,ILogger<CandidateService> logger):ICandidateService
    {
        public async Task<CreateOrUpdateResult> CreateOrUpdate(CreateOrUpdateCandidateRequestInput requestInput)
        {
            //check if candidate already exists
            var candidate = await dbContext.Candidates.FindAsync(requestInput.Email);
            if (candidate == null)
            {
                candidate = new Entities.Candidate()
                {
                    Email = requestInput.Email,
                    FirstName = requestInput.FirstName,
                    LastName = requestInput.LastName,
                    PhoneNumber = requestInput.PhoneNumber, 
                    IntervalStateTime = requestInput.IntervalStateTime,
                    IntervalEndTime = requestInput.IntervalEndTime,
                    LinkedInProfileUrl = requestInput.LinkedInProfileUrl,
                    GitHubProfileUrl = requestInput.GitHubProfileUrl,
                    Comments = requestInput.Comments
                };
                dbContext.Candidates.Add(candidate);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Candidate created with email {email}", requestInput.Email);
                return new CreateOrUpdateResult()
                {
                    IsUpdate = false
                };
            }
            else
            {
                //update the candidate
                if (candidate.FirstName != requestInput.FirstName)
                {
                    candidate.FirstName = requestInput.FirstName;
                }
                if (candidate.LastName != requestInput.LastName)
                {
                    candidate.LastName = requestInput.LastName;
                }
                if (candidate.PhoneNumber != requestInput.PhoneNumber)
                {
                    candidate.PhoneNumber = requestInput.PhoneNumber;
                }
                if (candidate.IntervalStateTime != requestInput.IntervalStateTime)
                {
                    candidate.IntervalStateTime = requestInput.IntervalStateTime;
                }
                if (candidate.IntervalEndTime != requestInput.IntervalEndTime)
                {
                    candidate.IntervalEndTime = requestInput.IntervalEndTime;
                }
                if (candidate.LinkedInProfileUrl != requestInput.LinkedInProfileUrl)
                {
                    candidate.LinkedInProfileUrl = requestInput.LinkedInProfileUrl;
                }
                if (candidate.GitHubProfileUrl != requestInput.GitHubProfileUrl)
                {
                    candidate.GitHubProfileUrl = requestInput.GitHubProfileUrl;
                }
                if (candidate.Comments != requestInput.Comments)
                {
                    candidate.Comments = requestInput.Comments;
                }
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Candidate updated with email {email}", requestInput.Email);
                return new CreateOrUpdateResult()
                {
                    IsUpdate = true
                };
            }
        }
    }
}
