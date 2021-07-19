using FestivalDataApi.BusinessLogic.Clients;
using FestivalDataApi.DataModels;
using FestivalDataApi.DataModels.Dto;
using FestivalDataApi.DataModels.Models;
using FestivalDataApi.Utilities.Constants;
using FestivalDataApi.Utilities.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FestivalDataApi.BusinessLogic.Handlers
{
    /// <summary>
    /// This is the Medit handler class to handle requests coming from the controller GetRecordLabelsAsync() endpoint
    /// </summary>
    public class GetRecordLabelsHandler : IRequestHandler<RecordLabelsRequest, RecordLabelsResponse>
    {
        private readonly ICodingTestServiceClient _codingTestServiceClient;
        private readonly ILogger<GetRecordLabelsHandler> _logger;

        public GetRecordLabelsHandler(ICodingTestServiceClient codingTestServiceClient,
                                      ILogger<GetRecordLabelsHandler> logger)
        {
            _codingTestServiceClient = codingTestServiceClient;
            _logger = logger;
        }

        public async Task<RecordLabelsResponse> Handle(RecordLabelsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Start of GetRecordLabelsHandler");

                var recordLabels = new RecordLabelsResponse();
                var response = await _codingTestServiceClient.GetMusicFestivalsAsync(cancellationToken);

                if (string.IsNullOrWhiteSpace(response) || response == StringConstants.EmptyStringResponse)
                {
                    throw new HttpException(_logger, HttpStatusCode.NoContent, 0, Messages.NoContent);
                }

                var musicFestivals = await response.FromJson<MusicFestival>();

                if (musicFestivals != null && musicFestivals.Any())
                {
                    var result = GroupAndSortRecords(musicFestivals);

                    recordLabels.RecordLabels = result;
                }

                return recordLabels;
            }
            catch (ApiException ex)
            {
                throw new HttpException(_logger, ex.StatusCode, 0, Messages.ClientError, ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _logger.LogDebug("End of GetRecordLabelsHandler");
            }
        }

        /// <summary>
        /// This will flatten, group and order (transform) the response from CodingTestService to the required data format
        /// NOTE: A performance improvement could be made if we paginate the data coming from the CodingTestService
        /// </summary>
        /// <param name="musicFestivals"></param>
        /// <returns></returns>
        private static List<RecordLabelDto> GroupAndSortRecords(IEnumerable<MusicFestival> musicFestivals)
        {
            var records = musicFestivals.SelectMany(mf => mf.Bands, (mf, band) => new { mf, band })
                .Select(r => new
                {
                    FestivalName = r.mf.Name,
                    BandName = r.band.Name,
                    RecordLabel = r.band.RecordLabel
                });

            var result = records.GroupBy(r => new { r.RecordLabel })
                                .Select(r => new RecordLabelDto
                                {
                                    RecordLabel = r.Key.RecordLabel,
                                    Bands = r.GroupBy(b => new { b.BandName })
                                                .Select(b => new BandDto
                                                {
                                                    Name = b.Key.BandName,
                                                    Festivals = b.GroupBy(f => new { f.FestivalName })
                                                                    .Select(f => f.Key.FestivalName)
                                                                    .OrderBy(festivalName => festivalName)
                                                                    .ToList()
                                                })
                                                .OrderBy(band => band.Name)
                                                .ToList()
                                })
                                .OrderBy(record => record.RecordLabel)
                                .ToList();

            return result;
        }
    }
}
