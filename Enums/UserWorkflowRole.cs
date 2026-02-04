namespace MazErpBack.Enums;

public enum UserWorkflowRole
{
    Owner,          // Acceso total
    Admin,          // Acceso total excepto a cambiar info del workflow
    Finance,        // Contable/Finanzas
    Sales,          // Ventas y comerciales
    Inventory,      // Inventario y almacén
    // He comentado producción puesto que no es parte del mvp
    // Production,     // Producción y operaciones
    // HumanResources,
    Spectator        // Puede ver pero no tocar, para auditorías
}
