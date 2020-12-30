using MediatR;
using System.Collections.Generic;

namespace WesleyCore.Ordering.API.Application.Queries
{
    public class OrderQuery : IRequest<List<string>>
    {
        public string UserName { get; set; }
    }
}