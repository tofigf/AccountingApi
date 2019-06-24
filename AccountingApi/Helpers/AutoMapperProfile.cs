using AccountingApi.Dtos;
using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.User;
using AccountingApi.Models;
using AutoMapper;
using EOfficeAPI.Dtos.Nomenklatura.Kontragent;
using EOfficeAPI.Dtos.Nomenklatura.Product;
using EOfficeAPI.Dtos.Nomenklatura.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            //App Url
            var appBaseUrl = MyHttpContext.AppBaseUrl;

            //Register
            #region User Company
            //Istifadeci
            //Qeydiyyat
            CreateMap<UserForRegisterDto, User>().ReverseMap();
            CreateMap<User, UserForReturnDto>().ReverseMap();
            //Sirket yaratmaq
            CreateMap<CompanyPostDto, Company>().ReverseMap();
            CreateMap<CompanyPutDto, Company>().ReverseMap();
            CreateMap<Company, CompanyGetDto>()
                .ForMember(dto => dto.PhotoUrl, opt => opt
                 .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));
            CreateMap<Company, CompanyEditDto>()
                   .ForMember(dto => dto.PhotoUrl, opt => opt
                .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));
            CreateMap<Company, CompanyPutProposalDto>();
            //    .ForMember(dto => dto.PhotoUrl, opt => opt
            //.MapFrom(src => $"{appBaseUrl}/Uploads/" + src.PhotoUrl));
            CreateMap<Company, CompanyAfterPutDto>()
                   .ForMember(dto => dto.PhotoUrl, opt => opt
                .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));
            //Get User Edit
            CreateMap<User, UserGetEditDto>().ReverseMap();
            //Put User
            CreateMap<UserPutDto, User>().ReverseMap();
            #endregion

            //Employee
            #region Worker
            //Post
            CreateMap<WorkerPostDto, Worker>().ReverseMap();
            CreateMap<WorkerPostDto, Worker_Detail>().ReverseMap();
            //Get
            CreateMap<Worker, WorkerGetDto>()
                .ForMember(dto => dto.PhotoUrl, opt => opt
                 .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));
            CreateMap<Worker, WorkerEditDto>()
                 .ForMember(dto => dto.PhotoUrl, opt => opt
               .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));

            CreateMap<Worker_Detail, WorkerEditDto>();
            //  .ForMember(dto => dto.Birthday, opt => opt
            //.MapFrom(src => src.Birthday.Value.ToString("dd.MM.yyy")));
            //Put
            CreateMap<WorkerPutDto, Worker>().ReverseMap();
            CreateMap<WorkerPutDto, Worker_Detail>().ReverseMap();
            #endregion

            //Product
            #region Product
            //Post
            CreateMap<ProductPostDto, Product>().ReverseMap();
            CreateMap<ProductPostDto, Stock>().ReverseMap();
            //Get
            CreateMap<Product, ProductGetDto>().ReverseMap();
            //     .ForMember(dto => dto.StockGet, opt => opt
            //.MapFrom(src => src.Stocks));
            CreateMap<Stock, ProductGetDto>().ReverseMap();
            CreateMap<Product, ProductGetEditDto>()
              .ForMember(dto => dto.PhotoUrl, opt => opt
               .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));
            CreateMap<Stock, ProductGetEditDto>().ReverseMap();
            //Put
            CreateMap<ProductPutDto, Product>().ReverseMap();
            CreateMap<ProductPutDto, Stock>().ReverseMap();
            #endregion

            //Contragent
            #region Contragent
            //Get
            CreateMap<Contragent, ContragentGetDto>()
                     .ForMember(dto => dto.PhotoUrl, opt => opt
                .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));
            CreateMap<Contragent, ContragentGetEditDto>()
                .ForMember(dto => dto.PhotoUrl, opt => opt
             .MapFrom(src => src.PhotoUrl != null ? $"{appBaseUrl}/Uploads/" + src.PhotoUrl : ""));
            CreateMap<Contragent_Detail, ContragentGetEditDto>();
            //Post
            CreateMap<ContragentPostDto, Contragent>().ReverseMap();
            CreateMap<ContragentPostDto, Contragent_Detail>().ReverseMap();
            //Put
            CreateMap<ContragentPutDto, Contragent>().ReverseMap();
            CreateMap<ContragentPutDto, Contragent_Detail>().ReverseMap();
            #endregion
        }
    }
}
