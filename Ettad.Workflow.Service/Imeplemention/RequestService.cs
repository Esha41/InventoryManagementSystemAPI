//using Ettad.Data.Enums;
//using Ettad.ResponseHandler.Models;
//using Ettad.Workflows.Service.DTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Ettad.Workflows.Service.Imeplemention
//{
//    public class RequestService : IRequestService
//    {
//        private readonly IEnumerable<IRequestHandler> _handlers;

//        public RequestService(IEnumerable<IRequestHandler> handlers)
//        {
//            _handlers = handlers;
//        }

//        public async Task<APIOperationResponse<List<RequestListDto>>> GetRequestsAsync(WorkflowType? type)
//        {
//            List<RequestListDto> result = new();

//            if (type.HasValue)
//            {
//                var handler = _handlers.First(h => h.Type == type.Value);
//                result = await handler.GetRequestsAsync();
//            }
//            else
//            {
//                foreach (var handler in _handlers)
//                    result.AddRange(await handler.GetRequestsAsync());
//            }

//            return APIOperationResponse<List<RequestListDto>>.Success(result);
//        }
//        public async Task<APIOperationResponse<RequestDetailsDto>> GetRequestDetailsAsync(long requestId, WorkflowType requestType)
//        {
//            var handler = _handlers.First(h => h.Type == requestType);
//            var data = await handler.GetRequestDetailsAsync(requestId);

//            if (data == null)
//                return APIOperationResponse<RequestDetailsDto>.NotFound("Request not found");

//            return APIOperationResponse<RequestDetailsDto>.Success(data);
//        }

//    }

//}
