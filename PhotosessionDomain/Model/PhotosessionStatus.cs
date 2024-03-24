using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotosessionDomain.Model;

public partial class PhotosessionStatus
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The field must not be empty")]
    [Display(Name = "Photosession status")]

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Photosession> Photosessions { get; set; } = new List<Photosession>();
}
