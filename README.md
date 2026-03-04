🏪 Sistema de Gestión de Ferretería JL

Sistema completo de gestión comercial para ferretería desarrollado con ASP.NET Core 8, compuesto por:

🔹 API REST (FerreteriaAPI)

🔹 Aplicación Web MVC (FerreteriaWeb)

🔹 Autenticación con Identity

🔹 Control de stock

🔹 Gestión de ventas

🔹 Dashboard y reportes

🔹 Exportación a Excel

🏗️ Arquitectura del Proyecto
FERRETERIAAPP
│
├── FerreteriaAPI      → Web API (Backend principal)
└── FerreteriaWeb      → Aplicación Web MVC (Frontend + Auth)
🔹 1️⃣ FerreteriaAPI (Backend)

API REST construida con:

ASP.NET Core 8

Entity Framework Core

MySQL (Pomelo)

ClosedXML (Exportación Excel)

Swagger

📦 Funcionalidades API
Productos

CRUD completo

Control de stock

Stock mínimo

Detección de productos con bajo stock

Ventas

Registro de venta con múltiples productos

Descuento automático de stock

Cálculo automático de subtotal y total

Snapshot del nombre del producto (aunque sea eliminado)

Reportes

Reporte por rango de fechas

Resumen diario

Resumen mensual

Dashboard general

Exportación mensual a Excel

📡 Endpoints principales
🛒 Ventas
POST   /api/Ventas
GET    /api/Ventas
GET    /api/Ventas/{id}
GET    /api/Ventas/reporte
GET    /api/Ventas/resumen-diario
GET    /api/Ventas/resumen-mensual
GET    /api/Ventas/dashboard
GET    /api/Ventas/exportar?Mes=MM&Año=YYYY
📦 Productos
GET    /api/Productos
POST   /api/Productos
PUT    /api/Productos/{id}
DELETE /api/Productos/{id}
🔹 2️⃣ FerreteriaWeb (Frontend MVC)

Aplicación MVC conectada a la API.

Incluye:

Login de usuarios

Gestión de productos

Registro de ventas

Visualización de boleta

Layout con Bootstrap

Validaciones con jQuery

Base de datos propia para usuarios:

SQLite (ferreteriaUsers.db)

ASP.NET Core Identity

🗃️ Base de Datos
📌 API

Motor: MySQL

ORM: Entity Framework Core

Migraciones incluidas

📌 Web

Motor: SQLite

Uso exclusivo para autenticación (Identity)

⚙️ Instalación
🔹 Requisitos

.NET 8 SDK

MySQL Server

Visual Studio 2022 o VS Code

🔹 1️⃣ Configurar Base de Datos API

Editar FerreteriaAPI/appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=FerreteriaDB;user=root;password=TU_PASSWORD;"
}

Ejecutar migraciones:

dotnet ef database update
🔹 2️⃣ Ejecutar API

Desde la carpeta FerreteriaAPI:

dotnet run

Swagger disponible en:

https://localhost:xxxx/swagger
🔹 3️⃣ Ejecutar Web

Desde la carpeta FerreteriaWeb:

dotnet run

Abrir en:

https://localhost:xxxx
📊 Funcionalidades Destacadas
🔒 Autenticación

Login con Identity

Base de datos separada para usuarios

📦 Control de Stock

No permite vender si no hay stock suficiente

Alerta de productos con stock bajo

🧾 Snapshot de Productos

Cuando se realiza una venta:

Se guarda el nombre del producto en DetalleVenta

Permite conservar historial aunque el producto sea eliminado

📈 Dashboard

Ventas del día

Ingresos del día

Ventas del mes

Ingresos del mes

Productos con bajo stock

📥 Exportación Excel

Genera archivo:

Ventas_MM_AAAA.xlsx

Incluye:

Fecha

N° Venta

Producto

Cantidad

Precio Unitario

Total línea

Total general

🧠 Buenas Prácticas Implementadas

Uso de DTOs

Uso de transacciones en ventas

Async/Await en toda la API

Separación de capas

Snapshot para integridad histórica

Validaciones de stock

Migraciones versionadas

Arquitectura limpia

🗂️ Estructura Técnica API
Controllers/
Data/
DTOs/
Models/
Migrations/
🗂️ Estructura Técnica Web
Controllers/
Models/
Views/
Data/
wwwroot/
🚀 Tecnologías Utilizadas

.NET 8

ASP.NET Core Web API

ASP.NET Core MVC

Entity Framework Core

MySQL

SQLite

ASP.NET Identity

Bootstrap 5

jQuery

ClosedXML

📌 Estado del Proyecto

✅ Sistema funcional
✅ API completamente operativa
✅ Frontend MVC conectado
✅ Exportación Excel
✅ Dashboard
✅ Autenticación

👨‍💻 Autor

Proyecto desarrollado como sistema de gestión comercial para Ferretería JL.
