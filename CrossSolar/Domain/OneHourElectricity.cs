using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossSolar.Domain
{
    public class OneHourElectricity
    {
        public int Id { get; set; }
        public int PanelId { get; set; }
        public Panel Panel { get; set; }
        public long KiloWatt { get; set; }

        public DateTime DateTime { get; set; }
    }
}