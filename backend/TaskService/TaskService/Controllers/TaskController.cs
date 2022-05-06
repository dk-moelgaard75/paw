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
namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskObjRepository _taskObjRepository;
        private readonly IMapper _mapper;
        public TaskController(ITaskObjRepository taskObjRepository, IMapper mapper)
        {
            _taskObjRepository = taskObjRepository;
            _mapper = mapper;
        }
        [HttpGet(Name = "GetAll")]
        public ActionResult<IEnumerable<TaskObjGetDto>> GetAll()
        {
            Console.WriteLine("TaskService - Http GET recieved");
            IEnumerable<TaskObject> tasks = _taskObjRepository.GetAll();
            //return JsonSerializer.Serialize(emps);
            return Ok(_mapper.Map<IEnumerable<TaskObjGetDto>>(tasks));
        }
        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<TaskObjGetDto> GetById(int id)
        {
            Console.WriteLine($"TaskService - Http GET recieved with ID: {id}");
            TaskObject task = _taskObjRepository.GetById(id);
            if (task != null)
            {
                return Ok(_mapper.Map<TaskObjGetDto>(task));
            }
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
            System.Diagnostics.Debug.WriteLine("TaskService - post");
            if (taskCreateDto == null)
            {
                //Console.WriteLine($"CustomerCreateDto is null:");

                return StatusCode(500);
            }
            //EmployeeCreateDto tmp = JsonSerializer.Deserialize<EmployeeCreateDto>(employeeCreateDto.ToString());
            Console.WriteLine($"Http POST recieved with data:");
            Console.WriteLine($"{taskCreateDto.TaskName}");
            Console.WriteLine($"{taskCreateDto.Description}");
            Console.WriteLine($"{taskCreateDto.StartDate.ToLongDateString()}");
            Console.WriteLine($"{taskCreateDto.EstimatedDays}");
            Console.WriteLine($"{taskCreateDto.CustomerGuid.ToString()}");

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
                Console.WriteLine($"{taskModel.EstimatedDays}");

                taskModel.EndDate = DateUtil.CalcEndDate(taskModel.StartDate, taskModel.EstimatedDays);

                //setting UID
                taskModel.UID = Guid.NewGuid();

                //Use the internal model object to create a DbContext object
                int createdEmployeeId = _taskObjRepository.Create(taskModel);

                //Map the DbContext object to a DTO object (removing potential personal or otherwise sensitive data)
                taskObjGetDto = _mapper.Map<TaskObjGetDto>(taskModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Http POST exception: {ex.Message}");
                return StatusCode(500);
            }

            return CreatedAtRoute(nameof(GetById), new { Id = taskObjGetDto.Id }, taskObjGetDto);
        }

    }
}
