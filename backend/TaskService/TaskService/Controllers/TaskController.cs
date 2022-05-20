using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TaskService.DTOs;
using TaskService.Models;
using TaskService.Data;
using TaskService.Utils;
using TaskService.AsyncDataServices;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskObjRepository _taskObjRepository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public TaskController(ITaskObjRepository taskObjRepository,
                                IMapper mapper,
                                IMessageBusClient msgBusClient)
        {
            _taskObjRepository = taskObjRepository;
            _mapper = mapper;
            _messageBusClient = msgBusClient;
        }
        [HttpGet(Name = "Get")]
        public ActionResult Get()
        {
            return Ok();
        }
                

        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<TaskObjGetDto> GetById(Guid id)
        {
            PawLogger.DoLog($"TaskService - Http GET recieved with ID: {id}");
            return NotFound();
        }
        /* HttpPost creates new instance*/
        [HttpPost]
        public ActionResult<TaskObjGetDto> Post([FromBody] TaskObjCreateDto taskCreateDto)
        {
            /*
             * This method shows the AutoMapper function.
             * EndDate is not in TaskObjCreateDto since it´s being calculated here
             * But it is in TaskObjGetDto since it´s returned to the frontend
             * 
             */
            PawLogger.DoLog("TaskService - post");
            if (taskCreateDto == null)
            {
                //Console.WriteLine($"CustomerCreateDto is null:");

                return StatusCode(500);
            }
            //EmployeeCreateDto tmp = JsonSerializer.Deserialize<EmployeeCreateDto>(employeeCreateDto.ToString());
            PawLogger.DoLog($"Http POST recieved with data:");
            PawLogger.DoLog($"{taskCreateDto.TaskName}");
            PawLogger.DoLog($"{taskCreateDto.Description}");
            PawLogger.DoLog($"{taskCreateDto.StartDate.ToLongDateString()}");
            PawLogger.DoLog($"{taskCreateDto.EstimatedHours}");
            PawLogger.DoLog($"{taskCreateDto.CustomerGuid.ToString()}");

            /*
             * Remember to calculate end data (use Util.CalcEndDate)
             * 
             */
            TaskObjGetDto taskObjGetDto = null;
            try
            {
                //Map incomming DTO to a internal model object
                TaskObject taskModel = _mapper.Map<TaskObject>(taskCreateDto);
                Console.WriteLine($"Employee model created:");
                Console.WriteLine($"{taskModel.TaskName}");
                Console.WriteLine($"{taskModel.Description}");
                Console.WriteLine($"{taskModel.StartDate.ToLongDateString()}");
                Console.WriteLine($"{taskModel.EstimatedHours}");

                taskModel.EndDate = DateUtil.CalcEndDate(taskModel.StartDate, taskModel.EstimatedHours);

                //setting UID
                taskModel.TaskGuid = Guid.NewGuid();

                //Use the internal model object to create a DbContext object
                int createdEmployeeId = _taskObjRepository.Create(taskModel);

                //Map the DbContext object to a DTO object (removing potential personal or otherwise sensitive data)
                taskObjGetDto = _mapper.Map<TaskObjGetDto>(taskModel);

                TaskXEmployee taskX = new TaskXEmployee();
                taskX.EmployeeGuid = taskModel.Employee;
                taskX.TaskGuid = taskModel.TaskGuid;
                _taskObjRepository.CreateEmployee(taskX);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Http POST exception: {ex.Message}");
                return StatusCode(500);
            }

            return CreatedAtRoute(nameof(GetById), new { Id = taskObjGetDto.Id }, taskObjGetDto);
        }


        /*
         * These are keept for educational purpose - shows how to have serveral GET´s on the same contoller
         * 
        [HttpGet("rtest1/", Name = "GetRouteTest")] //URL: /api/task/rtest1/
        
        public ActionResult GetRouteTest()
        {
            PawLogger.DoLog("GetRouteTest");
            return Ok();
        }
        [HttpGet("rtest2/{id}",Name = "GetRouteTestWithId")] URL: /api/task/rtest2/00000000-0000-0000-0000-000000000000
        
        public ActionResult GetRouteTestWithId(Guid id)
        {
            PawLogger.DoLog("GetRouteTestWithId:" + id.ToString());
            return Ok();
        }
        */


    }
}
