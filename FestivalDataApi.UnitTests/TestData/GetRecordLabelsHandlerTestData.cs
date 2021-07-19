using FestivalDataApi.DataModels;
using FestivalDataApi.DataModels.Dto;
using FestivalDataApi.DataModels.Models;
using FestivalDataApi.Utilities.Constants;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace FestivalDataApi.UnitTests.TestData
{
    public class GetRecordLabelsHandlerTestData
    {
        /// <summary>
        /// This is test data for the null or whitespace response type from the CodingTestService
        /// </summary>
        public static TheoryData<RequestAndExpectedResult> NoContentResponse =>
            new TheoryData<RequestAndExpectedResult>
            {
                // When an empty string is returned
                new RequestAndExpectedResult {
                    Request = new RecordLabelsRequest{ },
                    Code = 0,
                    Description = Messages.NoContent,
                    ResponseStatusCode = HttpStatusCode.NoContent,
                    ReturnedValue = StringConstants.EmptyStringResponse
                },
                // When whitespace character(s) returned
                new RequestAndExpectedResult {
                    Request = new RecordLabelsRequest{ },
                    Code = 0,
                    Description = Messages.NoContent,
                    ResponseStatusCode = HttpStatusCode.NoContent,
                    ReturnedValue = "\n  "
                },
                // When null returned
                new RequestAndExpectedResult {
                    Request = new RecordLabelsRequest{ },
                    Code = 0,
                    Description = Messages.NoContent,
                    ResponseStatusCode = HttpStatusCode.NoContent,
                    ReturnedValue = null
                }
            };

        /// <summary>
        /// This is test data for the ApiException response type from the CodingTestService
        /// </summary>
        public static TheoryData<RequestAndExpectedResult> ApiExceptionResponse =>
            new TheoryData<RequestAndExpectedResult>
            {
                // When throttling error/ TooManyRequests exception is thrown
                new RequestAndExpectedResult {
                    Request = new RecordLabelsRequest{ },
                    Code = 0,
                    Description = Messages.ClientError,
                    ResponseStatusCode = HttpStatusCode.TooManyRequests // http 429
                }
                // NOTE: The CodingTestService's Swagger file doesn't show that any other ApiErrors. 
                // In future if there are any they could go here for testing 
            };

        /// <summary>
        /// This is test data for the Ok response type from the CodingTestService
        /// </summary>
        public static TheoryData<RequestAndOkResult> OkResponse =>
            new TheoryData<RequestAndOkResult>
            {
                new RequestAndOkResult {
                    Request = new RecordLabelsRequest{ },
                    ClientResponse = new List<MusicFestival> {
                        new MusicFestival
                        {
                            Name = "",
                            Bands = new List<Band>
                            {
                                new Band
                                {
                                    Name = "Band Y",
                                    RecordLabel = "Record Label 1"
                                }
                            }
                        },
                        new MusicFestival
                        {
                            Name = "Omega Festival",
                            Bands = new List<Band>
                            {
                                new Band
                                {
                                    Name = "Band X",
                                    RecordLabel = "Record Label 1"
                                }
                            }
                        },
                        new MusicFestival
                        {
                            Name = "Alpha Festival",
                            Bands = new List<Band>
                            {
                                new Band
                                {
                                    Name = "Band A",
                                    RecordLabel = "Record Label 2"
                                }
                            }
                        },
                        new MusicFestival
                        {
                            Name = "Beta Festival",
                            Bands = new List<Band>
                            {
                                new Band
                                {
                                    Name = "Band A",
                                    RecordLabel = "Record Label 2"
                                }
                            }
                        }
                    },
                    FinalResponse = new RecordLabelsResponse
                    {
                        RecordLabels = new List<RecordLabelDto>
                        {
                            new RecordLabelDto
                            {
                                RecordLabel = "Record Label 1",
                                Bands = new List<BandDto>
                                {
                                    new BandDto
                                    {
                                        Name = "Band X",
                                        Festivals = new List<string>
                                        {
                                            "Omega Festival"
                                        }
                                    },
                                    new BandDto
                                    {
                                        Name = "Band Y",
                                        Festivals = new List<string>
                                        {
                                            ""
                                        }
                                    }
                                }
                            },
                            new RecordLabelDto
                            {
                                RecordLabel = "Record Label 2",
                                Bands = new List<BandDto>
                                {
                                    new BandDto
                                    {
                                        Name = "Band A",
                                        Festivals = new List<string>
                                        {
                                            "Alpha Festival",
                                            "Beta Festival"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
    }
}
