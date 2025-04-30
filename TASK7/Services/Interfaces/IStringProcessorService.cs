using System.Threading.Tasks;
using StringProcessor.API.Models.Requests;
using StringProcessor.API.Models.Responses;

namespace StringProcessor.API.Services.Interfaces
{
    public interface IStringProcessorService
    {
        Task<ProcessStringResponse> ProcessString(ProcessStringRequest request);
    }
}