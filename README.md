# Music Festival Data Api
This is developed as part of the Energy Australia coding test.

## Prerequisites

1. .NET Core SDK 3.1 (https://aka.ms/dotnet-download)
2. Energy Australia Coding Test API is up and running. (https://eacp.energyaustralia.com.au/codingtest/api-docs/#/festivals/APIFestivalsGet).

# Running locally

1. Open the solution file using visual studio and hit F5 or ctrl + F5.


## Assumptions

- Since there's no specific requirement around the design or technology to be used I assumed that it's appropriate to use Backend-For-Frontend (BFF) service to consume the Coding Test API and return the data in the expected format.
- Coding test service will never return a large data set.

## Notes

- Dependancy injection is done in the 'FestivalDataApi\Startup.cs' file.
- Added middlewear for exception handling and error logging with Serilog sinks to file. Error logging is configured for text file logging for simplicity.

