using AutoMapper;
using BookingService.Application.DTOs.Requests;
using BookingService.Application.DTOs.Responses;
using BookingService.Domain.Entities;

namespace BookingService.Application.Mappings;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<SaveBookingRequest, Booking>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}