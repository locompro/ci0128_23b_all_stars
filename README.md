# ci0128_23b_all_stars - LoCoMPro - Team All-Stars

## Curso

|  |  |
|-------------|---------------------------------------------------|
| Universidad | Universidad de Costa Rica 			  			  			  |
| Sigla       | CI-0126 		        			        			  			  |
| Año	        | 2023                      			  			  			  |
| Semestre    | II                        			  			          |
| Profesores  | Dra. Alexandra Martínez, Dr. Allan Berrocal Rojas |

## Equipo

| Nombre                         | Carné  |
|--------------------------------|--------|
| Ariel Antonio Arévalo Alvarado | B50562 |
| A. Badilla Olivas              | B80874 |
| Gabriel Molina Bulgarelli      | C14826 |
| Brandon Alonso Mora Umaña      | C15179 |
| Joseph Stuart Valverde Kong    | C18100 |

## Problema

> Al comprar un producto en cualquier establecimiento, a menudo nos intriga saber si el precio que vamos a pagar es adecuado. Nos gustaría averiguar rápidamente si el mismo producto tiene un mejor precio en otro lugar que nos resulte igualmente conveniente visitar. Sin embargo, no hay maneras simples de saber el precio de un producto arbitrario en diferentes establecimientos, normalmente eso implica ir a visitar muchos establecimientos y comparar los precios del producto de interés. Por ende, es difícil tener esa información de manera rápida.
>
> _-Especificacion 'Localización y Consulta del Mejor Producto'_

## Estructura

<pre>
ci0128_23b_all_stars/
│   ci0128_23b_all_stars.sln  
│   README.md  
│  
├───<a href="./Design">Design</a>  
│   ├───<a href="./Design/Database">Database</a>  
│   │   └───<a href="./Design/Database/Sprint0">Sprint0</a>  
│   │       ├───<a href="./Design/Database/Sprint0/Avance1">Avance1</a>  
│   │       │       <a href="./Design/Database/Sprint0/Avance1/DiagramaER.jpg">DiagramaER.jpg</a>  
│   │       │  
│   │       ├───<a href="./Design/Database/Sprint0/Avance2">Avance2</a>  
│   │       │       <a href="./Design/Database/Sprint0/Avance2/Mockup_Bases_de_Datos.pdf">Mockup_Bases_de_Datos.pdf</a>  
│   │  
│   └───<a href="./Design/Mockups">Mockups</a>  
│       └───<a href="./Design/Mockups/Sprint0">Sprint0</a>  
│       │   ├───<a href="./Design/Mockups/Sprint0/Avance1">Avance1</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance1/busqueda.png">busqueda.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance1/busqueda_avanzada.png">busqueda_avanzada.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance1/busqueda_llena.png">busqueda_llena.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance1/ingresar.png">ingresar.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance1/menu_principal.png">menu_principal.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance1/perfil_usuario.png">perfil_usuario.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance1/registro_producto.png">registro_producto.png</a>  
│       │   │  
│       │   └───<a href="./Design/Mockups/Sprint0/Avance2">Avance2</a>   
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/menu_principal.png">menu_principal.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/menu_principal_sin_iniciar_sesion.png">menu_principal_sin_iniciar_sesion.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/crear_cuenta.png">crear_cuenta.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/iniciar_sesion.png">iniciar_sesion.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/busqueda_resultados_producto.png">busqueda_resultados_producto.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/busqueda_resultados_producto_sin_iniciar_sesion.png">busqueda_resultados_producto_sin_iniciar_sesion.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/mi_perfil_de_usuario.png">mi_perfil_de_usuario.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/un_perfil_de_usuario.png">un_perfil_de_usuario.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/agregar_contribucion.png">agregar_contribucion.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/agregar_producto.png">agregar_producto.png</a>  
│       │   │       <a href="./Design/Mockups/Sprint0/Avance2/agregar_tienda.png">agregar_tienda.png</a>  
│       │
│       └───<a href="./Design/Mockups/Sprint1">Sprint1</a>
│           └───<a href="./Design/Mockups/Sprint1/Avance2">Avance2</a>   
│           │       <a href="./Design/Mockups/Sprint1/Avance2/menu_principal.png">menu_principal.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/menu_principal_sin_iniciar_sesion.png">menu_principal_sin_iniciar_sesion.png</a> 
│           │       <a href="./Design/Mockups/Sprint1/Avance2/menu_principal_avanzada.png">menu_principal_avanzada.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/crear_cuenta.png">crear_cuenta.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/iniciar_sesion.png">iniciar_sesion.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/busqueda_resultados_producto.png">busqueda_resultados_producto.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/busqueda_resultados_producto_sin_iniciar_sesion.png">busqueda_resultados_producto_sin_iniciar_sesion.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/busqueda_avanzada.png">busqueda_avanzada.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/agregar_contribucion.png">agregar_contribucion.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/agregar_contribucion_autocompletado.png">agregar_contribucion_autocompletado.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/agregar_producto.png">agregar_producto.png</a>
│           │       <a href="./Design/Mockups/Sprint1/Avance2/agregar_tienda.png">agregar_tienda.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/resultados_registros.png">resultados_registros.png</a>  
└───Locompro  
    └───(Proyecto ASP.NET)
