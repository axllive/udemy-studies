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
            .Map(destino => destino.bio , origem => origem.Bio)
            .Map(destino => destino.gender , origem => origem.Gender)
            .Map(destino => destino.photos , origem => origem.Photos.Adapt<List<PhotoDTO>>());

            TypeAdapterConfig<Photo, PhotoDTO>
            .NewConfig()
            .TwoWays() //DISPONÍVEL APENAS PARA PROPRIEDADE<>PROPRIEDADE, Ñ FUNCIONA COM OBJ RELACIONADO
            .Map(destino => destino.id, origem => origem.Id)
            .Map(destino => destino.url, origem => origem.Url)
            .Map(destino => destino.ismain, origem => origem.IsMain);

        }

    }
}