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
|
|
├───<a href="./Source">Source</a>
|    |
│    └───<a href="./Source/Locompro">Código Fuente de Locompro</a>
|
└───<a href="./tests">tests</a>
    |
    └───<a href="./tests/unit_tests/Locompro.Tests">Unit Tests</a>
    |
    └───<a href="./tests/functional_tests/Locompro.FunctionalTests">Functional Tests</a>
</pre>
## Manual de usuario

### 1. Información de acceso

#### Registrarse
Para registrarse, la persona usuaria puede desde cualquier lugar de la aplicación, presionar el botón iniciar sesión. Al presionar sobre el botón se redirige a una página con un formulario para iniciar la sesión, al final de ese formulario encontrará un enlace "Registrese" para registrarse, acompañado de la frase "¿No tiene una cuenta aún?"

![InicioDeSesionPagina](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/07ac034e-60ba-40e1-ac97-b7d58186c5be)

Al presionar el enlace "Regístrese" la persona usuaria será redirigida a la página del formulario de registro, donde podrá ingresar sus datos. Luego de ingresados los datos, presiona el botón "crear un cuenta" y esto realizará el registro.

![Registro](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/817dd592-0bd8-4cc1-825d-4bc48ff1a6fe)

Luego del registro, la persona usuaria habrá entrado automáticamente a su nueva cuenta sin necesidad de ir al formulario de inicio de sesión y será redirigida a la página de inicio.

#### Iniciar sesión

Para iniciar sesión, desde cualquier lugar de la aplicación, se puede presionar el botón iniciar sesión. 

![InicioDeSesion](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/853ef2e6-ea6b-427a-b9f0-5d41fa792845)

Al presionar sobre el botón se redirige a una página con un formulario para iniciar la sesión, allí la persona usuaria debe ingresar su nombre de usuario y su contraseña tal y como fueron puestas durante el registro con el fin de iniciar su sesión.

![InicioDeSesionPagina](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/07ac034e-60ba-40e1-ac97-b7d58186c5be)

#### Cerrar sesión
Luego de haber iniciado sesión, para cerrar sesión se debe presionar el botón "Cerrar Sesión" disponible desde cualquier parte de la aplicación. Al cerrar sesión la persona usuaria será redirigida a la página de inicio.

![PaginaPrincipal](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/b7239879-0c9c-4542-a1f1-37e58fe28b7d)


### Ver mi perfil
La persona usuaria también puede ver su perfil e información relacionada como contribuciones y calificaciones. 
![Perfil](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/3a8018b1-d70a-4697-aa28-bfdf02050055)


### Modificar los datos personales
Si fuera necesario dentro de mi perfil, la persona usuaria puede modificar su contraseña, su correo y su dirección, haciendo uso de los botones bien identificados en la interfaz.

![PerfilModificar](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/d903679c-1733-4212-b4b8-432f380c8283)


![PerfilCambiarContrasena](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/8a74d75e-d857-48b1-a328-1461e2d1d7bf)


Al modificar los datos la persona usuaria verá notificaciones en su pantalla de perfil indicandoles el exito de la operación o mensajes de error significativos en los formularios de que salió mal.

### Acceder a mis contribuciones

Al presionar el botón de la esquina inferior izquierda se podrá acceder a una vista de contribuciones personal. Esta contiene la lista completa de contribuciones realizadas en la plataforma

![MisContribuciones](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/91059deb-9734-48b8-9b52-912f06d2107f)



### 2. Funcionalidad de Aportar

**Vista general de aportar**
![Aportar](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/f3b120de-bae2-4be8-b894-52c74aa8feb6)


Para hacer un aporte la persona usuaria puede presionar el botón de **Aportar** desde cualquier lugar de la página. Aquí encontra un formulario para hacer su aporte. Los campos como producto y tienda se autocompletan conforme se escribe. Además se permite agregar imágenes

**Agregar una tienda nueva**

![AportesAgregarTienda](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/2de14ab4-7dd5-4892-b6e5-a540567d97f9)


Cuando la tienda no existe ya en la base de datos de lo compro, la persona usuaria puede usar el formulario de agregar tienda para introducirla al sistema y al finalizar, continuar justo donde estaba en la creación de su aporte. Por defecto la tienda se creará con la posición actual de la persona usuaria, pero puede cambiarse usando el mapa y los seleccionadores de provincia y cantón. También se puede seleccionar la ubicación por medio de un mapa interactivo el cual actualiza por su cuenta la provincia y el cantón.

**Agregar un producto nuevo**

![AportesAgregarProducto](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/b1d5294d-d521-41ee-9b66-d8b9c8688170)

Cuando el producto no existe ya en la base de datos de lo compro, la persona usuaria puede usar el formulario de agregar un producto nuevo al sistema y al finalizar, continuar justo donde estaba en la creación de su aporte.



### 3. Funcionalidad de búsqueda 

#### Búsqueda simple y Búsqueda avanzada
En la página principal y en la página de resultados se pueden realizar tanto las busquedas simples como las busquedas avanzadas.

