using Refit;
using System.Threading;
using System.Threading.Tasks;


namespace FestivalDataApi.BusinessLogic.Clients
{
    public interface ICodingTestServiceClient
    {
        [Get("/festivals")]
        Task<string> GetMusicFestivalsAsync(CancellationToken cancellationToken);
    }
}
