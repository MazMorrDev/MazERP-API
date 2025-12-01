namespace MazErpBack;

public enum ClientWorkflowRole
{
    Admin,          // Acceso total
    Member,         // Usuario básico
    Finance,        // Contable/Finanzas
    Sales,          // Ventas y comerciales
    Inventory,      // Inventario y almacén

    // He comentado producción puesto que no es parte del mvp
    // Production,     // Producción y operaciones
}
