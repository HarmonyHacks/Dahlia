using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dahlia.Repositories;
using System.IO;
using System.Text;
using Dahlia.Models;

namespace Dahlia.Services
{
    public interface IReportGeneratorService
    {
        byte[] GenerateRetreatsReportCsv();
        byte[] GenerateRetreatsReportCsvFor(int retreatId);
        byte[] GenerateRetreatsReportExcelHtml();
        byte[] GenerateRetreatReportExcelHtmlFor(int retreatId);
    }

    public class ReportGeneratorService : IReportGeneratorService
    {
        IRetreatRepository _retreatRepository;
        public ReportGeneratorService(IRetreatRepository retreats)
        {
            _retreatRepository = retreats;
        }

        public void AddReportLine(StringBuilder sb, string col0, string col1, string col2, string col3)
        {
            sb.AppendFormat("{0},{1},{2},{3}",
                        col0,
                        col1,
                        col2,
                        col3);
            sb.AppendLine(string.Empty);
        }

        public void AddRetreatLines(StringBuilder sb, Retreat retreat)
        {
            AddReportLine(sb,
                        "Name/ContactInfo",
                        "NOTES",
                        "Special Notes: Alergies/Dietary and Mobility Issues",
                        "Room Assignment");
            foreach (var reg in retreat.Registrations)
            {
                AddReportLine(sb,
                    reg.Participant.FirstName + " " + reg.Participant.LastName,
                    string.Empty,
                    reg.Participant.Notes,
                    reg.Bed.Code);
            }
        }

        public byte[] GenerateRetreatsReportCsvFor(int retreatId)
        {
            var retreat = _retreatRepository.GetById(retreatId);
            var sb = new StringBuilder();
            AddRetreatLines(sb, retreat);
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        const string _reportHtmlHeader = @"
        <html>
            <head>
                <style>
                    body
                    {
                        background-color:white;
                        color:black;
                        font-weight:normal;
                        font-size:large;
                    }
                    #root_div
                    {  
                        width:600px;
                        padding:10px;
                    }
                    .report_table
                    {
                        border-collapse:collapse;
                        width:100px;
                    }
                    .report_table td 
                    {
                        border: 1px solid black;
                    }
                    .retreat_header
                    {
                        font-weight:bolder;
                        font-size:x-large;
                        text-align:center;
                        padding: 10px auto 10px auto;
                        width:600px;
                        border:none;
                    }
                    .column_headers td
                    {
                        text-align:center;
                        font-weight:bolder;
                        font-size:larger;
                    }
                    .highlight_gray
                    {
                        background-color:#ddd;
                    }
                    .highlight_blue
                    {
                        background-color:#66f;
                    }
                    .highlight_rose
                    {
                        background-color:#f66;
                    }
                    .highlight_green
                    {
                        background-color:#6c6;
                    }
                    .highlight_orange
                    {
                        background-color:#f90;
                    }
                    .reg_row
                    {
                        padding:4px;
                    }
                </style>
            </head>
            <body>
                <div id='root_div'>
                <table class='report_table'>
        ";



        const string _reportHtmlRetreatInfo = @"
                    <tr>
                        <td class='retreat_header' colspan='4'>CONFIDENTIAL<br />CANCER RETREAT PARTICIPANTS<br />{0}<br />{1}</td>
                    </tr>
                    <tr class='column_headers'>
                        <td style='width:120px;'>NAME/CONTACT INFO</td>
                        <td style='width:300px;'>NOTES</td>
                        <td style='width:90px;'>Special Notes: Allergies, Dietary & Mobility Issues</td>
                        <td style='width:90px;'>Room Assignment</td>
                    </tr>
        ";

        const string _reportHtmlRegistrationInfo = @"
                    <tr class='reg_row'>
                        <td>{0}</td>
                        <td>
                            {1}
                        </td>
                        <td>&nbsp;</td>
                        <td class='highlight_{2}'>{3}</td>
                    </tr>
        ";

        const string _reportHtmlFooter = @"
                </table>
                </div>
            </body>
        </html>    
        ";
        public byte[] GenerateRetreatReportExcelHtmlFor(int retreatId)
        {
            var retreat = _retreatRepository.GetById(retreatId);
            if (retreat == null)
            {
                throw new Exception("barf.. null retreat");
            }
            var output = _reportHtmlHeader;
            output = GenerateReportHtmlForRetreat(retreat, output);
            output += _reportHtmlFooter;

            return Encoding.ASCII.GetBytes(output);
        }

        string GenerateReportHtmlForRetreat(Retreat retreat, string seed)
        {
            if (retreat != null)
            {

                seed += string.Format(_reportHtmlRetreatInfo, retreat.StartDate.ToLongDateString(), retreat.Description);

                seed = retreat.Registrations.OrderBy(x=>x.Bed == null ? "waitlist" : x.Bed.Code).Aggregate(seed, (memo, r) =>
                {
                    var name = r.Participant.FirstName + " " + r.Participant.LastName;
                    var notes = r.Participant.Notes;
                    var bedCode = r.Bed == null ? "waitlist" : r.Bed.Code;
                    var color = bedCode.StartsWith("waitlist") ? "gray" :
                        bedCode.StartsWith("CS") ? "rose" :
                        bedCode.StartsWith("L") ? "blue" :
                        bedCode.StartsWith("GH") ? "orange" :
                        "green";
                    return memo + string.Format(_reportHtmlRegistrationInfo, name, notes.Replace("\n", "<br />"), color, bedCode);
                });

            }
            return seed;
        }

        public byte[] GenerateRetreatsReportCsv()
        {
            var retreats = _retreatRepository.GetList();

            var sb = new StringBuilder();

            foreach (var retreat in retreats)
            {
                AddRetreatLines(sb, retreat);
            }
            
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
        public byte[] GenerateRetreatsReportExcelHtml()
        {
            var retreats = _retreatRepository.GetList();
            var output = _reportHtmlHeader;
            foreach (var retreat in retreats)
            {
                output = GenerateReportHtmlForRetreat(retreat, output);
            }
            output += _reportHtmlFooter;
            return Encoding.ASCII.GetBytes(output);
        }
    }
}