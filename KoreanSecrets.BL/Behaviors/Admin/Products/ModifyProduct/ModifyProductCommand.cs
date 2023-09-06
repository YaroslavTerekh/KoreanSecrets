using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ModifyProduct;

public class ModifyProductCommand : IRequest
{
    [JsonIgnore]
    public Guid ProductId { get; set; }

    public string Title { get; set; }

    public string Characteristics { get; set; }

    public string Usage { get; set; }

    public string Syllabes { get; set; }

    public long Price { get; set; }

    public Guid CategoryId { get; set; }

    public Guid BrandId { get; set; }

    public Guid SubCategoryId { get; set; }

    public Guid CountryId { get; set; }

    public Guid DemandId { get; set; }

    public ProductIcon Icon { get; set; } = ProductIcon.None;
}
