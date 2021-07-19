# Music Festival Data Api
This is developed as part of the Energy Australia coding test

## Prerequisites

1. .NET Core SDK 3.1 (https://aka.ms/dotnet-download)
2. Energy Australia Coding Test API is up and running. (https://eacp.energyaustralia.com.au/codingtest/api-docs/#/festivals/APIFestivalsGet)

## Assumptions

- Since there's no specific requirement around the design or technology to be used I assumed that it's appropriate to use BFF service to consume the Coding Test API and return the data in the expected format.
- Coding test service will never return a large data set. 

## Notes

- Dependancy injection is done in the 'FestivalDataApi\Startup.cs'
- Error logging is configured for console logging for simplicity

