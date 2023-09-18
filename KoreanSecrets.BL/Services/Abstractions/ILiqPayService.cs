using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Abstractions;

public interface ILiqPayService
{
    public Task<string> GenerateForm(Guid purchaseId, CancellationToken cancellationToken = default);

    public Task ProcessCallbackAsync(Dictionary<string, string> data, CancellationToken cancellationToken);
}
