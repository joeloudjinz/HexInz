using HexInz.Application.Contracts.Ports;
using Microsoft.EntityFrameworkCore;

namespace HexInz.Infrastructure.EF.MySQL;

public class MySqlDataContext(DbContextOptions options) : DbContext(options), IDataContext
{
}