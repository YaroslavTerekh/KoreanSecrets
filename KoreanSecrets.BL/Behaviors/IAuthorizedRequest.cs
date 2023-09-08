using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors;

public class IAuthorizedRequest : IRequest
{
    [JsonIgnore]
    public Guid CurrentUserId { get; set; }
}

public class IAuthorizedRequest<T> : IRequest<T>
{
    [JsonIgnore]
    public Guid CurrentUserId { get; set; }
}
