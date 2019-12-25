using System;
using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles () {
            CreateMap<User, UserForListDto> ()
                .ForMember (dest => dest.photoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.ResolveUsing (d => d.DateOfBirth.CalculateAge ());
                });
            CreateMap<User, UserForDetailedDto> ()
                .ForMember (dest => dest.photoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.ResolveUsing (d => d.DateOfBirth.CalculateAge ());
                });
            CreateMap<Photo, PhotosForDetailedDto> ();
            CreateMap<UserForUpdateDto, User> ();
            CreateMap<Photo, PhotoForReturnDto> ();
            CreateMap<PhotosForCreationDto, Photo> ();
            CreateMap<USerForRegisterDto, User> ();
            CreateMap<MessageForCreationDto, Message> ().ReverseMap ();
            CreateMap<Message, MessageToreturnDto> ()
                .ForMember (dest => dest.SenderPhtotoUrl, opt => {
                    opt.MapFrom (src => src.Sender.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.RecipientPhtotoUrl, opt => {
                    opt.MapFrom (src => src.Recipient.Photos.FirstOrDefault (p => p.IsMain).Url);
                });
        }

    }
}