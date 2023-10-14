using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Mapster;

namespace API.DTOs
{
    public static class MappingConfigurations
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            //TypeAdapterConfig<MODEL_ORIGEM, VIEW_MODEL_DESTINO>
            //.NewConfig()
            //.TwoWays() //DISPONÍVEL APENAS PARA PROPRIEDADE<>PROPRIEDADE, Ñ FUNCIONA COM OBJ RELACIONADO
            //.Map(destino => destino, origem => origem)

            TypeAdapterConfig<AppUser, RegisterDTO>
            .NewConfig()
            .Map(destino => destino.username , origem => origem.UserName)
            .Map(destino => destino.bio , origem => origem.Bio);
        }

    }
}