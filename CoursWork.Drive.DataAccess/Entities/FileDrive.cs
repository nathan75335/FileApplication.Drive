﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoursWork.Drive.DataAccess.Entities;

public class FileDrive
{
    
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Size { get; set; }
    public string Extension { get; set; }
    public byte[] Content { get; set; }    
    
}
