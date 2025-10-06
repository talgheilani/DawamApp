using System;
using System.ComponentModel.DataAnnotations;

namespace DawamApp.Models
{
    public enum StatusType
    {
        Training,
        Discharge,
        Holiday,
        Duty,
        Workshop
    }

    public class EmployeeStatus
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public StatusType Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
