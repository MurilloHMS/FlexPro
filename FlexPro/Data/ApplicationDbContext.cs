using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlexPro.Models;

namespace FlexPro.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{

public DbSet<FlexPro.Models.Veiculo> Veiculo { get; set; } = default!;
}