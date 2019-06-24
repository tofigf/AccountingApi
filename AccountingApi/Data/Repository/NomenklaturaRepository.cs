using AccountingApi.Data.Repository.Interface;
using AccountingApi.Helpers;
using AccountingApi.Models;
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

    }
}
