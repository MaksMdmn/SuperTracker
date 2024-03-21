using AutoMapper;
using StorageService.Api.DataContracts;
using StorageService.Api.Domain.Visitors;

namespace StorageService.Api.MessageApi.Mappers;

public class MessagingProfile : Profile
{
    public MessagingProfile()
    {
        CreateMap<CreateVisitor, Visitor>()
            .ConstructUsing(src => new Visitor(src.IpAddress, src.Referrer, src.UserAgent));
    }
}