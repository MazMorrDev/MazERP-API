namespace MazErpBack.Enums;

public enum DeliveryStatus
{
    Pending,        // Pendiente de entrega
    Processing,     // En proceso
    Shipped,        // Enviado
    InTransit,      // En tránsito
    Delivered,      // Entregado
    Cancelled,      // Cancelado
    Returned,       // Devuelto
    PartiallyDelivered // Parcialmente entregado
}
