﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RouletteApiCleanCode.Models
{
    public class Roulette
    {
        [Key]
        public int RouletteId { get; set; }
        public bool RouletteState { get; set; }
        public DateTime? FHOpening { get; set; }
        public DateTime? FHClosing { get; set; }
    }
}
