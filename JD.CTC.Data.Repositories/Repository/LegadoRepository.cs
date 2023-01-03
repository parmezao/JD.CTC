using Dapper;
using JD.CTC.Data.Repositories.DataContext;
using JD.CTC.Data.Repositories.Interfaces;
using JD.CTC.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JD.CTC.Data.Repositories.Repository
{
    public class LegadoRepository : ILegadoRepository
    {
        private readonly CTCContext _context;
        private readonly IDbConnection _connection;

        public LegadoRepository(IDbConnection connection, IConfiguration configuration)
        {
            //_context = context;
            _context = new CTCContext(@"Data Source=JDTST;Initial Catalog=DIANA_JDCTC_JD_NovaMod;User ID=jdteste;Password=jdteste;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            _connection = connection;
        }

        public async Task<IEnumerable<Legado>> GetLegadosAsync()
        {
            var legado = await _context.Legado.ToListAsync();
            return legado;
        }

        public async Task<IEnumerable<Legado>> GetLegadosDapperAsync()
        {
            var sql = @"SELECT CDLEGADO CdLegado,
                               NMLEGADO NomeLegado,
	                           STLEGADO SitLegado
                          FROM TBJDCTC_LEGADO";

            var legado = await _connection.QueryAsync<Legado>(sql);
            return legado;
        }
    }
}
