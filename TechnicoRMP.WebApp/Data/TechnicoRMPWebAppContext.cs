using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Models;

namespace TechnicoRMP.WebApp.Data
{
    public class TechnicoRMPWebAppContext : DbContext
    {
        public TechnicoRMPWebAppContext (DbContextOptions<TechnicoRMPWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<TechnicoRMP.Models.PropertyRepair> PropertyRepair { get; set; } = default!;
    }
}
