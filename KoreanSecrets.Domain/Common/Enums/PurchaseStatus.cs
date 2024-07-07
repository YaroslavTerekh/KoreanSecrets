using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.Enums;

public enum PurchaseStatus
{
    New = 0, // створено, не полачено
    
    Waiting = 1, // ОПЛАТА ОТРИМАНА,
    
    InProgress = 2,  // ЗАМОВЛЕННЯ В ОБРОБЦІ
    SendViaPost = 3, // Надіслано на пошту
    SendByAdmin = 4, // ЗАМОВЛЕННЯ В ПУНКТІ САМОВИВОЗУ 
    NotCompleted = 5, // є якісь нюанси і треба зідзвонитись і уточнити
    
    Success = 6, // відправлено і отримано
    Failure = 7  // скасовано,
    
}
