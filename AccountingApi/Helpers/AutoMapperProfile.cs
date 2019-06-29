using AccountingApi.Dtos;
using AccountingApi.Dtos.AccountsPlan;
using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Nomenklatura.Kontragent;
using AccountingApi.Dtos.Nomenklatura.Product;
using AccountingApi.Dtos.Nomenklatura.Worker;
using AccountingApi.Dtos.Sale.Income;
using AccountingApi.Dtos.Sale.Invoice;
using AccountingApi.Dtos.Sale.Proposal;
using AccountingApi.Dtos.User;
using AccountingApi.Models;
using AutoMapper;
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

            //AccountsPlan
            #region AccounstPlan
            CreateMap<AccountsPlan, AccountsPlanGetDto>().ReverseMap();
            #endregion

            //Proposal
            #region Proposal

            //Post
            CreateMap<ProposalPostDto, Proposal>();
            CreateMap<ProposalItemPostDto, ProposalItem>();
            CreateMap<ContragentPutInProposalDto, Contragent>();
            CreateMap<CompanyPutProposalDto, Company>();

            //Get
            CreateMap<Proposal, ProposalGetDto>();
            CreateMap<Tax, ProposalEditGetDto>().ReverseMap();
            CreateMap<Proposal, ProposalEditGetDto>()
                       .ForMember(dto => dto.ProposalItemGetDtos, opt => opt
                    .MapFrom(src => src.ProposalItems.Select(s => new
                    {
                        ProductName = s.Product.Name,
                        s.Id,
                        s.SellPrice,
                        s.Qty,
                        s.ProductId,
                        s.TotalOneProduct,
                        s.Price
                    })));

            CreateMap<Company, ProposalEditGetDto>();
            CreateMap<Contragent, ProposalEditGetDto>();
            CreateMap<ProposalItem, ProposalItemGetDto>();
            //   .AfterMap((src, dto) =>  { dto.ProposalItems = src.ProposalItemPostDtos; });

            //Put
            CreateMap<ProposalPutDto, Proposal>().ReverseMap();
            CreateMap<ProposalItemPutDto, ProposalItem>().ReverseMap();
            #endregion

            //Invoice
            #region Invoice
            //Post
            CreateMap<InvoicePostDto, Invoice>().ReverseMap();
            CreateMap<InvoiceItemPostDto, InvoiceItem>().ReverseMap();
            //Get
            CreateMap<Invoice, InvoiceGetDto>().ReverseMap();
            //get for edit
            CreateMap<Invoice, InvoiceEditGetDto>()
                  .ForMember(dto => dto.InvoiceItemGetDtos, opt => opt
                    .MapFrom(src => src.InvoiceItems.Select(s => new {
                        ProductName = s.Product.Name,
                        s.Id,
                        s.SellPrice,
                        s.Qty,
                        s.ProductId,
                        s.TotalOneProduct,
                        s.Price

                    })));
            CreateMap<Company, InvoiceEditGetDto>();
            CreateMap<Contragent, InvoiceEditGetDto>();
            CreateMap<Tax, InvoiceEditGetDto>().ReverseMap();
            CreateMap<AccountsPlan, InvoiceEditGetDto>().ReverseMap();

            CreateMap<InvoiceItem, InvoiceEditGetDto>();
            //Put
            CreateMap<InvoicePutDto, Invoice>().ReverseMap();
            CreateMap<InvoiceItemPutDto, InvoiceItem>().ReverseMap();
            #endregion

            //Income
            #region Income
            //Get
            CreateMap<Invoice, IncomeInvoiceGetDto>();
            CreateMap<IncomeItem, IncomeGetDto>().ReverseMap();
            CreateMap<Income, IncomeEditGetDto>()
                  .ForMember(dto => dto.IncomeItemGetDtos, opt => opt
                    .MapFrom(src => src.IncomeItems.Select(s => new {
                        s.IsBank,
                        s.PaidMoney,
                        s.Id,
                        s.AccountDebitId,
                        s.AccountKreditId,
                        s.InvoiceNumber,
                        s.Residue,
                        s.InvoiceId,
                        s.TotalOneInvoice
                    }).ToList()));
            //https://localhost:44317/api/income/geteditincome
            //get edit income
            CreateMap<Invoice, IncomeInvoiceEditGetDto>().ForMember(dto => dto.IncomeItemInvoiceGetDtos, opt => opt
                    .MapFrom(src => src.IncomeItems.Select(s => new {

                        s.IsBank,
                        s.PaidMoney,
                        s.Id,
                        IncomeCreatedAt = s.Income.CreatedAt,
                        s.TotalOneInvoice,
                        s.AccountDebitId,
                        s.AccountKreditId,
                        s.Date
                    })));

            CreateMap<IncomeItem, IncomeItemInvoiceGetDto>().ReverseMap();
            CreateMap<Income, IncomeItemInvoiceGetDto>().ReverseMap();

            CreateMap<IncomeItem, IncomeItemGetDto>().ReverseMap();

            //Post
            CreateMap<IncomePostDto, Income>();
            CreateMap<IncomeItemPostDto, IncomeItem>().ReverseMap();
            CreateMap<IncomeItem,IncomeItemGetEditDto>().ReverseMap();

            //Put
            CreateMap<IncomePutDto, Income>().ReverseMap();
            //.ForMember(m => m, opt => opt.Ignore());
            CreateMap<IncomeItemGetEditDto, IncomeItem>().ReverseMap();
            //.EqualityComparison((sir, si) => sir.Id == si.Id);

            #endregion

        }
    }
}
