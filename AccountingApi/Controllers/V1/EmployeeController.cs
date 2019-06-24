using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Nomenklatura.Worker;
using AccountingApi.Helpers.Extentions;
using AccountingApi.Models;
using AutoMapper;
using EOfficeAPI.Helpers.Extentions;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly INomenklaturaRepository _repo;
        private readonly IMapper _mapper;

        public EmployeeController(INomenklaturaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        //Post [baseUrl]/api/employee/addworker
        [HttpPost]
        [Route("addworker")]
        public async Task<IActionResult> AddWorker(WorkerPostDto vwWorker, [FromHeader]int? companyId)
        {
            //Check
            #region Check
            if (vwWorker == null)
                return StatusCode(409, "object null");
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.Checkworker(currentUserId, companyId))
                return Unauthorized();
            //if (!ModelState.IsValid)
            //    return BadRequest();
            #endregion

            //workerpostdto mapping class worker 
            Worker worker = _mapper.Map<Worker>(vwWorker);
            //mapped worker class sending  in create repo  
            Worker workerToGet = await _repo.CreateWorker(worker, companyId);
            //return added worker object
            WorkerGetDto workerForReturn = _mapper.Map<WorkerGetDto>(workerToGet);
            //Worker_Detail 
            Worker_Detail worker_Details = _mapper.Map<Worker_Detail>(vwWorker);

            Worker_Detail worker_DetailToGet = await _repo.CreateWorker_Detail(worker_Details, worker.Id);

            return Ok(workerForReturn);
        }
        //Get [baseUrl]/api/employee/getworkers
        [HttpGet]
        [Route("getworkers")]
        // pagination yazilmsdi                                                        
        public async Task<IActionResult> GetWorkers([FromQuery]PaginationParam workerParam, [FromHeader]int? companyId)
        {
            //Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.Checkworker(currentUserId, companyId))
                return Unauthorized();
            ///////////////////////////////////////////////////////////////////////////////////
            //iscileri sirkete gore getirmek
            var workers = await _repo.GetWorkers(workerParam, companyId);
            //gelen datani dto ya  yazmaq
            var workersToReturn = _mapper.Map<IEnumerable<WorkerGetDto>>(workers);
            //sehifelenme
            Response.AddPagination(workers.CurrentPage, workers.PageSize,
              workers.TotalCount, workers.TotalPages);

            return Ok(workersToReturn);
        }
        //deyisilik etmek ucun iscini getiririk
        [HttpGet()]
        [Route("geteditworker")]
        public async Task<IActionResult> GetEditWorker([FromHeader]int? workerId, [FromHeader]int? companyId)
        {
            //Check
            #region Check
            if (workerId == null)
                return BadRequest("workerId null");
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.Checkworker(currentUserId, companyId))
                return Unauthorized();
            if (await _repo.CheckWorkerId(workerId, companyId))
                return StatusCode(406, "Not Acceptable");
            #endregion

            ///////////////////////////////////////////////////////////////////////////////////
            //iscini sirkete gore getirmek
            Worker worker = await _repo.GetEditWorker(workerId, companyId);
            //iscini elave melumatlarini getirmek
            Worker_Detail worker_Detail = await _repo.GetEditWorkerDetail(workerId);

            //2 obyekti  bir obyekte birlesdirmek
            var workerToReturn = _mapper.MergeInto<WorkerEditDto>(worker, worker_Detail);

            return Ok(workerToReturn);
        }
        //deyismek
        [HttpPut]
        [Route("updateworker")]
        //Update zamani bu metodla dolduracayiq
        public async Task<IActionResult> UpdateWorker([FromBody]WorkerPutDto workerPut, [FromHeader]int? workerId, [FromHeader] int? companyId)
        {
            //Ckeck
            #region Check
            if (!ModelState.IsValid)
                return BadRequest();
            if (workerPut == null)
                return StatusCode(409, "object null");
            if (workerId == null)
                return StatusCode(409, "workerId null");
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.Checkworker(currentUserId, companyId))
                return Unauthorized();
            #endregion

            ///////////////////////////////////////////////////////////////////////////////////
            //repoya id gonderirik ve o bize lazim olan iscini ve detail-ni getirir.
            Worker worker_FromRepo = await _repo.GetEditWorker(workerId, companyId);
            Worker_Detail Detail_FromRepo = await _repo.GetEditWorkerDetail(workerId);
            //mapp olunmus isci
            Worker workerMapped = _mapper.Map(workerPut, worker_FromRepo);
            //repoda iscini yenileyirik 
            Worker updatedWorker = await _repo.EditWorker(worker_FromRepo, workerId);
            //qaytarmaq ucun 
            WorkerGetDto workToReturn = _mapper.Map<WorkerGetDto>(updatedWorker);
            //iscilerin elave melumatlari
            //mapp olunmus iscinin detaili
            Worker_Detail worker_DetailToMapped = _mapper.Map(workerPut, Detail_FromRepo);
            //repoda iscini yenileyirik 
            Worker_Detail updatedWorker_Detail = await _repo.EditWorker_Detail(Detail_FromRepo, workerId);
            return Ok(workToReturn);
        }
        [HttpGet]
        [Route("deleteworker")]
        public async Task<IActionResult> DeleteWorker([FromHeader]int? workerId, [FromHeader]int? companyId)
        {
            //Ckeck
            #region Check
            if (workerId == null)
                return StatusCode(409, "workerId null");
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.Checkworker(currentUserId, companyId))
                return Unauthorized();
            #endregion

            Worker Deletedworker = await _repo.DeleteWorker(workerId, companyId);

            WorkerGetDto DeletedWorkerToReturn = _mapper.Map<WorkerGetDto>(Deletedworker);

            return Ok();
        }
    }
}