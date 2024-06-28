using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class CommentDTO : BaseEntity
{
    public string CommentText { get; set; }

    public Guid UserId { get; set; }

    public UserDTO User { get; set; }

    public Guid ProductId { get; set; }

    public PageProductDTO Product { get; set; }

    public List<CommentDTO> Replies { get; set; } = new();
}
