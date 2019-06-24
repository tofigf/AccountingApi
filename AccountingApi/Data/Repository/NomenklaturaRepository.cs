using AccountingApi.Data.Repository.Interface;
using AccountingApi.Helpers;
using AccountingApi.Models;
using EOfficeAPI.Dtos.Nomenklatura.Product;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository
{
    public class NomenklaturaRepository : INomenklaturaRepository
    {
        private readonly DataContext _context;

        public NomenklaturaRepository(DataContext context)
        {
            _context = context;
        }
        //Worker
        #region Workers
        //Iscileri yaratmaq
        public async Task<Worker> CreateWorker(Worker workerCreate, int? companyId)
        {

            //sirkete gore getireceyik iscileri
            if (companyId == null)
                return null;
            workerCreate.CompanyId = Convert.ToInt32(companyId);

            //sekil yuklemek funksiyasi base 64
            workerCreate.PhotoFile = FileManager.Upload(workerCreate.PhotoFile);
            //seklini url-ni geri qaytarmaq ucun
            workerCreate.PhotoUrl = workerCreate.PhotoFile;
            workerCreate.RegisterDate = DateTime.Now;
            workerCreate.IsDeleted = false;
            await _context.Workers.AddAsync(workerCreate);
            await _context.SaveChangesAsync();

            return workerCreate;
        }
        //Iscileri elave melumatlarini yaratmaq
        public async Task<Worker_Detail> CreateWorker_Detail(Worker_Detail worker_DetailCreate, int workerId)
        {
            worker_DetailCreate.WorkerId = workerId;

            await _context.Worker_Details.AddAsync(worker_DetailCreate);
            await _context.SaveChangesAsync();

            return worker_DetailCreate;
        }
        //iscileri getirmek sirkete gore
        public async Task<PagedList<Worker>> GetWorkers(PaginationParam workerParam, int? companyId)
        {
            if (companyId == null)
                return null;

            var workers = _context.Workers.Where(w => w.CompanyId == companyId && w.IsDeleted == false).Include(i => i.Worker_Details)
                .OrderByDescending(d => d.Id).AsQueryable();

            if (!string.IsNullOrEmpty(workerParam.Name))
            {
                workers = workers.Where(d => d.Name.Contains(workerParam.Name));
            }
            if (!string.IsNullOrEmpty(workerParam.Surname))
            {
                workers = workers.Where(d => d.Name.Contains(workerParam.Surname));
            }

            return await PagedList<Worker>.CreateAsync(workers, workerParam.PageNumber, workerParam.PageSize);
        }
        //isci ve elave melumatinin getirilmesi
        public async Task<Worker> GetEditWorker(int? workerId, int? companyId)
        {
            if (workerId == null)
                return null;

            Worker worker = await _context.Workers.FirstOrDefaultAsync(f => f.Id == workerId && f.CompanyId == companyId && f.IsDeleted == false);

            return worker;
        }
        //elave melumatinin getirilmesi
        public async Task<Worker_Detail> GetEditWorkerDetail(int? workerId)
        {
            if (workerId == null)
                return null;

            Worker_Detail worker_Detail = await _context.Worker_Details.FirstOrDefaultAsync(s => s.WorkerId == workerId);

            return worker_Detail;
        }
        // iscini yenilemek
        public async Task<Worker> EditWorker(Worker workerEdit, int? id)
        {
            if (id == null)
                return null;

            if (workerEdit.PhotoFile != null && workerEdit.PhotoFile != "")
            {
                //database-de yoxlayiriq eger bize gelen iscinin id-si ile eyni olan sekili silsin.
                string dbFileName = _context.Workers.FirstOrDefault(f => f.Id == id).PhotoUrl;
                if (dbFileName != null)
                {
                    FileManager.Delete(dbFileName);
                }
                workerEdit.PhotoFile = FileManager.Upload(workerEdit.PhotoFile);
                //seklin url teyin edirik
                workerEdit.PhotoUrl = workerEdit.PhotoFile;
            }
            _context.Workers.Update(workerEdit);

            await _context.SaveChangesAsync();

            return workerEdit;
        }
        //iscinin elave melumatlarini yenilemek
        public async Task<Worker_Detail> EditWorker_Detail(Worker_Detail workerEdit, int? id)
        {

            if (_context.Worker_Details.Any(a => a.WorkerId == id))
            {
                // iscinin butun elave melumatlaini yenilemek
                _context.Worker_Details.Update(workerEdit);

                await _context.SaveChangesAsync();
            }

            return workerEdit;
        }
        //Delete
        public async Task<Worker> DeleteWorker(int? workerId, int? companyId)
        {
            if (companyId == null)
                return null;
            if (workerId == null)
                return null;
            Worker dbWorker = await _context.Workers.FirstOrDefaultAsync(f => f.Id == workerId && f.CompanyId == companyId);
            if (dbWorker == null)
                return null;
            dbWorker.IsDeleted = true;

            await _context.SaveChangesAsync();

            return dbWorker;

        }
        //check
        public async Task<bool> Checkworker(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Workers.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;
            //yoxluyuruq sirket databasede var 
            if (await _context.Companies.FirstOrDefaultAsync(a => a.Id == companyId) == null)
                return true;


            return false;
        }
        public async Task<bool> CheckWorkerId(int? workerId, int? companyId)
        {
            if (companyId == null)
                return true;
            if (workerId == null)
                return true;
            if (await _context.Workers.AnyAsync(a => a.CompanyId != companyId && a.Id == workerId))
                return true;

            return false;
        }
        #endregion

        //Product
        #region Products
        // Mehsul yaratmaq
        public async Task<Product> CreateProduct(Product product, int? companyId)
        {
            if (companyId == null)
                return null;
            //sekil yuklemek funksiyasi base 64
            product.PhotoFile = FileManager.Upload(product.PhotoFile);
            //seklini url-ni geri qaytarmaq ucun
            product.PhotoUrl = product.PhotoFile;
            product.CreatedAt = DateTime.Now;
            product.CompanyId = Convert.ToInt32(companyId);
            product.IsDeleted = false;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;

        }
        //Anbar yaratmaq
        public async Task<Stock> CreateStock(Stock stock, int? productId)
        {
            stock.ProductId = Convert.ToInt32(productId);

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return stock;

        }
        //Anbari getirmek
        public async Task<List<ProductGetDto>> GetProducts(PaginationParam productParam, int? companyId)
        {

            if (companyId == null)
                return null;
            List<Product> products = await _context.Products.Where(w => w.CompanyId == companyId && w.IsDeleted == false)
            .OrderByDescending(d => d.CreatedAt).ToListAsync();
            List<Stock> stocks = await _context.Stocks.Where(w => w.Product.CompanyId == companyId)
        .OrderByDescending(d => d.Product.CreatedAt).ToListAsync();

            var producstock = products.Join(stocks, m => m.Id, m => m.ProductId, (pro, st) => new ProductGetDto
            {
                Name = pro.Name,
                Category = pro.Category,
                SalePrice = st.SalePrice,
                Price = st.Price,
                IsSale = st.IsSale,
                IsPurchase = st.IsPurchase,
                Id = pro.Id,
                PhotoUrl = pro.PhotoUrl != null ? $"{MyHttpContext.AppBaseUrl}/Uploads/" + pro.PhotoUrl : ""
            }).ToList();

            //if (!string.IsNullOrEmpty(workerParam.Name))
            //{
            //    products = products.Where(d => d.Name.Contains(workerParam.Name));
            //}
            //return producstock;
            return producstock;
        }
        public async Task<List<ProductGetDto>> GetSaleProducts(PaginationParam productParam, int? companyId)
        {
            if (companyId == null)
                return null;
            List<Product> products = await _context.Products.Include(i => i.Unit).Where(w => w.CompanyId == companyId && w.IsDeleted == false)
            .OrderByDescending(d => d.CreatedAt).ToListAsync();
            List<Stock> stocks = await _context.Stocks.Where(w => w.Product.CompanyId == companyId && w.IsSale)
        .OrderByDescending(d => d.Product.CreatedAt).ToListAsync();

            var producstock = products.Join(stocks, m => m.Id, m => m.ProductId, (pro, st) => new ProductGetDto
            {
                Name = pro.Name,
                Category = pro.Category,
                SalePrice = st.SalePrice,
                Price = st.Price,
                IsSale = st.IsSale,
                IsPurchase = st.IsPurchase,
                Id = pro.Id,
                UnitName = pro.Unit.Name,
                PhotoUrl = pro.PhotoUrl != null ? $"{MyHttpContext.AppBaseUrl}/Uploads/" + pro.PhotoUrl : ""
            }).ToList();

            //if (!string.IsNullOrEmpty(workerParam.Name))
            //{
            //    products = products.Where(d => d.Name.Contains(workerParam.Name));
            //}
            //return producstock;
            return producstock;
        }
        public async Task<List<ProductGetDto>> GetPurchaseProducts(PaginationParam productParam, int? companyId)
        {
            if (companyId == null)
                return null;
            List<Product> products = await _context.Products.Include(i => i.Unit).Where(w => w.CompanyId == companyId && w.IsDeleted == false)
            .OrderByDescending(d => d.CreatedAt).ToListAsync();
            List<Stock> stocks = await _context.Stocks.Where(w => w.Product.CompanyId == companyId && w.IsPurchase)
        .OrderByDescending(d => d.Product.CreatedAt).ToListAsync();

            var producstock = products.Join(stocks, m => m.Id, m => m.ProductId, (pro, st) => new ProductGetDto
            {
                Name = pro.Name,
                Category = pro.Category,
                SalePrice = st.SalePrice,
                Price = st.Price,
                IsSale = st.IsSale,
                IsPurchase = st.IsPurchase,
                Id = pro.Id,
                UnitName = pro.Unit.Name,
                PhotoUrl = pro.PhotoUrl != null ? $"{MyHttpContext.AppBaseUrl}/Uploads/" + pro.PhotoUrl : ""
            }).ToList();

            //if (!string.IsNullOrEmpty(workerParam.Name))
            //{
            //    products = products.Where(d => d.Name.Contains(workerParam.Name));
            //}
            //return producstock;
            return producstock;
        }
        //Deyismek ucun Mehsul ve anbari getirmek
        public async Task<Product> GetEditProduct(int? productId, int? companyId)
        {
            if (productId == null)
                return null;
            Product product = await _context.Products.Include(i => i.Unit).FirstOrDefaultAsync(f => f.Id == productId && f.IsDeleted == false
              && f.CompanyId == companyId);

            return product;
        }
        public async Task<Stock> GetEditStock(int? productId)
        {
            if (productId == null)
                return null;
            Stock stock = await _context.Stocks.FirstOrDefaultAsync(f => f.ProductId == productId);

            return stock;
        }
        //Mehsulu deyismek
        public async Task<Product> EditProduct(Product productEdit, int? productId)
        {
            if (productId == null)
                return null;

            if (productEdit.PhotoFile != null && productEdit.PhotoFile != "")
            {
                //database-de yoxlayiriq eger bize gelen iscinin id-si ile eyni olan sekili silsin.
                string dbFileName = _context.Workers.FirstOrDefault(f => f.Id == productId).PhotoUrl;

                if (dbFileName != null)
                {
                    FileManager.Delete(dbFileName);
                }
                productEdit.PhotoFile = FileManager.Upload(productEdit.PhotoFile);
                //seklin url teyin edirik
                productEdit.PhotoUrl = productEdit.PhotoFile;
            }
            _context.Products.Update(productEdit);

            await _context.SaveChangesAsync();

            return productEdit;
        }
        //Anbari Deyismek
        public async Task<Stock> EditStock(Stock stockEdit, int? productId)
        {
            if (productId == null)
                return null;
            if (_context.Stocks.Any(a => a.ProductId == productId))
            {

                // mehsul butun elave melumatlarini yenilemek
                _context.Stocks.UpdateRange(stockEdit);

                await _context.SaveChangesAsync();
            }
            return stockEdit;
        }
        //Delete
        public async Task<Product> DeleteProduct(int? productId, int? companyId)
        {
            if (companyId == null)
                return null;
            if (productId == null)
                return null;

            Product dbProduct = await _context.Products.FirstOrDefaultAsync(f => f.Id == productId && f.CompanyId == companyId);
            if (dbProduct == null)
                return null;
            dbProduct.IsDeleted = true;

            await _context.SaveChangesAsync();

            return dbProduct;

        }
        //yoxlamaq
        public async Task<bool> CheckProduct(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Products.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;
            //yoxluyuruq sirket databasede var 
            if (await _context.Companies.FirstOrDefaultAsync(a => a.Id == companyId) == null)
                return true;


            return false;
        }
        public async Task<bool> CheckProductId(int? productId, int? companyId)
        {
            if (companyId == null)
                return true;
            if (productId == null)
                return true;
            if (await _context.Products.AnyAsync(a => a.CompanyId != companyId && a.Id == productId))
                return true;

            return false;
        }
        #endregion

        //Elaqeler
        #region Contragent
        //elaqeleri yaratmaq
        public async Task<Contragent> CreateContragent(Contragent contragentCreate, int? companyId)
        {
            //sirkete gore getireceyik elaqeleri
            if (companyId == null)
                return null;
            contragentCreate.CompanyId = Convert.ToInt32(companyId);
            //sekil yuklemek funksiyasi base 64
            contragentCreate.PhotoFile = FileManager.Upload(contragentCreate.PhotoFile);
            //seklini url-ni geri qaytarmaq ucun
            contragentCreate.PhotoUrl = contragentCreate.PhotoFile;
            contragentCreate.CreetedAt = DateTime.Now;
            contragentCreate.IsDeleted = false;
            await _context.Contragents.AddAsync(contragentCreate);
            await _context.SaveChangesAsync();

            return contragentCreate;
        }
        public async Task<Contragent_Detail> CreateContragent_Detail(Contragent_Detail contragent_DetailCreate, int? contragentId)
        {

            contragent_DetailCreate.ContragentId = Convert.ToInt32(contragentId);

            await _context.Contragent_Details.AddAsync(contragent_DetailCreate);
            await _context.SaveChangesAsync();

            return contragent_DetailCreate;
        }
        //Get
        public async Task<PagedList<Contragent>> GetContragents(PaginationParam contragentParam, int? companyId)
        {
            var contragents = _context.Contragents.Where(w => w.CompanyId == companyId && w.IsDeleted == false)
                .OrderByDescending(d => d.Id).AsQueryable();

            if (!string.IsNullOrEmpty(contragentParam.Name))
            {
                contragents = contragents.Where(d => d.CompanyName.Contains(contragentParam.Name));
            }
            if (!string.IsNullOrEmpty(contragentParam.Surname))
            {
                contragents = contragents.Where(d => d.Fullname.Contains(contragentParam.Surname));
            }

            return await PagedList<Contragent>.CreateAsync(contragents, contragentParam.PageNumber, contragentParam.PageSize);
        }
        //GetCostumerContragents
        public async Task<PagedList<Contragent>> GetCostumerContragents(PaginationParam contragentParam, int? companyId)
        {
            var contragents = _context.Contragents.Where(w => w.CompanyId == companyId && w.IsDeleted == false && w.IsCostumer)
                .OrderByDescending(d => d.Id).AsQueryable();

            if (!string.IsNullOrEmpty(contragentParam.Name))
            {
                contragents = contragents.Where(d => d.CompanyName.Contains(contragentParam.Name));
            }
            if (!string.IsNullOrEmpty(contragentParam.Surname))
            {
                contragents = contragents.Where(d => d.Fullname.Contains(contragentParam.Surname));
            }

            return await PagedList<Contragent>.CreateAsync(contragents, contragentParam.PageNumber, contragentParam.PageSize);
        }
        //GetSallerContragents
        public async Task<PagedList<Contragent>> GetSallerContragents(PaginationParam contragentParam, int? companyId)
        {
            var contragents = _context.Contragents.Where(w => w.CompanyId == companyId && w.IsDeleted == false && w.IsCostumer == false)
                .OrderByDescending(d => d.Id).AsQueryable();

            if (!string.IsNullOrEmpty(contragentParam.Name))
            {
                contragents = contragents.Where(d => d.CompanyName.Contains(contragentParam.Name));
            }
            if (!string.IsNullOrEmpty(contragentParam.Surname))
            {
                contragents = contragents.Where(d => d.Fullname.Contains(contragentParam.Surname));
            }

            return await PagedList<Contragent>.CreateAsync(contragents, contragentParam.PageNumber, contragentParam.PageSize);
        }

        public async Task<Contragent> GetEditContragent(int? contragentId, int? companyId)
        {

            if (contragentId == null)
                return null;
            Contragent worker = await _context.Contragents.FirstOrDefaultAsync(f => f.Id == contragentId && f.CompanyId == companyId);

            return worker;
        }
        //elave melumatinin getirilmesi
        public async Task<Contragent_Detail> GetEditContragent_Detail(int? contragentId)
        {
            if (contragentId == null)
                return null;

            Contragent_Detail worker_Detail = await _context.Contragent_Details.FirstOrDefaultAsync(s => s.ContragentId == contragentId);

            return worker_Detail;
        }
        //deyismek 
        public async Task<Contragent> EditContragent(Contragent contragentEdit, int? contragentId)
        {
            if (contragentId == null)
                return null;

            if (contragentEdit.PhotoFile != null && contragentEdit.PhotoFile != "")
            {  //database-de yoxlayiriq eger bize gelen elaqe id-si ile eyni olan sekili silsin.
                string dbFileName = _context.Contragents.FirstOrDefault(f => f.Id == contragentId).PhotoUrl;
                if (dbFileName != null)
                {
                    FileManager.Delete(dbFileName);
                }
                contragentEdit.PhotoFile = FileManager.Upload(contragentEdit.PhotoFile);
                //seklin url teyin edirik
                contragentEdit.PhotoUrl = contragentEdit.PhotoFile;
            }
            _context.Contragents.Update(contragentEdit);

            await _context.SaveChangesAsync();

            return contragentEdit;
        }
        public async Task<Contragent_Detail> EditContragent_Detail(Contragent_Detail contragentDetailEdit, int? contragentId)
        {
            if (_context.Contragent_Details.Any(a => a.ContragentId == contragentId))
            {
                // iscinin butun elave melumatlaini yenilemek
                _context.Contragent_Details.Update(contragentDetailEdit);

                await _context.SaveChangesAsync();
            }

            return contragentDetailEdit;
        }
        //Check
        public async Task<bool> CheckContragent(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Contragents.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;
            //yoxluyuruq sirket databasede var 
            if (await _context.Companies.FirstOrDefaultAsync(a => a.Id == companyId) == null)
                return true;

            return false;
        }
        public async Task<bool> CheckContragentId(int? contragentId, int? companyId)
        {
            if (companyId == null)
                return true;
            if (contragentId == null)
                return true;
            if (await _context.Contragents.AnyAsync(a => a.CompanyId != companyId && a.Id == contragentId))
                return true;

            return false;
        }
        //Delete
        public async Task<Contragent> DeleteContragent(int? contragentId, int? companyId)
        {
            if (companyId == null)
                return null;
            if (contragentId == null)
                return null;

            Contragent dbContragent = await _context.Contragents.FirstOrDefaultAsync(f => f.Id == contragentId && f.CompanyId == companyId);
            if (dbContragent == null)
                return null;
            dbContragent.IsDeleted = true;

            await _context.SaveChangesAsync();

            return dbContragent;

        }

        #endregion

    }
}