</pre>

## Manual de usuario

### 1. Información de acceso

#### Registrarse
Para registrarse, la persona usuaria puede desde cualquier lugar de la aplicación, se puede presionar el botón iniciar sesión. Al presionar sobre el botón se redirige a una página con un formulario para iniciar la sesión, al final de ese formulario encontrará un enlace "Registrese" para registrarse, acompañado de la frase "¿No tiene una cuenta aún?"

<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/e5d74ddf-4ca2-4241-80d0-a7e8ae6abd95" alt="FormularioDeInicioDeSesión" width="500"/>
</center>

Al presionar el enlace "Registrese" la persona usuaria será redirigida a la página del formulario de registro, donde podrá ingresar sus datos. Luego de ingresados los datos, presiona el botón "crear un cuenta" y esto realizará el registro.

<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/a40d6f07-c63d-4a00-a548-e0158b76c1ce" alt="FormularioDeRegistro" width="500"/>
</center>

Luego del registro, la persona usuaria habrá entrado automáticamente a su nueva cuenta sin necesidad de ir al formulario de inicio de sesión y será redirigida a la página de inicio.

#### Iniciar sesión

Para iniciar sesión, desde cualquier lugar de la aplicación, se puede presionar el botón iniciar sesión. 

<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/de2454b5-7ec7-4f40-9deb-2fb9817126d3" alt="Página principal con botón de Iniciar Sesión" width="800"/>
</center>

Al presionar sobre el botón se redirige a una página con un formulario para iniciar la sesión, allí la persona usuaria debe ingresar su nombre de usuario y su contraseña tal y como fueron puestas durante el registro con el fin de iniciar su sesión.

<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/e5d74ddf-4ca2-4241-80d0-a7e8ae6abd95" alt="Formulario de inicio de sesión" width="500"/>
</center>

#### Cerrar sesión
Luego de haber iniciado sesión, para cerrar sesión se debe presionar el botón "Cerrar Sesión" disponible desde cualquier parte de la aplicación. Al cerrar sesión la persona usuaria será redirigida a la página de inicio.
<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/7b1ebf82-272f-438a-96f4-766e2def90d8" alt="Página principal con botón de Cerrar Sesión" width="800"/>
</center>

### 2. Funcionalidad de la aplicación


#### Búsqueda simple y Búsqueda avanzada
En la página principal y en la página de resultados se pueden realizar tanto las busquedas simples como las busquedas avanzadas.

**Vista en la página principal**
<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/de2454b5-7ec7-4f40-9deb-2fb9817126d3" alt="Página Principal con botón de búsqueda avanzada" width="800"/>
</center>

La página principal es lo primero que ve el usuario a la hora de ingresar a la aplicación. En ella se encuentran diversas opciones para realizar busquedas. Primero se puede observar la barra de busqueda donde se ingresa en nombre del producto que el usuario desea buscar. Posteriormente, dentro de este se encuentra el botón para realizar la busqueda. Cuando solo se encuentra el nombre del producto, la busqueda será realizada solamente en terminos de esta. Inferior a estos se encuentra el botón de búsqueda avanzada, el cual al presionarlo despliega un menú de opciones para realizar búsquedas más especificas.

**Vista de la página principal con menú de busqueda avanzada**
<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/0c679361-c4e8-466e-be83-05333d7abd31" alt="PaginaDeResultadosBusquedaAvanzadaDespuesDelClick" width="800"/>
</center>
De presionar el boton de busqueda avanzada, se despliega el menu de busqueda avanzada. Dentro de este se pueden realizar busquedas por nombre de producto, nombre de tienda, provincia, canton, precio minimo y precio maximo. Al presionar el boton de buscar, se realizara la busqueda con los parametros ingresados. Siendo estos, en esta version y entrega, por provincia y canton, por marca y modelo. Un usuario puede elegir solamente una de estos parametros de busqueda o por combinación de estos. Por ejemplo uno puede buscar solamente por marca y sin ingresar un nombre de producto en la barra de busqueda, con lo cual solo se realizará una busqueda por el parametro de marca. 

**Vista en la página de resultados**
<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/5c7fda27-a623-4b21-b0bf-5f6da8931a9b" alt="PáginaDeResultados" width="800"/>
</center>

Dentro de la página de resultados, al igual que en la página principal, se encuentra la barra de busqueda y el botón de busqueda avanzada. Las busquedas correspondientes se pueden realizar de la misma manera que en la página principal.

**Vista en la página de resultados con menú de busqueda avanzada**
<center>
    <img src="https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/2b935d19-844a-4c9f-97e2-2fa1cc7bccce" alt="PaginaDeResultadosBusquedaAvanzadaDespuesDelClick" width="800"/>
</center>
La busqueda avanzada en la página de resultados se realiza de la misma manera que en la página principal.


##### Buscar por provincia y cantón
Para buscar los aportes hechos en una provincia y cantón especificos, se puede seleccionar la provincia o el cantón en el menú desplegable de nombre "Provincia" y "Cantón" en el menú de busqueda avanzada, luego presione el botón de buscar (la lupa azul en la barra de búsqueda). Se pueden agregar más condiciones para la búsqueda simultaneamente.


