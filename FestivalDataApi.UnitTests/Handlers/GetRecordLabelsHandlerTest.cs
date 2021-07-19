using FestivalDataApi.BusinessLogic.Clients;
using FestivalDataApi.BusinessLogic.Handlers;
using FestivalDataApi.DataModels;
using FestivalDataApi.UnitTests.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FestivalDataApi.UnitTests
{
    [Trait("Category", "Unit_Tests")]
    public class GetRecordLabelsHandlerTest
    {
        private readonly ILogger<GetRecordLabelsHandler> _logger;
        private readonly CancellationToken _cancellationToken;

        public GetRecordLabelsHandlerTest()
        {
            _logger = new Mock<ILogger<GetRecordLabelsHandler>>().Object;
            _cancellationToken = new CancellationTokenSource().Token;
        }

        [Theory]
        [MemberData(nameof(GetRecordLabelsHandlerTestData.NoContentResponse), MemberType = typeof(GetRecordLabelsHandlerTestData))]
        public async Task When_No_Data_Is_Returned_Then_An_NoContent_Exception_Should_Be_Thrown(RequestAndExpectedResult requestAndExpectedResult)
        {
            // Given
            var request = new RecordLabelsRequest { };
            var mocICodingTestServiceClient = new Mock<ICodingTestServiceClient>();

            mocICodingTestServiceClient.Setup(x => x.GetMusicFestivalsAsync(_cancellationToken)).ReturnsAsync(requestAndExpectedResult.ReturnedValue);

            var getRecordLabelsHandler = new GetRecordLabelsHandler(mocICodingTestServiceClient.Object, _logger);

            // When
            RecordLabelsResponse actualResult = null;
            Func<Task<RecordLabelsResponse>> getRecordLabelsAction = async () => actualResult = await getRecordLabelsHandler.Handle(requestAndExpectedResult.Request, _cancellationToken);

            // Then
            var exception = await getRecordLabelsAction.Should().ThrowAsync<HttpException>();
            actualResult.Should().BeNull();
            exception.Should().NotBeNull();
            exception.And.Code.Should().Be(requestAndExpectedResult.Code);
            exception.And.Description.Should().Be(requestAndExpectedResult.Description);
            exception.And.HttpStatusCode.Should().Be(requestAndExpectedResult.ResponseStatusCode);
        }

        [Theory]
        [MemberData(nameof(GetRecordLabelsHandlerTestData.ApiExceptionResponse), MemberType = typeof(GetRecordLabelsHandlerTestData))]
        public async Task When_CodingTestService_Returned_An_ApiException_Then_An_HttpException_Should_Be_Thrown(RequestAndExpectedResult requestAndExpectedResult)
        {
            // Given
            var request = new RecordLabelsRequest { };
            var mocICodingTestServiceClient = new Mock<ICodingTestServiceClient>();

            mocICodingTestServiceClient.Setup(x => x.GetMusicFestivalsAsync(_cancellationToken))
                .Throws(ApiException.Create(new HttpRequestMessage(), new HttpMethod("GET"), requestAndExpectedResult.Response).Result);

            var getRecordLabelsHandler = new GetRecordLabelsHandler(mocICodingTestServiceClient.Object, _logger);

            // When
            RecordLabelsResponse actualResult = null;
            Func<Task<RecordLabelsResponse>> getRecordLabelsAction = async () => actualResult = await getRecordLabelsHandler.Handle(requestAndExpectedResult.Request, _cancellationToken);

            // Then
            var exception = await getRecordLabelsAction.Should().ThrowAsync<HttpException>();
            actualResult.Should().BeNull();
            exception.Should().NotBeNull();
            exception.And.Code.Should().Be(requestAndExpectedResult.Code);
            exception.And.Description.Should().Be(requestAndExpectedResult.Description);
            exception.And.HttpStatusCode.Should().Be(requestAndExpectedResult.ResponseStatusCode);
        }

        [Theory]
        [MemberData(nameof(GetRecordLabelsHandlerTestData.OkResponse), MemberType = typeof(GetRecordLabelsHandlerTestData))]
        public async Task When_Data_Is_Returned_Then_They_Should_Be_In_The_Specific_Grouping_And_Order(RequestAndOkResult requestAndOkResult)
        {
            // Given
            var request = new RecordLabelsRequest { };
            var mocICodingTestServiceClient = new Mock<ICodingTestServiceClient>();

            mocICodingTestServiceClient.Setup(x => x.GetMusicFestivalsAsync(_cancellationToken)).ReturnsAsync(await requestAndOkResult.GetSerializedClientResponse());

            var getRecordLabelsHandler = new GetRecordLabelsHandler(mocICodingTestServiceClient.Object, _logger);

            // When
            var response = await getRecordLabelsHandler.Handle(request, _cancellationToken);

            // Then
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(requestAndOkResult.FinalResponse);
        }
    }
}
