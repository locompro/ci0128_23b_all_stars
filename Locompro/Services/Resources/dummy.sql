-- Agregar categor�as de ejemplo a la tabla Category
INSERT INTO Category (Name)
VALUES
    ('Electr�nica'),
    ('Ropa'),
    ('Hogar y Jard�n'),
    ('Libros'),
    ('Juguetes'),
    ('Salud y Belleza'),
    ('Deportes y Aire Libre'),
    ('Autom�viles'),
    ('Alimentos y Comestibles'),
    ('Joyer�a'),
    ('Muebles'),
    ('M�sica'),
    ('Mascotas'),
    ('Electrodom�sticos'),
    ('Pel�culas y Series de TV'),
    ('Suministros El�ctricos'),
    ('Suministros de Oficina'),
    ('Beb�s y Ni�os'),
    ('Herramientas y Ferreter�a'),
    ('Fitness'),
    ('Zapatos'),
    ('Viajes'),
    ('Manualidades y Pasatiempos'),
    ('Software'),
    ('Arte y Coleccionables'),
    ('C�maras y Fotograf�a'),
    ('Relojes'),
    ('Industrial y Cient�fico'),
    ('Jardiner�a'),
    ('Suministros para Fiestas');


    -- Insert items into the Item table
INSERT INTO Item (DateAdded, Name, Price, StoreName, Canton, Address, Description)
VALUES
    ('2020-10-10', 'sombrero', 1000, 'sombrereria', 'San Jose', 'San Jose', 'sombrero de paja'),
    ('2020-10-10', 'zapatos', 1000, 'zapateria', 'San Jose', 'San Jose', 'zapatos de cuero'),
    ('2020-10-10', 'camisas', 1000, 'camiseria', 'San Jose', 'San Jose', 'camisas de algodon'),
    ('2020-10-10', 'pantalones', 1000, 'pantaloneria', 'San Jose', 'San Jose', 'pantalones de mezclilla'),
    ('2020-10-10', 'sombrero', 1000, 'sombrereria', 'San Jose', 'San Jose', 'sombrero de paja'),
    ('2020-10-10', 'zapatos', 1000, 'zapateria', 'San Jose', 'San Jose', 'zapatos de cuero'),
    ('2021-03-15', 'camisa', 500, 'tienda de ropa', 'Heredia', 'Heredia', 'camisa de algod�n'),
    ('2021-02-28', 'televisor', 2000, 'electrodom�sticos', 'Cartago', 'Cartago', 'televisor LED de 55 pulgadas'),
    ('2021-01-05', 'laptop', 1200, 'tecnolog�a', 'San Jose', 'San Jose', 'laptop ultrabook'),
    ('2021-04-20', 'bicicleta', 350, 'deportes', 'Alajuela', 'Alajuela', 'bicicleta de monta�a'),
    ('2021-06-10', 'refrigeradora', 900, 'electrodom�sticos', 'Heredia', 'Heredia', 'refrigeradora de acero inoxidable'),
    ('2021-08-22', 'reloj', 300, 'joyer�a', 'Puntarenas', 'Puntarenas', 'reloj de pulsera'),
    ('2020-11-17', 'mueble de sal�n', 750, 'muebles', 'San Jose', 'San Jose', 'mueble de sal�n moderno'),
    ('2021-09-02', 'tel�fono m�vil', 800, 'tecnolog�a', 'Cartago', 'Cartago', 'tel�fono m�vil Android'),
    ('2021-07-30', 'c�mara DSLR', 1100, 'electr�nica', 'Alajuela', 'Alajuela', 'c�mara r�flex digital'),
    ('2020-12-05', 'tabla de surf', 350, 'deportes acu�ticos', 'Puntarenas', 'Puntarenas', 'tabla de surf para principiantes'),
    ('2021-04-05', 'silla de oficina', 150, 'muebles', 'Heredia', 'Heredia', 'silla ergon�mica para oficina'),
    ('2021-03-12', 'caf� gourmet', 12, 'cafeter�a', 'San Jose', 'San Jose', 'caf� molido de alta calidad'),
    ('2021-02-14', 'guitarra ac�stica', 300, 'instrumentos musicales', 'Cartago', 'Cartago', 'guitarra ac�stica de concierto'),
    ('2021-08-28', 'juego de mesa', 25, 'juguetes', 'Alajuela', 'Alajuela', 'juego de mesa familiar'),
    ('2021-07-01', 'ca�a de pescar', 40, 'deportes', 'Puntarenas', 'Puntarenas', 'ca�a de pescar telesc�pica'),
    ('2021-06-15', 'batidora', 60, 'electrodom�sticos', 'Heredia', 'Heredia', 'batidora de mano'),
    ('2021-09-10', 'silla de playa', 25, 'muebles de exterior', 'San Jose', 'San Jose', 'silla plegable para la playa'),
    ('2020-11-30', 'tenis deportivos', 75, 'tienda de deportes', 'Cartago', 'Cartago', 'tenis deportivos para correr');

