using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoRMP.Services;

public interface IDisplayService<T>
{
    void Display(T indentifier);
}
