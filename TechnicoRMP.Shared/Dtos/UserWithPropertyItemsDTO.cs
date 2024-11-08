using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoRMP.Shared.Dtos
{
    public class UserWithProperyItemsDTO : CreateUserResponse
    {
        public List<CreatePropertyItemRequest>? PropertyItems { get; set; }
    }
}
