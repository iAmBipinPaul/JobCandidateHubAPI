# JobCandidateHubAPI

This is my implementation of the job interview task. I've kept it simple and focused on the requirements while ensuring cross-platform compatibility.

## Technical Choices

I've used:
- .NET 9
- EF Core with SQLite (for easy testing on Linux/MacOS without Visual Studio)
- FluentValidation for model validation
- Fluent API for EF Core configurations
- Scalar for API documentation
- xUnit with Moq for testing

## Implementation Notes

- Created a single endpoint that handles both create and update operations for candidate info
- Used SQLite to make it easy to run without dependencies
- Kept all code in one project for simplicity
- Implemented validation using FluentValidation package
- Added Scalar for easy API testing

## How to Run

1. Clone the repo
2. Just run the project - SQLite DB will be created automatically
3. Open `/scalar` in your browser to test the API
Note : I've included a `JobCandidateHubAPI.http` file that can be used to test the API
## API Details

- `POST /candidates` - Creates or updates candidate information, returns 201 on create and 200 on update
- `GET /candidates` - Returns a list of all candidates (not required just created to get the result and make sure it saves)
## Testing

I've written unit tests using:
- xUnit
- Moq for mocking

Note: I focused on keeping the implementation straightforward while following good practices and making it easy to test on any platform.
