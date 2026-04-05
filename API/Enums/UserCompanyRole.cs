namespace MazErpAPI.Enums;

public enum UserCompanyRole
{
    Owner,          // Acceso total
    Admin,          // Acceso total excepto a cambiar info del company
    Finance,        // Contable/Finanzas
    Sales,          // Ventas y comerciales
    Inventory,      // Inventario y almacén
    // Comentados puesto que no son parte del mvp
    // Production,     // Producción y operaciones
    // HumanResources,  // Manejo del personal
    Spectator        // Puede ver pero no tocar, para auditorías
}
