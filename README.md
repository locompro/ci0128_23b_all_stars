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
│           │       <a href="./Design/Mockups/Sprint1/Avance2/mi_perfil_de_usuario.png">mi_perfil_de_usuario.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/un_perfil_de_usuario.png">un_perfil_de_usuario.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/agregar_contribucion.png">agregar_contribucion.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/agregar_producto.png">agregar_producto.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/agregar_tienda.png">agregar_tienda.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/resultados_registros.png">resultados_registros.png</a>  
│           │       <a href="./Design/Mockups/Sprint1/Avance2/resultados_registros_sin_iniciar_sesion.png">resultados_registros_sin_iniciar_sesion.png</a>  
└───Locompro  
    └───(Proyecto ASP.NET)
</pre>
