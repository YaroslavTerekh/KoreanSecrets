using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddPhotoToVolumeList;

public class AddPhotoToVolumeListCommand : IRequest
{
    public Guid VolumeId { get; set; }

    public IFormFile Photo { get; set; }
}
