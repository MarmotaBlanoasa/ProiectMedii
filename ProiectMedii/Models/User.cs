﻿namespace ProiectMedii.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // Navigation properties
        public ICollection<Ticket> Tickets { get; set; }
    }

}