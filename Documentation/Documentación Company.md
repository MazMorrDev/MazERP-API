
Un **Company** en MazErp representa un **entorno de trabajo independiente** que contiene su propio conjunto de almacenes, productos, y usuarios colaborativos.

### **Analogía Simple:**
```
🏢 Company = Empresa Virtual o Proyecto Independiente
    ├── 👥 Personas (Clients con diferentes roles)
    ├── 🏬 Almacenes (Warehouses)
    ├── 📦 Productos (Products)
    └── 📊 Movimientos (Movements)
```

---

## 🔄 **Flujo de Trabajo MultiEntorno**

### **1. Creación de Companies**
- Cada cliente puede crear **múltiples Companies** (entornos)
- Ejemplo: Un usuario puede tener:
  - `Company "MiRestaurantePrincipal"`
  - `Company "MiCafetería"`
  - `Company "ProyectoNuevo"`

### **2. Trabajo Colaborativo por Company**

```csharp
// Tabla ClientCompanies - Control de acceso
ClientCompany {
    ClientId,      // 👤 Persona
    CompanyId,    // 🏢 Entorno de trabajo
    Role           // 🎭 Rol (Owner, Admin, Contributor, Viewer)
}
```

#### **Roles Disponibles:**
| Rol           | Permisos                                                          |
| ------------- | ----------------------------------------------------------------- |
| **Admin**     | Dueño total (crear, editar, eliminar, gestionar personas)         |
| **Member**    |  Casi todos los permisos excepto eliminar Company |
| **Inventory** | Puede hacer movimientos y modificar inventarios                   |
| **Sales**     | gestión de ventas (como un dependiente en un merolico)            |
| **Finance**   | Solo lectura (ver datos, reportes)                                |

---

## 🏗️ **Estructura Jerárquica**

```
CLIENT (Usuario)
│
├── 🔗 CLIENT_Company (Acceso a entorno)
│   │
│   └── 🏢 Company (Entorno)
│       │
│       ├── 🏬 WAREHOUSE (Almacén 1)
│       │   ├── 📦 INVENTORY (Producto A + Stock)
│       │   ├── 📦 INVENTORY (Producto B + Stock)
│       │   └── ...
│       │
│       ├── 🏬 WAREHOUSE (Almacén 2)
│       │   ├── 📦 INVENTORY (Producto A + Stock)
│       │   └── ...
│       │
│       ├── 👥 CLIENT_Company (Persona 2 - Finance)
│       ├── 👥 CLIENT_Company (Persona 3 - Sales)
│       └── ...
│
└── 🔗 CLIENT_Company (Otro entorno diferente)
    └── 🏢 Company (Segundo proyecto)
```

---

## 💼 **Casos de Uso Prácticos**

### **Caso 1: Restaurante con Múltiples Sucursales**

```text
🏢 Company: "RestauranteElBuenSabor"
├── 👥 Team: 
│   ├️ → Juan (Admin)
│   ├️ → María (Member - Gerente)
│   └️ → Carlos (Sales - Encargado)
├── 🏬 Almacenes:
│   ├️ → "Almacén Central"
│   ├️ → "Sucursal Norte"
│   └️ → "Sucursal Sur"
└── 📦 Productos compartidos entre todos los almacenes
```

### **Caso 2: Empresa con Proyectos Independientes**
```
👤 Usuario: Ana
│
├── 🏢 Company: "ConsultoríaProyectoA"
│   ├️ → 🏬 Almacén "OficinaPrincipal"
│   └️ → 👥 Colaborador: Luis (Sales)
│
├── 🏢 Company: "VentasOnline"  
│   ├️ → 🏬 Almacén "BodegaDigital"
│   └️ → 👥 Colaborador: Pedro (Inventory)
│
└── 🏢 Company: "Personal"
    └️ → 🏬 Almacén "Casa"
```

---

## 🔐 **Reglas de Seguridad y Aislamiento**

### **Aislamiento Total entre Companies:**
- ❌ **Usuario A** en Company X **NO PUEDE** ver datos del Company Y
- ✅ **Datos completamente separados** entre diferentes Companies
- 🔒 **Cada Company es un sandbox** independiente

### **Acceso a Almacenes:**
- 👥 **Varios usuarios** pueden acceder a **los mismos almacenes** dentro de un Company
- 🏬 **Los almacenes son compartidos** entre todos los miembros del Company
- 👀 **El nivel de acceso** lo determina el **rol** en ClientCompany

---

## 📈 **Flujo de Operaciones Diarias**

### **1. Movimientos de Inventario:**
```csharp
Movement {
    ClientId,      // 👤 Quién hizo el movimiento
    WarehouseId,   // 🏬 En qué almacén
    ProductId,     // 📦 Qué producto
    MovementType,  // 📊 Entrada/Salida/Ajuste
    Quantity,      // 🔢 Cantidad
    MovementDate   // 📅 Cuándo
}
```

### **2. Colaboración en Tiempo Real:**
- **Múltiples personas** pueden trabajar en **el mismo almacén** simultáneamente
- **Sistema de roles** previene conflictos y define responsabilidades
- **Historial completo** de quién hizo cada movimiento

---

## 🚀 **Ventajas del Sistema MultiCompany**

### **Para Usuarios Individuales:**
- ✅ **Separación clara** entre proyectos personales y profesionales
- ✅ **Pruebas** en Companies de desarrollo sin afectar producción
- ✅ **Organización** por clientes, proyectos, o temporadas

### **Para Equipos:**
- ✅ **Onboarding fácil** - agregar personas solo a los Companies necesarios
- ✅ **Seguridad granular** - diferentes permisos por proyecto
- ✅ **Auditoría completa** - saber quién hizo qué en cada entorno

---

## 🎯 **Resumen Ejecutivo**

- **🏢 Company = Entorno independiente** con datos aislados
- **👥 Multi-usuario** dentro de cada Company con sistema de roles
- **🏬 Almacenes compartidos** entre miembros del mismo Company
- **🔒 Aislamiento total** entre diferentes Companies
- **📊 Auditoría completa** de todas las operaciones

Este sistema permite **escalabilidad horizontal** (múltiples proyectos) y **vertical** (múltiples usuarios por proyecto) de manera organizada y segura.