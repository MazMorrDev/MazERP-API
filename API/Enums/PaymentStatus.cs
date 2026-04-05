namespace MazErpAPI.Enums;

public enum PaymentStatus
{
    Pending,        // Pendiente de pago
    Processing,     // En proceso
    Completed,      // Completado
    Failed,         // Fallido
    Refunded,       // Reembolsado
    PartiallyPaid,  // Parcialmente pagado
    Overdue,        // Vencido
    Cancelled       // Cancelado
}
