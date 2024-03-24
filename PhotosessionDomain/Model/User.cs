using System;
using System.Collections.Generic;

namespace PhotosessionDomain.Model;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<UserPhotosession> UserPhotosessions { get; set; } = new List<UserPhotosession>();
}
