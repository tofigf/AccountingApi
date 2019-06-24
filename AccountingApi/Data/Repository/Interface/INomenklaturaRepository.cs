using AccountingApi.Models;
using EOfficeAPI.Dtos.Nomenklatura.Product;
using EOfficeAPI.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
 public  interface INomenklaturaRepository
    {
        //Isciler
        #region Worker
        //Isci
        Task<Worker> CreateWorker(Worker workerCreate, int? companyId);
        //iscinin elave melumatlari
        Task<Worker_Detail> CreateWorker_Detail(Worker_Detail worker_DetailCreate, int workerId);
        //Isci Get
        Task<PagedList<Worker>> GetWorkers(PaginationParam workerParam, int? companyId);
        //Isci deyis get 
        Task<Worker> GetEditWorker(int? workerId, int? companyId);
        Task<Worker_Detail> GetEditWorkerDetail(int? workerId);
        //Isci deyis
        Task<Worker> EditWorker(Worker workerEdit, int? id);
        //Isci elave melumatlari deyis 
        Task<Worker_Detail> EditWorker_Detail(Worker_Detail workerEdit, int? id);
        //silmek
        Task<Worker> DeleteWorker(int? workerId, int? companyId);
        //yoxlamaq
        Task<bool> Checkworker(int? currentUserId, int? companyId);
        Task<bool> CheckWorkerId(int? workerId, int? companyId);
        #endregion

        //Mehsullar
        #region Product
        //mehsul yaratmaq
        Task<Product> CreateProduct(Product product, int? companyId);
        //mehsulu anbara elave etmek
        Task<Stock> CreateStock(Stock stocks, int? productId);
        //mehsullari getirmek
        Task<List<ProductGetDto>> GetProducts(PaginationParam productParam, int? companyId);
        Task<List<ProductGetDto>> GetPurchaseProducts(PaginationParam productParam, int? companyId);
        //mehsul edit get 
        Task<Product> GetEditProduct(int? productId, int? companyId);
        Task<List<ProductGetDto>> GetSaleProducts(PaginationParam productParam, int? companyId);
        Task<Stock> GetEditStock(int? productId);
        //mehsul deyis
        Task<Product> EditProduct(Product productEdit, int? productId);
        //anbar deyis 
        Task<Stock> EditStock(Stock stockEdit, int? productId);
        //mehsulu silmek
        Task<Product> DeleteProduct(int? workerId, int? companyId);
        //yoxlamaq
        Task<bool> CheckProduct(int? currentUserId, int? companyId);
        Task<bool> CheckProductId(int? productId, int? companyId);
        #endregion

        //Elaqeler
        #region Contragent
        //Isci
        Task<Contragent> CreateContragent(Contragent contragentCreate, int? companyId);
        //iscinin elave melumatlari
        Task<Contragent_Detail> CreateContragent_Detail(Contragent_Detail contragent_DetailCreate, int? contragentId);
        //Elaqeler Get
        Task<PagedList<Contragent>> GetContragents(PaginationParam contragentParam, int? companyId);
        Task<PagedList<Contragent>> GetSallerContragents(PaginationParam contragentParam, int? companyId);
        Task<PagedList<Contragent>> GetCostumerContragents(PaginationParam contragentParam, int? companyId);
        Task<Contragent> GetEditContragent(int? contragentId, int? companyId);

        Task<Contragent_Detail> GetEditContragent_Detail(int? contragentId);

        Task<Contragent> EditContragent(Contragent contragentEdit, int? contragentId);
        Task<Contragent_Detail> EditContragent_Detail(Contragent_Detail contragentDetailEdit, int? contragentId);
        //Check
        Task<bool> CheckContragent(int? currentUserId, int? companyId);
        Task<bool> CheckContragentId(int? contragentId, int? companyId);
        //Delete
        Task<Contragent> DeleteContragent(int? contragentId, int? companyId);
        #endregion
    }
}