##### Buscar por marca
Para buscar los aportes hechos a productos de una marca especifica, se debe ingresar el nombre de la marca a consultar en el campo de texto de nombre "Marca" en el menú de busqueda avanzada, luego presione el botón de buscar (la lupa azul en la barra de búsqueda). Se pueden agregar más condiciones para la búsqueda simultaneamente.'

##### Buscar por modelo
De la misma manera que la búsqueda por marca, para buscar los aportes hechos a productos de un modelo especifico, se debe ingresar el nombre del modelo a consultar en el campo de texto de nombre "Modelo" en el menú de busqueda avanzada, luego presione el botón de buscar. Es posible agregar más filtros para la búsqueda simultaneamente.


#### Hacer un aporte

## Manual técnico

### Requerimientos de instalación o ejecución

1. **Plataforma o Framework:** .NET 6.0
   - El proyecto está basado en el SDK `Microsoft.NET.Sdk.Web` y tiene como objetivo `net6.0`.
  
2. **Paquetes o Bibliotecas Necesarias:**  
   - Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore: Versión 6.0.0
   - Microsoft.AspNetCore.Identity.EntityFrameworkCore: Versión 6.0.21
   - Microsoft.AspNetCore.Identity.UI: Versión 6.0.21
   - Microsoft.EntityFrameworkCore.Design: Versión 6.0.22
   - Microsoft.EntityFrameworkCore.Proxies: Versión 6.0.22
   - Microsoft.EntityFrameworkCore.Sqlite: Versión 6.0.21
   - Microsoft.EntityFrameworkCore.SqlServer: Versión 6.0.22
   - Microsoft.EntityFrameworkCore.Tools: Versión 6.0.21
   - Microsoft.jQuery.Unobtrusive.Validation: Versión 3.2.11
   - Microsoft.VisualStudio.Web.CodeGeneration.Design: Versión 6.0.16
   - NUnit.Analyzers: Versión 3.6.1
   - NUnit.ConsoleRunner: Versión 3.16.3
   - NUnit.Engine: Versión 3.16.3
   - NUnit.Extension.NUnitProjectLoader: Versión 3.7.1
   - System.Device.Location.Portable: Versión 1.0.0
  

Para instalarlos puede hacer uso del instalador de paquetes Nuget o por medio de la terminal estando dentro de directorio `Source/Locompro`, usando el siguiente comando.
```
dotnet add package <nombre del paquete> --version <número de version>
```
Ejemplo:
```
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 6.0.0
```
### Preparación de la base de datos

Para preparar la base de datos primero deben hacerse las migraciones. Estas ya estan prepardas dentro del proyecto en la carpeta `/Locompro/Migrations.`
Desde la terminal y estando dentro de directorio `Source/Locompro`.
```
dotnet ef database update
```
o desde la consola del package manager en visual studio:
```
Update-Database
```
Luego de haber hecho esto, el esquema de la base de datos estará listo en la base de datos especificada. Finalmente, para agregar los datos estáticos de la aplicación, se debe ejecutar el script de sql static.sql en la carpeta `/Locompro/Resources.`

### Manual de instalación o ejecución del sistema

Para ejecutar el proyecto desde el IDE Visual Studio Community 2022 o Jetbrains Rider puede presionar el botón de `Run` o `Ejecutar`. También, por medio de la terminal estando dentro de directorio `Source/Locompro`, usando el siguiente comando.
```
dotnet run
```
Según los parametros actuales de configuración del proyecto, este se ejecutará y estará escuchando en el puerto 7249

### Manual de ejecución de pruebas 

El proyecto de pruebas está en la solución principal. Se debe ingresar en el menú superior a Test > Run All Tests. También, por medio de la terminal estando dentro de directorio `Source/Locompro.Tests`


### Generación de la documentación.

Usar el `Doxyfile` que se encuentra en  `Source/Locompro`, abrirlo con doxygenwizard y ejecutarlo. Esto genera documentación en una carpeta `Docs` en la raíz del repositorio.

### Manual de ejecución de los casos de prueba
1. **Paquetes o Bibliotecas Necesarias para Pruebas:**  
   - Microsoft.NET.Test.Sdk: Versión 17.7.2
   - MockDbSetExtensions: Versión 1.1.0
   - Moq: Versión 4.20.69
   - NUnit: Versión 3.13.3
   - NUnit.Analyzers: Versión 3.6.1
   - coverlet.collector: Versión 3.2.0
   - Microsoft.EntityFrameworkCore.InMemory: Versión 6.0.22
   - NUnit3TestAdapter --version 4.2.0

   Para correr las pruebas y generar un reporte, desde la raiz del repositorio puede utilizar los siguientes comando en terminal
   ```
   dotnet tool install --global JetBrains.dotCover.GlobalTool
   ```
   Luego
   ```
   dotnet dotcover test --dcReportType=HTML
   ```
   Esto generará en la raiz de la carpeta un archivo "dotCover.Output.html" que muestra la cobertura de las pruebas.
   

