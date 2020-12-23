using MediatR;
using System.Collections.Generic;

namespace GeekTime.Ordering.API.Application.Queries
{
    public class OrderQuery : IRequest<List<string>>
    {
        public string UserName { get; set; }
    }
}