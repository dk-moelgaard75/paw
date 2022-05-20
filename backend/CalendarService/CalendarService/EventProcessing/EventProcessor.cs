using AutoMapper;
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

        public string GetCalendarHtml(string guid)
        {
            if (_htmlDictionary.ContainsKey(guid))
            {
                return _htmlDictionary[guid];
            }
            return null;
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
    }
}
