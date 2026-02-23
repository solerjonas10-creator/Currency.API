# Currency.API

Version: .NET 10
Base de Datos: SQLite

Requisito: tener instalado el .NET SDK (Software Development Kit). (Ya viene con Visual Studio)

DESCRIPCION:
Esta es una API REST estructura CQRS desarrollada en .NET 10 para la gestión de usuarios (Users), direcciones (Addresses) y conversión de divisas (Currencies), 
protegida por seguridad basada en API Key. 


COMO DESPLEGAR:
1. Clona el repositorio o descomprime el archivo.
2. Abre una terminal en la carpeta raíz del proyecto.
3. Restaura las dependencias: dotnet restore
4. El proyecto usa Entity Framework con SQLite 
4.1 Instala la herramienta global: dotnet ef migrations add NombreDeLaMigracion
4.2 Crear Migracion de la BD: dotnet ef migrations add NombreDeLaMigracion
4.3 Aplicar la migracion: dotnet ef database update
5. Ejecutar: dotnet run
6. Abrir en el navegador http://localhost:5015/swagger (O la direccion que se muestra en el archivo launchSettings.json)
7. Para poder usar los EndPoints hacer clic en el boton [Authorize]
8. Pedira el ApiKey, que por defecto es "12345" para pruebas... (o revisar el archivo appsettings.json)
9. Click en [Authorize] y luego en [Close]
10. Listo ya es posible probar todos los EndPoints

IMPLEMENTADOS:
CRUD Users completo: Crear, Modificar, Listar y Eliminar.
CRUD Addresses completo: Crear, Modificar, Listar por UserId, y Eliminar.
CRUD Currencies completo: Crear, Listar, Modificar y eliminar. (Si bien no fue especificado se implementó Update y Delete para más dinamismo)
Conversion de monedas: Currencies
Validaciones via FluentValidation
Middleware de seguridad con APIKEY de prueba = 12345

Nota: 
Hash de password: se usa SHA256 + Base64. A considerar usar un algoritmo con salt para mayor seguridad...