**Vista en la página principal**

![PaginaPrincipal](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/b7239879-0c9c-4542-a1f1-37e58fe28b7d)

La página principal es lo primero que ve el usuario a la hora de ingresar a la aplicación. En ella se encuentran diversas opciones para realizar busquedas. Primero se puede observar la barra de busqueda donde se ingresa en nombre del producto que el usuario desea buscar. Posteriormente, dentro de este se encuentra el botón para realizar la busqueda. Cuando solo se encuentra el nombre del producto, la busqueda será realizada solamente en terminos de esta. Inferior a estos se encuentra el botón de búsqueda avanzada, el cual al presionarlo despliega un menú de opciones para realizar búsquedas más especificas.

**Vista de la página principal con menú de busqueda avanzada**

![PaginaPrincipalAvanzada](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/53141c68-d037-4e46-93ee-1d531230449b)

De presionar el botón de búsqueda avanzada, se despliega el menu de busqueda avanzada. Dentro de este se pueden realizar busquedas por nombre de producto, nombre de tienda, provincia, canton, precio minimo y precio maximo. Al presionar el boton de buscar, se realizara la busqueda con los parametros ingresados. Siendo esta entrega los que se pueden ver en la imagen adjunta. Un usuario puede elegir solamente una de estos parametros de busqueda o por combinación de estos. Por ejemplo uno puede buscar solamente por marca y sin ingresar un nombre de producto en la barra de busqueda, con lo cual solo se realizará una busqueda por el parametro de marca. 

**Vista de ubicación en menú de búsqueda avanzada**

De querer seleccionar un rango especifico en donde se encuentren productos, se cuenta con un mapa interactivo en el cual seleccionar la zona específica y el radio de kilómetros en donde aplica la búsqueda.

![MapaDeBúsqueda](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/1647d177-b243-4327-8011-dd5edb0d9dad)

**Vista en la página de resultados con menú de búsqueda avanzada**

La busqueda avanzada en la página de resultados se realiza de la misma manera que en la página principal. Es importante recordar que la búsqueda avanzada y los filtros son distintos, pues la primera hacer consultas a los servidores de lo compro, mientras que la segunda solo filtra sobre los resultados ya obtenidos por la persona usuaria.

![ResultadodeBusquedaAvanzada](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/0329a164-7d25-48bb-920a-a613d46ffbf9)


**Vista en la página de resultados**

![ResultadoDeBusqueda](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/06ce5c30-e22e-4c92-9e38-ae2bde689458)

Dentro de la página de resultados, al igual que en la página principal, se encuentra la barra de busqueda y el botón de busqueda avanzada. Las busquedas correspondientes se pueden realizar de la misma manera que en la página principal. Además se muestra la barra de filtros, con ellos la persona usuaria puede organizar y obtener información más especifica de resultados ya obtenidos. 

### 4. Historial de contribuidores, imágenes, calificiones y reportar

![ModalDeRegistros](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/1e746116-53f1-434c-9423-6171e6abcfb9)

Al hacer click sobre un resultado de búsqueda específico se abrira un modal que permite ver todos los aportes que forman ese resultado. Al presionar el botón de perfil en la columna de usuario, a la izquierda del nombre de usuario, se podrá acceder a una página que muestra todas las contribuciones de ese usuario.

![HistorialDeContribuidor](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/63b946d3-133c-4167-98f4-38eca8df278b)

En este mismo modal, la persona usuaria puede votar seleccionando calificación que desea darle al aporte con las estrellas. Las filas se pueden reorganizar ascendente o descendentemente presionando el título de la columna.
Al mismo tiempo el botón bandera permite reportar un aporte que se considera debe ser reportado a moderación. Este botón abre un formulario para describir el problema y enviar el reporte como se muestre a continuación.

![Reportar](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/4d28d3c4-8adb-4491-8d60-e1f20dccc6f2)

Si un producto tiene registros con imágenes, se mostrarán en un carrusel de imágenes las cuales se pueden ver presionando las flechas respectivas

![ModalImagen1](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/b9f51e1e-6e95-4b24-8f67-775ee4982c9d)

![ModalImagen2](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/e2a232d8-2433-4710-9527-9a296b498675)



### 5. Lista de compras y búsqueda en tiendas

Desde el mismo modal que muestra los registros de un resultado, se pueden agregar productos a una lista de compras presionando un botón de marcador.

![PresionarMarcador](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/08677d7c-9e2d-45e7-a3b2-a68b978fff2c)

Al agregarse se mostrará una guía visual temporarl antes de desaparecer.

![PresionadoMarcador](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/97947450-8e21-448f-8db3-2f84ffa75605)

Se pueden agregar todos los productos que se deseen y se pueden acceder desde el botón a la izquierda del de Cerrar Sesión.

![ListaDeCompras](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/aa1f1d34-adce-4ce6-941e-fbd1fccce877)

Presionando el botón de basurero rojo se eliminarán de la lista.

