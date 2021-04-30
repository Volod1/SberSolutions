using SberSolutions.Api.Models;

namespace SberSolutions.Api.Services
{
    public interface ILunchService
    {
        decimal GetPaid(LunchRequest lunchRequest);
    }
}
