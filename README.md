# Sistema de Gestión de Ferretería JL

Sistema completo de gestión comercial desarrollado con **ASP.NET Core 8**, compuesto por una API REST y una aplicación Web MVC conectadas entre sí.

---

## Arquitectura del Proyecto


FERRETERIAAPP
│
├── FerreteriaAPI → Web API (Backend principal)
└── FerreteriaWeb → Aplicación Web MVC (Frontend + Autenticación)


---

## Tecnologías Utilizadas

### Backend (FerreteriaAPI)

- ASP.NET Core 8
- Entity Framework Core
- MySQL (Pomelo)
- ClosedXML (Exportación Excel)
- Swagger

### Frontend (FerreteriaWeb)

- ASP.NET Core MVC
- Bootstrap 5
- jQuery
- ASP.NET Core Identity
- SQLite (Base de datos de usuarios)

---

## Funcionalidades Principales

### Gestión de Productos

- CRUD completo
- Control de stock
- Stock mínimo configurable
- Detección automática de productos con bajo stock

### Gestión de Ventas

- Registro de ventas con múltiples productos
- Validación de stock antes de vender
- Descuento automático de inventario
- Cálculo automático de subtotal y total
- Conservación del nombre del producto aunque sea eliminado

### Reportes y Dashboard

- Reporte por rango de fechas
- Resumen diario
- Resumen mensual
- Dashboard general
- Exportación mensual a Excel

---

## Endpoints API

### Ventas


POST /api/Ventas
GET /api/Ventas
GET /api/Ventas/{id}
GET /api/Ventas/reporte
GET /api/Ventas/resumen-diario
GET /api/Ventas/resumen-mensual
GET /api/Ventas/dashboard
GET /api/Ventas/exportar?Mes=MM&Año=YYYY


### Productos


GET /api/Productos
POST /api/Productos
PUT /api/Productos/{id}
DELETE /api/Productos/{id}


---

## Base de Datos

### API
- Motor: MySQL
- ORM: Entity Framework Core
- Migraciones incluidas

### Web
- Motor: SQLite
- Uso exclusivo para autenticación (Identity)

---

## Instalación

### Requisitos

- .NET 8 SDK
- MySQL Server
- Visual Studio 2022 o VS Code

---

### 1. Configurar conexión a la base de datos (API)

Editar `FerreteriaAPI/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=FerreteriaDB;user=root;password=TU_PASSWORD;"
}
2. Ejecutar migraciones

Desde la carpeta FerreteriaAPI:

dotnet ef database update
3. Ejecutar la API
dotnet run

Swagger disponible en:

https://localhost:xxxx/swagger
4. Ejecutar la aplicación Web

Desde la carpeta FerreteriaWeb:

dotnet run

Abrir en:

https://localhost:xxxx
Características Técnicas

Uso de DTOs para transferencia de datos

Uso de transacciones en registro de ventas

Programación asíncrona (async/await)

Separación de capas

Snapshot del nombre del producto para mantener historial

Validaciones de negocio en backend

Exportación a Excel

Genera archivo:

Ventas_MM_AAAA.xlsx

Incluye:

Fecha

Número de venta

Producto

Cantidad

Precio unitario

Total por línea

Total general del mes

Estado del Proyecto

Proyecto completamente funcional:

API operativa

Frontend conectado

Dashboard implementado

Exportación a Excel

Autenticación con Identity
