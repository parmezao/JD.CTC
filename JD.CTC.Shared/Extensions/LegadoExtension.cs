using System;
using System.Collections.Generic;
using System.Text;

namespace JD.CTC.Shared.Extensions
{
    public static class LegadoExtension
    {
        public static string ToSituacaoLegado(this string situacao)
        {
            return situacao switch
            {
                "A" => "Ativo",
                "I" => "Inativo",
                _ => string.Empty
            };
        }
    }
}
