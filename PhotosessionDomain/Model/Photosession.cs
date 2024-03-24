using System;
using System.Collections.Generic;

namespace PhotosessionDomain.Model;

public partial class Photosession
{
    public int Id { get; set; }

    public double Price { get; set; }

    public DateTime DateTime { get; set; }

    public int PhotosessionLocationId { get; set; }

    public string? Description { get; set; }

    public int PhotosessionTypeId { get; set; }

    public int PhotosessionStatusId { get; set; }

    public virtual PhotosessionLocation PhotosessionLocation { get; set; } = null!;

    public virtual PhotosessionStatus PhotosessionStatus { get; set; } = null!;

    public virtual PhotosessionType PhotosessionType { get; set; } = null!;

    public virtual ICollection<UserPhotosession> UserPhotosessions { get; set; } = new List<UserPhotosession>();
}
