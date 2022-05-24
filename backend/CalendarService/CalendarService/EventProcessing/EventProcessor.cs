using AutoMapper;
using CalendarService.Data;
using CalendarService.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarService.EventProcessing
{
    public class EventProcessor :IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly IMapper _mapper;
        private Dictionary<string, string> _htmlDictionary;
        private int NrOfDaysToDisplay = 8;
        //public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            //_mapper = mapper;
            _htmlDictionary = new Dictionary<string, string>();
            //for test
            _htmlDictionary.Add("3", TestHtml());
        }

        public string GetCalendarHtml(string guid, DateTime startDate)
        {
            if (!_htmlDictionary.ContainsKey(guid))
            {
                while (!IsDataReady(guid))
                {
                    //Hack
                    System.Threading.Thread.Sleep(3000);
                }
                string htmlCalendar = BuildCalendar(guid, startDate);
                SetCalendarHtml(guid, htmlCalendar);
            }
            return _htmlDictionary[guid];
            
        }


        public void SetCalendarHtml(string guid,string html)
        {
            if (_htmlDictionary.ContainsKey(guid))
            {
                _htmlDictionary[guid] = html;
            }
            else
            {
                _htmlDictionary.Add(guid, html);
            }
        }
        private string TestHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table style='border: 1px solid black'>");
            sb.Append("	<tr>");
            sb.Append("		<th>medarbejder</th>");
            sb.Append("		<th colspan='8'>17/5-2022</th>");
            sb.Append("		<th colspan='8'>18/5-2022</th>");
            sb.Append("		<th colspan='8'>18/5-2022</th>");
            sb.Append("		<th colspan='8' > 18/5-2022</th>");
            sb.Append("		<th colspan='8'>18/5-2022</th>");
            sb.Append("	</tr>");
            sb.Append("	<tr>");
            sb.Append("		<td>empA</td>");
            sb.Append("		<td bgcolor='green'>8</td>");
            sb.Append("		<td>9</td>");
            sb.Append("		<td>10</td>");
            sb.Append("		<td>11</td>");
            sb.Append("		<td>12</td>");
            sb.Append("		<td>13</td>");
            sb.Append("		<td>14</td>");
            sb.Append("		<td bgcolor='green'> 15</td>");
            sb.Append("	</tr>");
            sb.Append("	<tr>");
            sb.Append("		<td>empB</td>");
            sb.Append("		<td bgcolor='red'> 8</td>");
            sb.Append("		<td>9</td>");
            sb.Append("		<td>10</td>");
            sb.Append("		<td>11</td>");
            sb.Append("		<td>12</td>");
            sb.Append("		<td>13</td>");
            sb.Append("		<td>14</td>");
            sb.Append("		<td>15</td>");
            sb.Append("	</tr>");
            sb.Append("	<tr>");
            sb.Append("		<td>empC</td>");
            sb.Append("	</tr>	");
            sb.Append("</table>");
            return sb.ToString();
        }
        private bool IsDataReady(string guid)
        {
            bool dataIsReady = false;
            Guid calendarGuid = Guid.Parse(guid);
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICalendarRepository>();
                CalendarModel model =  repo.GetCalendarByCalendarGuid(calendarGuid);
                if (model.TaskDone == 1 && model.EmployeeDone == 1)
                {
                    dataIsReady = true;
                }
            }
            return dataIsReady;
        }

        private string BuildCalendar(string guid, DateTime startDate)
        {
            Guid calendarGuid = Guid.Parse(guid);
            StringBuilder sb = new StringBuilder();
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICalendarRepository>();
                List<CalendarEmployeeModel> employeeModel = repo.GetCalendarEmployeeByCalendarGuid(calendarGuid).ToList();
                List<CalendarTaskObjModel> taskModel = repo.GetCalendarTaskObjByCalendarGuid(calendarGuid).ToList();
                sb.Append("<table style=\"border: 1px solid black\">");
                
                //loop thru employees and print red/green based on whether there´s tasks or not (grey for weekends)
                foreach(CalendarEmployeeModel employee in employeeModel)
                {
                    List<CalendarTaskObjModel> subList = taskModel.Where(p => p.Employee == employee.EmployeeGuid).ToList();
                    sb.Append(BuildEmployeeHtml(employee,subList,startDate,1 ));
                    sb.Append(BuildEmployeeHtml(employee, subList, startDate, 5));
                }
                sb.Append("</table>");
            }

            return sb.ToString(); //.Replace("<","&lt;").Replace(">","&gt;");
        }
        public string BuildHtmlHeader(DateTime startDate)
        {
            //At this point the calendar will allways be 'NrOfDaysToDisplay' days (8). This should be changed to use _configuration["CalenderSearchNrOfDays"] in the furture
            //and be in sync with the same value from TaskService
            DateTime endDate = startDate.AddDays(NrOfDaysToDisplay);
            double nrOfDays = (endDate - startDate).TotalDays;
            StringBuilder sb = new StringBuilder();
            sb.Append("	<tr>");
            sb.Append("		<th>medarbejder</th>");
            for (int i = 0; i < nrOfDays; i++)
            {
                sb.Append("		<th colspan='9'>" + startDate.AddDays(i).ToString("dd-MM-yyyy") + "</th>");
            }
            sb.Append("	</tr>");

            return sb.ToString();

        }

        public string BuildEmployeeHtml(CalendarEmployeeModel employeeModel, List<CalendarTaskObjModel> subList, DateTime startDate, int startValue)
        {
            List<string> taskItems = GetTaskItems(subList);
            StringBuilder sb = new StringBuilder();
            DateTime endDate = startDate.AddDays(NrOfDaysToDisplay);
            double nrOfDays = (endDate - startDate).TotalDays;

            //Build firstpart (employee)
            sb.Append("	<tr>");
            if (startValue == 1)
            {
                sb.Append("		<th>medarbejder</th>");
            }
            else
            {
                sb.Append("		<th></th>");
            }
            for (int i = startValue; i <= nrOfDays; i++)
            {
                sb.Append("		<th colspan='9'>" + startDate.AddDays(i-1).ToString("dd-MM-yyyy") + "</th>");

                if (i % 4 == 0)
                {
                    //close this part of the table and start over
                    sb.Append("	</tr>");
                    sb.Append("	<tr>");
                    sb.Append("		<td>" + employeeModel.FirstName + " " + employeeModel.LastName + "</td>");
                    break;
                }
            }

            //Build (for each date) 8 section representing the 8 slots avaible
            for (int i = startValue; i <= nrOfDays; i++)
            {
                DateTime currentDate = startDate.AddDays(i-1);
                //8 o´clock is the starthour - 16 (4 p.m.) end hour
                for (int j = 8; j <= 16; j++)
                {
                    string currentColor = "green";
                    string hourItem = j.ToString().PadLeft(2, '0');
                    string dateItem = currentDate.ToString("yyyyMMdd") + hourItem;
                        
                    if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        currentColor = "grey";
                    }
                    if (taskItems.Contains(dateItem))
                    {
                        currentColor = "red";
                    }

                    sb.Append("		<td bgcolor='" + currentColor + "'>" + hourItem + "</td>");
                }
                if (i % 4 == 0)
                {
                    //close this part of the table and start over
                    break;
                }
            }
            sb.Append("	</tr>");

            return sb.ToString();
        }


        private string BuildEmployee(List<CalendarTaskObjModel> employeeModel)
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }
        /// <summary>
        /// Creates a list of dateitem based on the list of calendarojects recieved. The calendarobject has a startdate, a starttime and an estimate hours. If 
        /// the estimated hours are greate than one the methode returns an item for each hour. I.E - if the calendarobj has 5/6-2022 as startdate, 8 (a.m) as starttime and 
        /// an estimated hours of 3 the method will return a list containing the followin values: 2022050608, 2022050609, 2022050610, 2022050611
        /// </summary>
        /// <param name="subList">A list of CalendarTaskObjModel.</param>
        /// <returns>A list of dateitem describing workitem on the form YYYYMMDDHH - like: 2022050610 indicating a workitem at 10 a.m. on the 6. of May 2022</returns>
        public List<string> GetTaskItems(List<CalendarTaskObjModel> subList)
        {
            int dailyHourStart = 8; //every workday starts at 8 o'clock
            List<string> resultList = new List<string>();

            foreach (CalendarTaskObjModel obj in subList)
            {
                string dateObj = obj.StartDate.ToString("yyyyMMdd");
                int startTime = obj.StartTime; //.ToString().PadLeft(2, '0');
                int estimatedHours = obj.EstimatedHours;
                for (int i = 0; i < estimatedHours; i++)
                {
                    string startValue = (startTime + i + dailyHourStart).ToString().PadLeft(2, '0');
                    string itemVal = dateObj + startValue;
                    resultList.Add(itemVal);
                }
            }
            return resultList;
        }

    }
}