Si se desea ver qué tiendas poseen estos productos y el precio total de comprarlos en tal lugar, se puede presionar el botón de Mostrar por tienda para mostrar un modal con todas las tiendas que tienen al menos un 25% de los productos en ella. Los resultados pueden ser reorganizados ascendente o descendentemente presionando el título de las columnas.

![MostrarTiendas](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/0965c758-2135-40cf-aac6-064b1be56792)


### 6. Moderación

Cuando una persona usuaria cumple con los requisitos de tener más de 10 aportes y una calificación de 4.9 en promedio sobre los aportes, esta es elegida por el sistema para ser moderadora. Esto genera una notificación en la página de mi perfil. Esta notificación es persitente, es decir, aparece cada vez que se entrá al perfil, al menos que se rechace o se acepte. Sin embargo, la persona puede dar click fuera de la notificación y ver su perfil, la notificación volverá a aparecer cuando vuelva a visitar su perfil.

![PerfilPosibleModerador](https://github.com/locompro/ci0128_23b_all_stars/assets/84429050/80f69da1-aea0-4f6e-b2e8-6f937bac39db)

Al aceptar la notificación, la persona tendrá esta interfaz y una nueva acción en su página de perfil para dejar el rol de moderador.

![PerfilModeradores](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/2a65ccfa-c6bf-41e3-b197-a0040485d281)

Con el botón de moderar en la barra de navegación disponible desde cualquier parte de la aplicación las personas moderadores tiene acceso al tablero de moderación donde ver aportes reportados y donde pueden tomar acciones de aceptar el reporte, lo que borra el aporte reportado, o solo moderarlo.

![ModeracionYa](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/29a16cc8-83b0-4a6c-9a26-2a3f607d8730)

De igual forma se pueden ver estadísticas sobre registros cuyos que se salen de la norma al presionar el botón de Precios Anómalos. Estos se revisan automáticamente por el sistema y son agregados conforme se agregan los registros.

![ModeracionPreciosAnomalos](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/3895926c-8e2e-4cfb-a3da-767f82d28688)

Al darle click a cualquier reporte, se mostrarán las opciones de moderación.

![ModeracionAporte](https://github.com/locompro/ci0128_23b_all_stars/assets/112010851/56ef7cb4-4ac0-4f5a-bbb2-d8871335563a)


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

Para preparar la base de datos primero deben hacerse las migraciones. Estas ya estan prepardas dentro del proyecto en la carpeta `Source/Locompro/Migrations.`
Desde la terminal y estando dentro de directorio `Source/Locompro/`.
```
dotnet ef database update
```
o desde la consola del package manager en visual studio:
```
Update-Database
```
Luego de haber hecho esto, el esquema de la base de datos estará listo en la base de datos especificada. Finalmente, para agregar los datos estáticos de la aplicación, se debe ejecutar directamente sobre la base de datos el script de sql static.sql en la carpeta `Source/Locompro/Resources`. Para esto es recomendado conectarse a la base de datos de la aplicación utilizando sql server management studio y ejecutar el script allí. Luego de esto la base de datos estará lista.

### Manual de instalación o ejecución del sistema

Para ejecutar el proyecto desde el IDE Visual Studio Community 2022 o Jetbrains Rider puede presionar el botón de `Run` o `Ejecutar`. También, por medio de la terminal estando dentro de directorio `Source/Locompro/`, usando el siguiente comando.
```
dotnet build
dotnet run --environment <ambiente>
```
Según los parametros actuales de configuración del proyecto, este se ejecutará y estará escuchando en el puerto 7249. Se recomienda utilizar el ambiente "Production", pues ya esta base de datos esta preparada. Cabe resaltar que la aplicación cuando el ambiente es Production pedirá por terminal la hilera de conección a la base de datos de production. 
En caso de usar el ambiente "Development" la aplicación se conecta al servidor sql local.  Para que la aplicación funcione tal y como se espera la base de datos debe esta preparada, con las migraciones aplicadas y con los datos y procedimientos estáticos insertados y creados.


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

     
Por otra parte, para **las pruebas de javascript** necesitara tener instalado node.js y el framework de pruebas unitarias en javascript jest. Para instalar presione el siguiente enlace: [Descargar Node.js](https://nodejs.org/en/download/current) y elija la versión adaptada a su sistema operativo.

Luego asegurese de **instalar las dependencias de jest necesarias**, las cuales puede instalar con los siguientes comandos:

```
npm install --save-dev jest
npm install --save-dev jest-environment-jsdom
npm install --save-dev @babel/core @babel/preset-env babel-jest
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

### Ejecutar pruebas unitarias de javascript

Para ejecutar las pruebas unitarias de javascript, dirijase al folder con todo el javascript del proyecto y abra una terminal. Este folder es, desde la raiz del repositorio:

```
Source/Locompro/wwwroot/js
```

Allí puede ejecutar el siguiente comando en terminal para ver la ejecución de las pruebas unitarias:
```
npm test

```


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
