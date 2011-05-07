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
    }
}