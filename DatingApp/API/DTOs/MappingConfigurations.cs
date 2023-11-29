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
            .Map(destino => destino.username ,      origem => origem.UserName)
            .Map(destino => destino.bio ,           origem => origem.Bio)
            .Map(destino => destino.gender ,        origem => origem.Gender)
            .Map(destino => destino.age ,           origem => origem.GetAge())
            .Map(destino => destino.kwonas ,        origem => origem.KnownAs)
            .Map(destino => destino.created ,       origem => origem.Created.ToShortDateString())
            .Map(destino => destino.lastactive ,    origem => origem.LastActive.ToString("yyyy-MM-dd: HH:MM:ss"))
            .Map(destino => destino.intrests ,      origem => origem.Intrests)
            .Map(destino => destino.lookingfor ,    origem => origem.LookingFor)
            .Map(destino => destino.city ,          origem => origem.City)
            .Map(destino => destino.country ,       origem => origem.Country)
            .Map(destino => destino.photos ,        origem => origem.Photos.Adapt<List<PhotoDTO>>());

            TypeAdapterConfig<RegisterDTO, AppUser>
            .NewConfig()
            .Map(destino => destino.UserName,       origem => origem.username)
            .Map(destino => destino.Bio,            origem => origem.bio)
            .Map(destino => destino.Gender,         origem => origem.gender)
            .Map(destino => destino.KnownAs,        origem => origem.kwonas)
            .Map(destino => destino.Intrests,       origem => origem.intrests)
            .Map(destino => destino.LookingFor,     origem => origem.lookingfor)
            .Map(destino => destino.City,           origem => origem.city)
            .Map(destino => destino.Country,        origem => origem.country);


            TypeAdapterConfig<Photo, PhotoDTO>
            .NewConfig()
            .TwoWays() //DISPONÍVEL APENAS PARA PROPRIEDADE<>PROPRIEDADE, Ñ FUNCIONA COM OBJ RELACIONADO
            .Map(destino => destino.id, origem => origem.Id)
            .Map(destino => destino.url, origem => origem.Url)
            .Map(destino => destino.ismain, origem => origem.IsMain);

            
            TypeAdapterConfig<Message, MessageDTO>
            .NewConfig()
            .TwoWays() //DISPONÍVEL APENAS PARA PROPRIEDADE<>PROPRIEDADE, Ñ FUNCIONA COM OBJ RELACIONADO
            .Map(destino => destino.id, origem => origem.Id)
            .Map(destino => destino.senderid, origem => origem.SenderId)
            .Map(destino => destino.senderusername, origem => origem.SenderUsername)
            .Map(destino => destino.recipientid, origem => origem.RecipientId)
            .Map(destino => destino.recipientusername, origem => origem.RecipientUsername)
            .Map(destino => destino.content, origem => origem.Content)
            .Map(destino => destino.dateread, origem => origem.DateRead)
            .Map(destino => destino.messagesent, origem => origem.MessageSent)
            .Map(destino => destino.senderphotourl, origem => origem.Sender.Photos.FirstOrDefault(x => x.IsMain).Url )
            .Map(destino => destino.recipientphotourl, origem => origem.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url );

        }

    }
}