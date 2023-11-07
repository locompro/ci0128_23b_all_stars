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
Para registrarse, la persona usuaria puede desde cualquier lugar de la aplicación, presionar el botón iniciar sesión. Al presionar sobre el botón se redirige a una página con un formulario para iniciar la sesión, al final de ese formulario encontrará un enlace "Registrese" para registrarse, acompañado de la frase "¿No tiene una cuenta aún?"

![InicioDeSesionPagina](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/07ac034e-60ba-40e1-ac97-b7d58186c5be)


Al presionar el enlace "Registrese" la persona usuaria será redirigida a la página del formulario de registro, donde podrá ingresar sus datos. Luego de ingresados los datos, presiona el botón "crear un cuenta" y esto realizará el registro.

![Registro](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/817dd592-0bd8-4cc1-825d-4bc48ff1a6fe)

Luego del registro, la persona usuaria habrá entrado automáticamente a su nueva cuenta sin necesidad de ir al formulario de inicio de sesión y será redirigida a la página de inicio.

#### Iniciar sesión

Para iniciar sesión, desde cualquier lugar de la aplicación, se puede presionar el botón iniciar sesión. 

![InicioDeSesion](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/853ef2e6-ea6b-427a-b9f0-5d41fa792845)


Al presionar sobre el botón se redirige a una página con un formulario para iniciar la sesión, allí la persona usuaria debe ingresar su nombre de usuario y su contraseña tal y como fueron puestas durante el registro con el fin de iniciar su sesión.

![InicioDeSesionPagina](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/07ac034e-60ba-40e1-ac97-b7d58186c5be)

#### Cerrar sesión
Luego de haber iniciado sesión, para cerrar sesión se debe presionar el botón "Cerrar Sesión" disponible desde cualquier parte de la aplicación. Al cerrar sesión la persona usuaria será redirigida a la página de inicio.

![paginaprincipal](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/fd5558c5-2c63-433b-9bff-b8f8b4f81082)

### Ver mi perfil
La persona usuaria también puede ver su perfil e información relacionada como contribuciones y calificaciones. 
![Perfil](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/a994c352-9179-48d1-aba1-a0ab659e09c4)

### Modificar los datos personales
Si fuera necesario dentro de mi perfil, la persona usuaria puede modificar su contraseña, su correo y su dirección, haciendo uso de los botones bien identificados en la interfaz.

![PerfilModificar](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/b0da8442-7609-4a4c-a9e5-e589d3312b9f)

![PerfilCambiarContrasena](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/efb15109-80fc-433c-a095-acc7d27edd49)


### 2. Funcionalidad de búsqueda 


#### Búsqueda simple y Búsqueda avanzada
En la página principal y en la página de resultados se pueden realizar tanto las busquedas simples como las busquedas avanzadas.

**Vista en la página principal**

![paginaprincipal](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/fd5558c5-2c63-433b-9bff-b8f8b4f81082)

La página principal es lo primero que ve el usuario a la hora de ingresar a la aplicación. En ella se encuentran diversas opciones para realizar busquedas. Primero se puede observar la barra de busqueda donde se ingresa en nombre del producto que el usuario desea buscar. Posteriormente, dentro de este se encuentra el botón para realizar la busqueda. Cuando solo se encuentra el nombre del producto, la busqueda será realizada solamente en terminos de esta. Inferior a estos se encuentra el botón de búsqueda avanzada, el cual al presionarlo despliega un menú de opciones para realizar búsquedas más especificas.

**Vista de la página principal con menú de busqueda avanzada**

![paginaprincipalavanzada](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/a8b9063c-631f-4623-9340-a89db2715d2e)


De presionar el boton de busqueda avanzada, se despliega el menu de busqueda avanzada. Dentro de este se pueden realizar busquedas por nombre de producto, nombre de tienda, provincia, canton, precio minimo y precio maximo. Al presionar el boton de buscar, se realizara la busqueda con los parametros ingresados. Siendo esta entrega los que se pueden ver en la imagen adjunta. Un usuario puede elegir solamente una de estos parametros de busqueda o por combinación de estos. Por ejemplo uno puede buscar solamente por marca y sin ingresar un nombre de producto en la barra de busqueda, con lo cual solo se realizará una busqueda por el parametro de marca. 

**Vista en la página de resultados**

![resultadodebusqueda](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/bdb4f0e1-be58-4fc8-bea2-d11077bedefe)

Dentro de la página de resultados, al igual que en la página principal, se encuentra la barra de busqueda y el botón de busqueda avanzada. Las busquedas correspondientes se pueden realizar de la misma manera que en la página principal. Además se muestra la barra de filtros, con ellos la persona usuaria puede organizar y obtener información más especifica de resultados ya obtenidos. 

**Vista en la página de resultados con menú de busqueda avanzada**

La busqueda avanzada en la página de resultados se realiza de la misma manera que en la página principal. Es importante recordar que la búsqueda avanzada y los filtros son distintos, pues la primera hacer consultas a los servidores de lo compro, mientras que la segunda solo filtra sobre los resultados ya obtenidos por la persona usuaria.

