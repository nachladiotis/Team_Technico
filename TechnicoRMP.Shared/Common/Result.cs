using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoRMP.Shared.Common;

public class Result
{
    public int Status { get; set; }
    public string? Message { get; set; }
}
public class Result<T>
{
    public int Status { get; set; }
    public string? Message { get; set; }
    public T? Value { get; set; }
}
