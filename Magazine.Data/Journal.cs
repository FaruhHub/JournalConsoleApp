namespace Magazine.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Journal")]
    public partial class Journal
    {
        public Journal()
        {
        }
        public Journal(string message, DateTime? datetime)
        {
            this.Message = message;
            this.MessageDate = datetime != null ? (DateTime)datetime : DateTime.Now;
        }
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Message { get; set; }

        public DateTime MessageDate { get; set; }
    }
}
