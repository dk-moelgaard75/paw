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
                if (IsDataReady(guid))
                {
                    string htmlCalendar = BuildCalendar(guid, startDate);
                    SetCalendarHtml(guid, htmlCalendar);
                }
                else
                {
                    return null;
                }
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
                sb.Append("<table style='border: 1px solid black'>");
                sb.Append(BuildHtmlHeader(startDate));
            }

            return sb.ToString();
        }

        private string BuildHtmlHeader(DateTime startDate)
        {
            //At this point the calendar will allways be 7 days. This should be changed to use _configuration["CalenderSearchNrOfDays"] in the furture
            //and be in sync with the same value from TaskService
            DateTime endDate = startDate.AddDays(7);
            double nrOfDays = (startDate - endDate).TotalDays;
            StringBuilder sb = new StringBuilder();
            sb.Append("	<tr>");
            sb.Append("		<th>medarbejder</th>");
            sb.Append("		<th colspan='8'>17/5-2022</th>");
            sb.Append("		<th colspan='8'>18/5-2022</th>");
            sb.Append("		<th colspan='8'>18/5-2022</th>");
            sb.Append("		<th colspan='8' > 18/5-2022</th>");
            sb.Append("		<th colspan='8'>18/5-2022</th>");
            sb.Append("	</tr>");

            return sb.ToString();

        }

        private string BuildEmployee(List<CalendarEmployeeModel> employeeModel)
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }
    }
}
