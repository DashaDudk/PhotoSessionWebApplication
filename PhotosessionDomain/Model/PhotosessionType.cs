using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotosessionDomain.Model;

public partial class PhotosessionType
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The field must not be empty")]
    [Display(Name = "Photosession type")]
    public string TypeName { get; set; } = null!;

    public virtual ICollection<Photosession> Photosessions { get; set; } = new List<Photosession>();
}