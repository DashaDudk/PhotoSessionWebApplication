using System;
using System.Collections.Generic;

namespace PhotosessionDomain.Model;

public partial class UserPhotosession
{
    public int Id { get; set; }

    public int PhotosessionId { get; set; }

    public int UserId { get; set; }

    public virtual Photosession Photosession { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
