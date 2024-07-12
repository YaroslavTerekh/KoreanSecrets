using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Charts.GetCountStatistic;

public class GetCountStatisticHandler : IRequestHandler<GetCountStatisticQuery, Data>
{
    private readonly DataContext _context;

    public GetCountStatisticHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Data> Handle(GetCountStatisticQuery request, CancellationToken cancellationToken)
    {
        var totalPurchases = await _context.Purchases.Where(x =>
            x.PurchaseStatus == PurchaseStatus.New || x.PurchaseStatus == PurchaseStatus.Waiting).CountAsync(cancellationToken);
      
        var totalFeedbacks = await _context.Feedbacks.Where(x =>
            x.CreatedDate.Date == DateTime.Today.Date).CountAsync(cancellationToken);

        // кількість підписок на товар
        
        var totalReports = await _context.Reports.Where(x =>
            x.Status == ReportStatus.Awaiting).CountAsync(cancellationToken);
        
        return new Data
        {
            Purchases = totalPurchases,
            Feedbacks = totalFeedbacks,
            Reports = totalReports
        };
    }
}

public class Data
{
    public int Purchases { get; set; }
    
    public int Feedbacks { get; set; }
    
    public int Reports { get; set; }
}