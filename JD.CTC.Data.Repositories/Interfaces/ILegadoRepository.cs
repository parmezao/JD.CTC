using JD.CTC.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JD.CTC.Data.Repositories.Interfaces
{
    public interface ILegadoRepository
    {
        Task<IEnumerable<Legado>> GetLegadosAsync();
        Task<IEnumerable<Legado>> GetLegadosDapperAsync();
    }
}
