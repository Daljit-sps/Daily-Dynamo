using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDynamo.Shared.Models.DTO;

public class PaginatedRequest<T>
{
    public int Page { get; set; } = 1;
    public int Offset { get; set; } = 10;
    public T? Model { get; set; }
}

public class PaginatedResponse<T> 
{
    public int TotalItems { get; set; } = 0;
    public T? Data { get; set; } 
}