using System;
using System.ComponentModel.DataAnnotations.Schema;
using EFCore.Enums;

namespace EFCore.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public ServiceStatus Status { get; set; }
    }
}
