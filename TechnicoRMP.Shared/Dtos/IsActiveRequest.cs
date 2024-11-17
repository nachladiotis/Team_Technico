﻿namespace TechnicoRMP.Shared.Dtos
{
    public class IsActiveRequest
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
