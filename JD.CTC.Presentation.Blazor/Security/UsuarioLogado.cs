using JD.CTC.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JD.CTC.Presentation.Blazor.Security
{
    public class UsuarioLogado : IUsuarioLogado
    {
        private readonly AuthenticationStateProvider _stateProvider;
        private readonly IConfiguration _configuration;

        public UsuarioLogado(
            AuthenticationStateProvider stateProvider, 
            IConfiguration configuration)
        {
            _stateProvider = stateProvider;
            _configuration = configuration;
        }

        private bool TryParseValueFromString<T>(string value, out T result)
        {
            var type = typeof(T);
            if (typeof(T) == typeof(string))
            {
                result = (T)(object)value;
                return true;
            }
            else if (value == null && (Nullable.GetUnderlyingType(type) != null))
            {
                result = (T)(object)default(T);
                return true;
            }
            else if (value == "" && typeof(DateTime) != typeof(T) && typeof(DateTimeOffset) != typeof(T))
            {
                result = (T)(object)default(T);
                return true;
            }
            else if (typeof(T).IsEnum)
            {
                // There's no non-generic Enum.TryParse (https://github.com/dotnet/corefx/issues/692)
                try
                {
                    result = (T)Enum.Parse(typeof(T), value);
                    return true;
                }
                catch (ArgumentException)
                {
                    result = default;
                    return false;
                }
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            {
                result = (T)(object)Convert.ToInt32(value);
                return true;
            }
            else if (typeof(T) == typeof(long) || typeof(T) == typeof(long?))
            {
                result = (T)(object)Convert.ToInt64(value);
                return true;
            }
            else if (typeof(T) == typeof(double) || typeof(T) == typeof(double?))
            {
                result = (T)(object)double.Parse(value, CultureInfo.InvariantCulture);
                return true;
            }
            else if (typeof(T) == typeof(decimal) || typeof(T) == typeof(decimal?))
            {
                result = (T)(object)decimal.Parse(value, CultureInfo.InvariantCulture);
                return true;
            }
            else if (typeof(T) == typeof(Guid) || typeof(T) == typeof(Guid?))
            {
                try
                {
                    result = (T)(object)Guid.Parse(value);
                }
                catch
                {
                    throw new InvalidOperationException($"Could not parse input. Invalid Guid format.");
                }
                return true;
            }
            
            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(T)}'.");
        }

        private async Task<T> GetValueIfExists<T>(string claimType, T valueDefault)
        {
            if (await IsAuthenticated())
            {
                var userState = await _stateProvider.GetAuthenticationStateAsync();

                if (userState.User.HasClaim(x => x.Type.Equals(claimType)))
                {
                    var value = userState.User.FindFirst(claimType).Value;

                    if (TryParseValueFromString<T>(value, out T result))
                    {
                        return result;
                    }

                    return valueDefault;
                }
            }

            return valueDefault;
        }

        public async Task<int> CDUser()
        {
            return await GetValueIfExists<int>("CDUser", 0);
        }

        public async Task<int> CDPerfil()
        {
            return await GetValueIfExists<int>("CDPerfil", 0);
        }

        public async Task<long> CNPJ()
        {
            return await GetValueIfExists<long>("CNPJ", 0);
        }

        public async Task<bool> IsAdmin()
        {
            return (await GetValueIfExists<string>(ClaimTypes.Role, "")).Equals("Adm");
        }

        public async Task<bool> IsAuthenticated()
        {
            var userState = await _stateProvider.GetAuthenticationStateAsync();

            return userState.User.Identity.IsAuthenticated;
        }

        public async Task<bool> IsMaster()
        {
            return (await GetValueIfExists<string>(ClaimTypes.Role, "")).Equals("MSTR");
        }

        public async Task<bool> IsOperational()
        {
            return (await GetValueIfExists<string>(ClaimTypes.Role, "")).Equals("Ope");
        }

        public async Task<int> ISPB()
        {
            return await GetValueIfExists<int>("ISPB", 0);
        }

        public async Task<string> Role()
        {
            return await GetValueIfExists<string>(ClaimTypes.Role, "");
        }

        public async Task<string> Instituicao()
        {
            return await GetValueIfExists<string>("Instituicao", "-");
        }

        public async Task<bool> IsAuthorizedAsync(params Perfil[] perfil)
        {
            if ((perfil.Contains(Perfil.Admin) && await IsAdmin()) ||
                (perfil.Contains(Perfil.Master) && await IsMaster()) ||
                (perfil.Contains(Perfil.Opr) && await IsOperational()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAuthorized(params Perfil[] perfil)
        {
            return Task.Run(async () => await IsAuthorizedAsync(perfil)).Result;
        }

        public bool ModoLogin()
        {
            bool result = false;

            if (bool.TryParse(_configuration["Configuracoes:ModoLogin"], out bool login))
            {
                result = login;
            }

            return result;
        }
    }
}
