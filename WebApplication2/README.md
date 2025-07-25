# Sistema de Gestión de Usuarios y Roles

Este es un proyecto ASP.NET Core MVC que implementa un sistema de gestión de usuarios y roles con autenticación y autorización.

## Características

- Gestión completa de usuarios (CRUD)
- Gestión de roles (CRUD)
- Sistema de autenticación basado en cookies
- Control de acceso basado en roles
- Encriptación segura de contraseñas usando BCrypt
- Interfaz de usuario responsive usando Bootstrap

## Requisitos Previos

- .NET 7.0 SDK o superior
- SQL Server
- Visual Studio 2022 o Visual Studio Code

## Configuración de la Base de Datos

La aplicación utiliza SQL Server como base de datos. La cadena de conexión se encuentra en `appsettings.json`:

```json
"ConnectionStrings": {
    "Conn": "Data Source=M12-CI;Initial Catalog=pruebatecnica;Integrated Security=True;TrustServerCertificate=True"
}
```

## Estructura del Proyecto

- **Controllers/**
  - `AccountController.cs` - Maneja la autenticación
  - `HomeController.cs` - Controlador principal
  - `RolesController.cs` - Gestión de roles
  - `UsersController.cs` - Gestión de usuarios

- **Models/**
  - `User.cs` - Modelo de usuario
  - `Role.cs` - Modelo de rol
  - `PruebatecnicaContext.cs` - Contexto de Entity Framework

- **Views/**
  - `Account/` - Vistas de autenticación
  - `Users/` - Vistas de gestión de usuarios
  - `Roles/` - Vistas de gestión de roles

## Características de Seguridad

1. **Autenticación**
   - Sistema de login basado en cookies
   - Tiempo de expiración de sesión: 1 hora
   - Redirección automática a login para recursos protegidos

2. **Autorización**
   - Control de acceso basado en roles
   - Rol "admin" requerido para gestión de usuarios y roles
   - Página de acceso denegado personalizada

3. **Seguridad de Contraseñas**
   - Encriptación usando BCrypt
   - Salt único por contraseña
   - Validación de longitud mínima

## Uso

1. **Inicio de Sesión**
   - Acceder a la página de login
   - Ingresar email y contraseña
   - Los usuarios admin tendrán acceso a todas las funcionalidades

2. **Gestión de Usuarios**
   - Crear, ver, editar y eliminar usuarios
   - Asignar roles a usuarios
   - Las contraseñas se encriptan automáticamente

3. **Gestión de Roles**
   - Crear, ver, editar y eliminar roles
   - Ver usuarios asignados a cada rol

## Paquetes NuGet Utilizados

- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- BCrypt.Net-Next
- Microsoft.VisualStudio.Web.CodeGeneration.Design

## Notas de Seguridad

- Las contraseñas se almacenan de forma segura usando BCrypt
- Las vistas de gestión están protegidas por rol
- Se implementan validaciones tanto en cliente como en servidor
- Se utiliza protección contra CSRF en todos los formularios

## Mantenimiento

Para mantener el sistema:
1. Actualizar regularmente los paquetes NuGet
2. Hacer backups regulares de la base de datos
3. Monitorear los logs de errores
4. Revisar periódicamente los usuarios con privilegios de administrador
