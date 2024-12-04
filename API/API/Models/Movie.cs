﻿namespace API.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Producers { get; set; }
    public string Studios { get; set; }
    public int Year { get; set; }
    public bool Winner { get; set; }
}