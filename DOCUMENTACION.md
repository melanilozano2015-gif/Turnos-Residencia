# ResidenciaApp SENAF - Documentación Completa

## Índice
1. [Descripción del Proyecto](#1-descripción-del-proyecto)
2. [Stack Tecnológico](#2-stack-tecnológico)
3. [Arquitectura del Sistema](#3-arquitectura-del-sistema)
4. [Base de Datos](#4-base-de-datos)
5. [Backend - C# / .NET](#5-backend--c-net)
6. [Frontend - Razor Views / CSS](#6-frontend--razor-views--css)
7. [Autenticación y Roles](#7-autenticación-y-roles)
8. [Cómo Ejecutar](#8-cómo-ejecutar)
9. [Credenciales](#9-credenciales)

---

## 1. Descripción del Proyecto

**ResidenciaApp SENAF** es un sistema de gestión para una residencia de niños y adolescentes (NNyA) del Servicio Nacional de Adminstración Fiscal (SENAF). Permite registrar NNyA, gestionar turnos con profesionales, y llevar un historial de cada turno.

### Funcionalidades principales
- **Dashboard**: Vista general con estadísticas y turnos próximos
- **Niños y Adolescentes**: Alta, baja, modificación y consulta de NNyA
- **Turnos**: Gestión completa de turnos con filtros por estado, tipo y fechas
- **Historial**: Registro de cambios de estado en cada turno
- **Login**: Sistema de autenticación con roles

---

## 2. Stack Tecnológico

| Capa | Tecnología |
|------|------------|
| **Lenguaje** | C# 12 |
| **Framework** | .NET 10.0 |
| **ORM** | Entity Framework Core 10.0 |
| **Base de datos** | SQL Server LocalDB |
| **Autenticación** | ASP.NET Core Identity |
| **Frontend** | Razor Views + Bootstrap 5 |
| **CSS** | Apple Design System (custom) |
| **Tipografía** | Inter (Google Fonts) |

---

## 3. Arquitectura del Sistema

```
ResidenciaApp/
├── Controllers/          # Lógica de negocio (Backend)
│   ├── AccountController.cs      # Login/Logout
│   ├── DashboardController.cs    # Panel principal
│   ├── NinosController.cs        # CRUD de NNyA
│   └── TurnosController.cs       # CRUD de Turnos
│
├── Models/               # Modelos de datos
│   ├── NinoAdolescente.cs        # Modelo principal NNyA
│   ├── Turno.cs                  # Modelo de turnos
│   ├── Profesional.cs            # Profesionales (matrícula, especialidad)
│   ├── Institucion.cs            # Instituciones
│   ├── EstadoTurno.cs            # Estados: Pendiente, Confirmado, Completado, Cancelado
│   ├── HistorialTurno.cs         # Historial de cambios de estado
│   ├── Especialidad.cs           # Especialidades médicas
│   └── Usuario.cs                # Usuarios del sistema
│
├── Data/                 # Contexto y datos iniciales
│   ├── ApplicationDbContext.cs   # DbContext de EF Core
│   └── SeedData.cs               # Datos de prueba iniciales
│
├── Views/                # Vistas Razor (Frontend)
│   ├── Account/Login.cshtml      # Pantalla de login
│   ├── Dashboard/Index.cshtml    # Panel principal
│   ├── Ninos/                    # Vistas de NNyA
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   ├── Delete.cshtml
│   │   └── Turnos.cshtml
│   ├── Turnos/                   # Vistas de turnos
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   └── Delete.cshtml
│   └── Shared/_Layout.cshtml     # Layout principal
│
├── wwwroot/css/site.css   # Estilos Apple Design System
├── Program.cs             # Punto de entrada de la aplicación
└── Migrations/            # Migraciones de EF Core
```

---

## 4. Base de Datos

### Diagrama ER (Modelo Corregido por Profesor)

```
┌─────────────────────┐     ┌─────────────────────┐
│  NINOS_ADOLESCENTES │     │     ESPECIALIDAD    │
├─────────────────────┤     ├─────────────────────┤
│ IdNinoAdolescente PK│     │ IdEspecialidad PK   │
│ Dni (unique)        │     │ Nombre              │
│ Nombre              │     │ Descripcion         │
│ Apellido            │     └──────────┬──────────┘
│ FechaNacimiento     │                │
│ Genero              │                │ 1
│ Nacionalidad        │                │
│ LugarNacimiento     │     ┌──────────┴──────────┐
│ NumeroExpediente    │     │     PROFESIONAL     │
│ EstadoActual        │     ├─────────────────────┤
│ Escolaridad         │     │ IdProfesional PK    │
│ Telefono            │     │ Nombre              │
│ Email               │     │ Apellido            │
│ ObraSocial          │     │ Matricula           │
│ Domicilio           │     │ IdEspecialidad FK ──┘
└─────────┬───────────┘     └─────────────────────┘
          │
          │ 1
          │
┌─────────┴───────────┐     ┌─────────────────────┐
│       TURNO         │     │   INSTITUCION       │
├─────────────────────┤     ├─────────────────────┤
│ IdTurno PK          │     │ IdInstitucion PK    │
│ IdNinoAdolescente FK│     │ Nombre              │
│ NombreProfesional   │     │ Calle               │
│ NombreInstitucion   │     │ Numero              │
│ IdEstadoTurno FK ───┤     │ Telefono            │
│ IdUsuario FK        │     │ Email               │
│ Fecha               │     └─────────────────────┘
│ Hora                │
│ TipoTurno           │     ┌─────────────────────┐
│ Motivo              │     │   ESTADO_TURNO      │
│ Resultado           │     ├─────────────────────┤
│ Observaciones       │     │ IdEstadoTurno PK    │
└─────────┬───────────┘     │ Nombre              │
          │                 │ Descripcion         │
          │ 1               └─────────────────────┘
          │
┌─────────┴───────────┐     ┌─────────────────────┐
│  HISTORIAL_TURNO    │     │      USUARIO        │
├─────────────────────┤     ├─────────────────────┤
│ IdHistorial PK      │     │ IdUsuario PK        │
│ IdTurno FK          │     │ Nombre              │
│ IdUsuario FK        │     │ Apellido            │
│ IdEstadoTurno FK    │     │ Email               │
│ FechaHora           │     │ Contrasena          │
│ Accion              │     └─────────────────────┘
│ Observacion         │
└─────────────────────┘
```

### Tablas principales

#### NinosAdolescentes
Almacena los datos de los niños y adolescentes de la residencia.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| IdNinoAdolescente | int (PK) | Identificador único |
| Dni | nvarchar(20) | DNI (único) |
| Nombre | nvarchar(100) | Nombre del NNyA |
| Apellido | nvarchar(100) | Apellido del NNyA |
| FechaNacimiento | datetime2 | Fecha de nacimiento |
| Genero | nvarchar(20) | Género |
| Nacionalidad | nvarchar(100) | Nacionalidad (default: Argentina) |
| LugarNacimiento | nvarchar(200) | Lugar de nacimiento |
| NumeroExpediente | nvarchar(50) | Número de expediente |
| EstadoActual | nvarchar(50) | Estado: En residencia, Internado, Egresado, En tratamiento |
| Escolaridad | nvarchar(200) | Nivel de escolaridad |
| Telefono | nvarchar(30) | Teléfono de contacto |
| Email | nvarchar(100) | Email de contacto |
| ObraSocial | nvarchar(100) | Obra social |
| Domicilio | nvarchar(500) | Domicilio completo |

#### Turno
Registra cada turno de un NNyA con un profesional.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| IdTurno | int (PK) | Identificador único |
| IdNinoAdolescente | int (FK) | NNyA asociado |
| NombreProfesional | nvarchar(200) | Nombre del profesional (texto libre) |
| NombreInstitucion | nvarchar(200) | Nombre de la institución (texto libre) |
| IdEstadoTurno | int (FK) | Estado del turno |
| IdUsuario | int (FK) | Usuario que registró |
| Fecha | datetime2 | Fecha del turno |
| Hora | datetime2 | Hora del turno |
| TipoTurno | nvarchar(50) | Tipo: Médico, Psicológico, Odontológico, Judicial, Educativo, Trabajo Social, Otro |
| Motivo | nvarchar(500) | Motivo de la consulta |
| Resultado | nvarchar(500) | Resultado de la consulta |
| Observaciones | nvarchar(1000) | Observaciones adicionales |

#### EstadosTurno
Estados posibles de un turno:
1. **Pendiente** - Turno pendiente de atención
2. **Confirmado** - Turno confirmado por el profesional
3. **Completado** - Turno realizado exitosamente
4. **Cancelado** - Turno cancelado

#### Especialidades
Especialidades de los profesionales:
1. Salud Mental (Psicología y psiquiatría)
2. Medicina General (Pediatría y clínica general)
3. Terapias (Fonoaudiología y terapia ocupacional)
4. Nutrición (Dietética y nutrición)
5. Odontología (Salud bucal)

---

## 5. Backend - C# / .NET

### Program.cs - Punto de entrada
```csharp
// Configuración de servicios
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Ruta por defecto: Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
```

### AccountController.cs - Autenticación
- `GET /Account/Login`: Muestra formulario de login
- `POST /Account/Login`: Valida credenciales con `SignInManager`
- `POST /Account/logout`: Cierra sesión

### NinosController.cs - CRUD de NNyA
- `GET /Ninos`: Lista con búsqueda por nombre, apellido o DNI
- `GET /Ninos/Create`: Formulario de alta
- `POST /Ninos/Create`: Guarda nuevo NNyA
- `GET /Ninos/Edit/{id}`: Formulario de edición
- `POST /Ninos/Edit/{id}`: Actualiza NNyA
- `GET /Ninos/Details/{id}`: Detalle con turnos recientes
- `GET /Ninos/Delete/{id}`: Confirmación de eliminación
- `GET /Ninos/Turnos/{id}`: Turnos del NNyA

### TurnosController.cs - Gestión de turnos
- `GET /Turnos`: Lista con filtros (tab, tipo, búsqueda, estado, fechas)
- `GET /Turnos/Create`: Formulario de nuevo turno
- `POST /Turnos/Create`: Guarda turno + crea historial
- `GET /Turnos/Edit/{id}`: Formulario de edición
- `POST /Turnos/Edit/{id}`: Actualiza turno + registra cambio de estado en historial
- `GET /Turnos/Details/{id}`: Detalle con historial completo

### SeedData.cs - Datos iniciales
Crea automáticamente:
- Roles: Director, Educador, Dirección
- Usuario admin: `admin@arguelloinfancias.com` / `Admin123!`
- NNyA de prueba: Juan Pérez, Ana García
- Instituciones de prueba: Hospital General, Centro de Salud Mental
- Especialidades: 5 especialidades médicas
- Estados de turno: Pendiente, Confirmado, Completado, Cancelado

---

## 6. Frontend - Razor Views / CSS

### Apple Design System
El CSS está basado en el diseño de Apple con:

**Variables de color:**
```css
--apple-primary: #0066cc;        /* Azul acción */
--apple-surface-tile-1: #272729; /* Fondo oscuro */
--apple-body-on-dark: #ffffff;   /* Texto blanco */
```

**Tipografía:**
- Fuente: Inter (fallback: SF Pro)
- Tamaño base: 17px
- Letter-spacing: -0.374px (tight tracking)

**Componentes:**
- Botones pill (border-radius: 9999px)
- Cards con border-radius: 18px
- Active state: transform: scale(0.95)

### Layout principal
```
┌──────────────────────────────────────────┐
│  Navbar (fondo oscuro)                   │
│  [Logo] [Dashboard] [NNyA] [Turnos] [Salir] │
├──────────┬───────────────────────────────┤
│ Sidebar  │  Contenido principal          │
│ Dashboard│  (varía según la pantalla)    │
│ NNyA     │                               │
│ Turnos   │                               │
│ Nuevo NNyA│                              │
│ Nuevo Turno│                             │
└──────────┴───────────────────────────────┘
```

### Pantalla de Login
- Card oscura centrada en pantalla
- Título: "Argüello Infancias"
- Subtítulo: "Sistema de Gestión Residencia NNyA"
- Campos: Email, Contraseña
- Botón teal "Ingresar"

### Formulario Registrar NNyA
Secciones:
1. **Datos personales**: Nombre, Apellido, DNI, Fecha nacimiento, Género, Nacionalidad, Lugar nacimiento, N° Expediente
2. **Contacto y situación**: Estado actual, Escolaridad, Teléfono, Email, Obra social, Domicilio

### Pantalla de Turnos
- **Tabs**: Todos, Próximos, Pasados, Cancelados
- **Filtros**: Tipo (Médico, Psicológico, etc.), Búsqueda de texto, Estado, Profesional, Fechas
- **Tabla**: NNyA, Profesional, Institución, Fecha, Hora, Estado, Acciones
- **Badges de estado**: Colores según estado

---

## 7. Autenticación y Roles

### Roles del sistema
| Rol | Descripción |
|-----|-------------|
| Director | Acceso completo al sistema |
| Educador | Acceso a consulta y registro |
| Dirección | Acceso administrativo |

### Flujo de autenticación
1. Al entrar a cualquier página, si no está autenticado → redirige a `/Account/Login`
2. El usuario ingresa email y contraseña
3. `SignInManager` valida las credenciales contra ASP.NET Identity
4. Si son correctas → crea cookie de autenticación → redirige a Dashboard
5. Si son incorrectas → muestra error "Email o contraseña incorrectos"

### Configuración de contraseña
```csharp
options.Password.RequireDigit = true;      // Requiere número
options.Password.RequireLowercase = true;  // Requiere minúscula
options.Password.RequireUppercase = true;  // Requiere mayúscula
options.Password.RequireNonAlphanumeric = false;
options.Password.RequiredLength = 6;       // Mínimo 6 caracteres
```

---

## 8. Cómo Ejecutar

### Requisitos
- .NET 10.0 SDK
- SQL Server LocalDB (viene con Visual Studio)

### Pasos
```bash
# 1. Navegar al directorio del proyecto
cd "C:\Users\melan\OneDrive\Documentos\Default Project\ResidenciaApp"

# 2. Restaurar paquetes NuGet
dotnet restore

# 3. Crear/actualizar base de datos
dotnet ef database update

# 4. Ejecutar la aplicación
dotnet run --urls http://localhost:5166

# 5. Abrir en el navegador
# http://localhost:5166
```

### Comandos útiles
```bash
# Ver migraciones
dotnet ef migrations list

# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migración
dotnet ef database update

# Revertir última migración
dotnet ef migrations remove

# Eliminar y recrear base de datos
dotnet ef database drop --force
dotnet ef database update
```

---

## 9. Credenciales

### Usuario Admin
- **Email**: `admin@arguelloinfancias.com`
- **Contraseña**: `Admin123!`
- **Rol**: Director

### Roles
| Rol | Permisos |
|-----|----------|
| Director | Acceso completo |
| Educador | Consulta y registro |
| Dirección | Administrativo |

---

## Archivos importantes

| Archivo | Descripción |
|---------|-------------|
| `Program.cs` | Configuración y punto de entrada |
| `Data/ApplicationDbContext.cs` | Contexto de Entity Framework |
| `Data/SeedData.cs` | Datos iniciales y semillas |
| `Models/*.cs` | Modelos de datos |
| `Controllers/*.cs` | Lógica de negocio |
| `Views/**/*.cshtml` | Vistas Razor |
| `wwwroot/css/site.css` | Estilos Apple Design |
| `appsettings.json` | Configuración (connection string) |
| `Migrations/` | Migraciones de EF Core |

---

*Documentación generada el 15/09/2026*