![ResultadodeBusquedaAvanzada](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/d185d5c9-4ee1-48ec-af67-2435ab7c8f07)

### 3. Funcionalidad de Aportar

**Vista general de aportar**
![Aportes](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/a355c92c-c40c-49de-a430-aa16272b2423)

Para hacer un aporte la persona usuaria puede presionar el botón de **Aportar** desde cualquier lugar de la página. Aquí encontra un formulario para hacer su aporte. Los campos como producto y tienda se autocompletan conforme se escribe. Además se permite agregar imágenes

**Agregar una tienda nueva**

![AportesAgregarTienda](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/de7fbb88-e6b1-4a10-a246-841813a5d178)

Cuando la tienda no existe ya en la base de datos de lo compro, la persona usuaria puede usar el formulario de agregar tienda para introducirla al sistema y al finalizar, continuar justo donde estaba en la creación de su aporte.

**Agregar un producto nuevo**

![AportesAgregarProducto](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/3eb27e18-d7d3-4f36-915d-8b6478bec639)

Cuando el producto no existe ya en la base de datos de lo compro, la persona usuaria puede usar el formulario de agregar un producto nuevo al sistema y al finalizar, continuar justo donde estaba en la creación de su aporte.


### 4. Calificiones y Reportar

![resultadodebusquedaVotarYModerar](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/10b499de-0ff0-408d-87c8-a955ab87b762)


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


### Generación de la documentación.

Usar el `Doxyfile` que se encuentra en  `Source/Locompro`, abrirlo con doxygenwizard y ejecutarlo. Esto genera documentación en una carpeta `Docs` en la raíz del repositorio.

### Manual de ejecución de los casos de prueba
1. **Paquetes o Bibliotecas Necesarias para Pruebas:**  
- Microsoft.NET.Test.Sdk: Versión 17.7.1
- NUnit: Versión 3.13.3
- NUnit3TestAdapter: Versión 4.4.2
- NUnit.Analyzers: Versión 3.6.1
- coverlet.collector: Versión 3.2.0
- Selenium.Support: Versión 4.15.0
- Selenium.WebDriver: Versión 4.15.0
- MockDbSetExtensions: Versión 1.1.0
- Moq: Versión 4.20.69
- Microsoft.EntityFrameworkCore.InMemory: Versión 6.0.22
- NUnit3TestAdapter: Versión 4.2.0


Además, necesita instalar la herramienta dotcover para generar los reportes. Para instalar la herramienta, desde la raiz del repositorio, en la terminal.
     ```
     dotnet tool install --global JetBrains.dotCover.GlobalTool
     ```
  
#### Ejecutar pruebas unitarias

Para correr las pruebas unitarias y generar un reporte, desde la raiz del repositorio, puede utilizar el siguiente comando en terminal:
   ```
   dotnet dotcover test .\tests\unit_tests\Locompro.Tests\Locompro.Tests.csproj --dcReportType=HTML
   ```
Esto contruye la solución de test unitarios, ejecuta estas pruebas y genera en la raiz de la carpeta un archivo "dotCover.Output.html" que muestra la cobertura de las pruebas. Este reporte muestra con la cobertura de pruebas sobre le proyecto.

También puede ejecutar las pruebas funcionales sin generar el documento de reporte usando el comando en terminal:

 ```
 dotnet test .\tests\unit_tests\Locompro.Tests\Locompro.Tests.csproj
 ```

Esto contruye la solución de test funcionales, ejecuta estas pruebas y muestra los resultados en terminal de cuantas pruebas fallaron, fueron exitosas o fueron ignoradas.

#### Ejecutar pruebas funcionales

Antes que nada el ambiente de aplicación debe **esta lista**, eso requiere que los datos estáticos esten listos dentro de la base de datos. Ver la sección de **Manual Tecnico** para tener más información. Estando el ambiente listo, primero debe ejecutar la aplicación. Para ejecutar la aplicación puede ejecutar el siguiente comando en terminal:

```
dotnet build
dotnet run --environment <ambiente>
```

Ejemplo:

```
dotnet build
dotnet run --environment "Development"
```

Luego, desde la raiz del repositorio, se puede utilizar el siguiente comando en la terminal para ejecutar las pruebas:

```
dotnet dotcover test .\tests\unit_tests\Locompro.Tests\Locompro.Tests.csproj --dcReportType=HTML
```
    
Esto generará en la raiz de la carpeta un archivo "dotCover.Output.html" que muestra la cobertura de las pruebas. Este reporte muestra las pruebas realizadas.

También puede ejecutar las pruebas funcionales sin generar el documento de reporte usando el comando en terminal:
 ```
 dotnet test .\tests\unit_tests\Locompro.Tests\Locompro.Tests.csproj
 ```
Esto contruye la solución de test funcionales, ejecuta estas pruebas y muestra los resultados en terminal de cuantas pruebas fallaron, fueron exitosas o fueron ignoradas.



   

