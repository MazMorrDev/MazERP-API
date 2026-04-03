# MazERP - Enterprise Resource Planning API

Este proyecto es una API RESTful desarrollada con .NET Core a modo práctica para demostrar mis conocimientos en desarrollo backend. El sistema implementa un ERP básico con funcionalidades esenciales para la gestión empresarial.

## 🛠️ Tecnologías y Herramientas Utilizadas

- **.NET Core 10.0** - Framework principal para el desarrollo de la API
- **Entity Framework Core** - ORM para el manejo de base de datos
- **PostgreSQL** - Sistema de gestión de bases de datos relacional
- **JWT (JSON Web Tokens)** - Autenticación y autorización segura
- **CORS** - Configuración de políticas de acceso desde diferentes orígenes
- **xUnit** - Framework para pruebas unitarias
- **Swagger/OpenAPI** - Documentación automática de la API
- **DotNetEnv** - Manejo de variables de entorno

## 🏗️ Arquitectura y Patrones de Diseño Implementados

### Arquitectura en Capas

- **Controllers**: Manejo de las peticiones HTTP y rutas
- **Services**: Lógica de negocio encapsulada
- **Repositories**: Abstracción del acceso a datos (implícito en servicios)
- **Models**: Entidades que representan las tablas de la base de datos
- **DTOs (Data Transfer Objects)**: Objetos para transferencia de datos entre capas
- **Mappers**: Conversión entre entidades y DTOs
- **Enums**: Tipos enumerados para valores fijados
- **Middleware**: Componentes personalizados para el pipeline de ASP.NET Core

### Principios SOLID y Buenas Prácticas

- **Separación de responsabilidades**: Cada capa tiene un propósito bien definido
- **Inyección de dependencias**: Gestión centralizada de dependencias mediante el contenedor de DI
- **Principio de inversión de dependencias**: Las dependencias se abstraen mediante interfaces
- **DRY (Don't Repeat Yourself)**: Reutilización de código mediante servicios y mappers
- **Validación de datos**: Uso de atributos de validación en DTOs

## 🔑 Características y Funcionalidades Implementadas

### Autenticación y Autorización

- Registro y login de usuarios con encriptación de contraseñas
- Generación y validación de tokens JWT
- Middleware personalizado para autorización basada en roles
- Protección de endpoints mediante atributos `[Authorize]`

### Gestión de Empresas

- CRUD completo de empresas
- Asociación de usuarios a empresas con diferentes roles
- Consultas de empresas por usuario y viceversa

### Gestión de Inventario

- Control de productos, almacenes y existencias
- Movimientos de inventario (entradas y salidas)
- Asignación de productos a puntos de venta
- Control de lotes y trazabilidad básica

### Gestión de Ventas y Compras

- Registro de ventas con diferentes tipos y métodos de pago
- Gestión de compras a proveedores
- Devoluciones de productos con estados y acciones definidas
- Gastos operativos categorizados

### Otros Módulos

- Gestión de proveedores
- Control de puntos de venta
- Historial de movimientos financieros
- Reportes básicos de inventario y ventas

## 📊 Estructura del Proyecto

```text
MazERP-API/
├── API/
│   ├── Controllers/          # Endpoints RESTful
│   ├── Services/             # Lógica de negocio
│   │   ├── Implementation/   # Implementaciones de servicios
│   │   └── Interfaces/       # Contratos de servicios
│   ├── Models/               # Entidades de base de datos
│   ├── DTOs/                 # Objetos de transferencia de datos
│   ├── Utils/                # Utilidades y helpers
│   │   ├── Mappers/          # Mapeo entre entidades y DTOs
│   │   └── PaginatedResult.cs# Resultado paginado genérico
│   ├── Config/               # Configuración de la aplicación
│   ├── Middleware/           # Middleware personalizado
│   ├── Enums/                # Enumeraciones
│   ├── Context/              # Contexto de Entity Framework
│   └── Migrations/           # Migraciones de base de datos
├── UnitTests/                # Proyecto de pruebas unitarias
├── MazERP Integration Tests/ # Tests de colección (Postman/Newman)
├── MazErpAPI.sln             # Solución de Visual Studio
└── README.md                 # Este archivo
```

## 🧪 Calidad del Código y Testing

- **Pruebas Unitarias**: Proyecto separado para testing de lógica de negocio
- **Cobertura de código**: Enfoque en probar casos críticos y edge cases
- **Tests de integración**: Colecciones de Postman para validar endpoints
- **Manejo de errores**: Respuestas consistentes y códigos de estado HTTP apropiados
- **Logging**: Implementación básica para seguimiento de eventos

## 🚀 Cómo Ejecutar el Proyecto

### Prerrequisitos

- .NET SDK 10.0 o superior
- PostgreSQL 12 o superior
- Git

### Pasos para Ejecutar

1. Clonar el repositorio:

   ```bash
   git clone <url-del-repositorio>
   cd MazERP-API
   ```

2. Configurar variables de entorno:

   ```bash
   cp .env.example .env
   # Editar .env con sus configuraciones
   ```

3. Aplicar migraciones de base de datos:

   ```bash
   dotnet ef database update
   ```

4. Ejecutar la aplicación:

   ```bash
   dotnet run --project API/MazErpAPI.csproj
   ```

5. Acceder a la documentación Swagger:

   <http://localhost:5148/swagger>

## 🎯 Objetivos de Aprendizaje Demostrados

Este proyecto demuestra competencia en:

1. **Desarrollo de APIs RESTful** con buenas prácticas de diseño
2. **Implementación de autenticación y autorización** segura
3. **Manejo de base de datos** con Entity Framework Core (migraciones, consultas, relaciones)
4. **Arquitectura de software** separando preocupaciones en capas distintas
5. **Inyección de dependencias** y uso de contenedores de servicio
6. **Mapeo de objetos** entre capas utilizando patrones de DTO y Mapper
7. **Validación de datos** tanto en entrada como en lógica de negocio
8. **Manejo de errores** y respuestas HTTP apropiadas
9. **Configuración de entorno** para diferentes ambientes (desarrollo/producción)
10. **Pruebas de software** tanto unitarias como de integración
11. **Documentación de API** mediante Swagger/OpenAPI
12. **Uso de enumeraciones** para valores de dominio específicos
13. **Implementación de middleware** personalizado para funcionalidades transversales
14. **Gestión de proyectos** con solución de Visual Studio y múltiples proyectos

## 📝 Notas Adicionales

Este proyecto fue desarrollado como ejercicio de aprendizaje y práctica para consolidar conocimientos en desarrollo backend con .NET Core. Aunque implementa funcionalidades completas de un ERP básico, está diseñado principalmente con fines educativos para demostrar:

- Comprensión de conceptos avanzados de programación orientada a objetos
- Aplicación de patrones de diseño arquitecturales
- Manejo seguro de autenticación y autorización
- Buenas prácticas en el desarrollo de APIs profesionales
- Organización y estructuración de proyectos de mediana complejidad

El código sigue convenciones de nomenclatura de C# y .NET, con comentarios explicativos en las secciones más complejas y una estructura clara que facilita el mantenimiento y la extensión futura.

---

*Desarrollado como parte de mi portafolio de habilidades en desarrollo backend*.
