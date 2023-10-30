-- Insert Country
INSERT INTO Countries (Name)
VALUES ('Costa Rica');

-- Insert Provinces for Costa Rica
INSERT INTO Provinces (CountryName, Name)
VALUES ('Costa Rica', 'San José'),
       ('Costa Rica', 'Alajuela'),
       ('Costa Rica', 'Cartago'),
       ('Costa Rica', 'Heredia'),
       ('Costa Rica', 'Guanacaste'),
       ('Costa Rica', 'Puntarenas'),
       ('Costa Rica', 'Limón');

-- Insert Cantons for San José, Costa Rica
INSERT INTO Cantons (CountryName, ProvinceName, Name)
VALUES ('Costa Rica', 'San José', 'San José'),
       ('Costa Rica', 'San José', 'Escazú'),
       ('Costa Rica', 'San José', 'Desamparados'),
       ('Costa Rica', 'San José', 'Puriscal'),
       ('Costa Rica', 'San José', 'Tarrazú'),
       ('Costa Rica', 'San José', 'Aserrí'),
       ('Costa Rica', 'San José', 'Mora'),
       ('Costa Rica', 'San José', 'Goicoechea'),
       ('Costa Rica', 'San José', 'Santa Ana'),
       ('Costa Rica', 'San José', 'Alajuelita'),
       ('Costa Rica', 'San José', 'Vázquez de Coronado'),
       ('Costa Rica', 'San José', 'Acosta'),
       ('Costa Rica', 'San José', 'Tibás'),
       ('Costa Rica', 'San José', 'Moravia'),
       ('Costa Rica', 'San José', 'Montes de Oca'),
       ('Costa Rica', 'San José', 'Turrubares'),
       ('Costa Rica', 'San José', 'Dota'),
       ('Costa Rica', 'San José', 'Curridabat'),
       ('Costa Rica', 'San José', 'Pérez Zeledón'),
       ('Costa Rica', 'San José', 'León Cortés Castro');

-- Insert Cantons for Alajuela, Costa Rica
INSERT INTO Cantons (CountryName, ProvinceName, Name)
VALUES ('Costa Rica', 'Alajuela', 'Alajuela'),
       ('Costa Rica', 'Alajuela', 'San Ramón'),
       ('Costa Rica', 'Alajuela', 'Grecia'),
       ('Costa Rica', 'Alajuela', 'San Mateo'),
       ('Costa Rica', 'Alajuela', 'Atenas'),
       ('Costa Rica', 'Alajuela', 'Naranjo'),
       ('Costa Rica', 'Alajuela', 'Palmares'),
       ('Costa Rica', 'Alajuela', 'Poás'),
       ('Costa Rica', 'Alajuela', 'Orotina'),
       ('Costa Rica', 'Alajuela', 'San Carlos'),
       ('Costa Rica', 'Alajuela', 'Zarcero'),
       ('Costa Rica', 'Alajuela', 'Sarchí'),
       ('Costa Rica', 'Alajuela', 'Upala'),
       ('Costa Rica', 'Alajuela', 'Los Chiles'),
       ('Costa Rica', 'Alajuela', 'Guatuso'),
       ('Costa Rica', 'Alajuela', 'Río Cuarto');

-- Insert Cantons for Heredia, Costa Rica
INSERT INTO Cantons (CountryName, ProvinceName, Name)
VALUES ('Costa Rica', 'Heredia', 'Heredia'),
       ('Costa Rica', 'Heredia', 'Barva'),
       ('Costa Rica', 'Heredia', 'Santo Domingo'),
       ('Costa Rica', 'Heredia', 'Santa Bárbara'),
       ('Costa Rica', 'Heredia', 'San Rafael'),
       ('Costa Rica', 'Heredia', 'San Isidro'),
       ('Costa Rica', 'Heredia', 'Belén'),
       ('Costa Rica', 'Heredia', 'Flores'),
       ('Costa Rica', 'Heredia', 'San Pablo'),
       ('Costa Rica', 'Heredia', 'Sarapiquí');

-- Insert Cantons for Cartago, Costa Rica
INSERT INTO Cantons (CountryName, ProvinceName, Name)
VALUES ('Costa Rica', 'Cartago', 'Cartago'),
       ('Costa Rica', 'Cartago', 'Paraíso'),
       ('Costa Rica', 'Cartago', 'La Unión'),
       ('Costa Rica', 'Cartago', 'Jiménez'),
       ('Costa Rica', 'Cartago', 'Turrialba'),
       ('Costa Rica', 'Cartago', 'Alvarado'),
       ('Costa Rica', 'Cartago', 'Oreamuno'),
       ('Costa Rica', 'Cartago', 'El Guarco');

-- Insert Cantons for Guanacaste, Costa Rica
INSERT INTO Cantons (CountryName, ProvinceName, Name)
VALUES ('Costa Rica', 'Guanacaste', 'Liberia'),
       ('Costa Rica', 'Guanacaste', 'Nicoya'),
       ('Costa Rica', 'Guanacaste', 'Santa Cruz'),
       ('Costa Rica', 'Guanacaste', 'Bagaces'),
       ('Costa Rica', 'Guanacaste', 'Carrillo'),
       ('Costa Rica', 'Guanacaste', 'Cañas'),
       ('Costa Rica', 'Guanacaste', 'Abangares'),
       ('Costa Rica', 'Guanacaste', 'Tilarán'),
       ('Costa Rica', 'Guanacaste', 'Nandayure'),
       ('Costa Rica', 'Guanacaste', 'La Cruz'),
       ('Costa Rica', 'Guanacaste', 'Hojancha');

-- Insert Cantons for Limón, Costa Rica
INSERT INTO Cantons (CountryName, ProvinceName, Name)
VALUES ('Costa Rica', 'Limón', 'Limón'),
       ('Costa Rica', 'Limón', 'Pococí'),
       ('Costa Rica', 'Limón', 'Siquirres'),
       ('Costa Rica', 'Limón', 'Talamanca'),
       ('Costa Rica', 'Limón', 'Matina'),
       ('Costa Rica', 'Limón', 'Guácimo');

-- Insert Cantons for Puntarenas, Costa Rica
INSERT INTO Cantons (CountryName, ProvinceName, Name)
VALUES ('Costa Rica', 'Puntarenas', 'Buenos Aires'),
       ('Costa Rica', 'Puntarenas', 'Corredores'),
       ('Costa Rica', 'Puntarenas', 'Coto Brus'),
       ('Costa Rica', 'Puntarenas', 'Esparza'),
       ('Costa Rica', 'Puntarenas', 'Garabito'),
       ('Costa Rica', 'Puntarenas', 'Golfito'),
       ('Costa Rica', 'Puntarenas', 'Montes de Oro'),
       ('Costa Rica', 'Puntarenas', 'Osa'),
       ('Costa Rica', 'Puntarenas', 'Parrita'),
       ('Costa Rica', 'Puntarenas', 'Puntarenas'),
       ('Costa Rica', 'Puntarenas', 'Quepos'),
       ('Costa Rica', 'Puntarenas', 'Monteverde'),
       ('Costa Rica', 'Puntarenas', 'Puerto Jiménez');

-- Agregar categorías de ejemplo a la tabla Category
INSERT INTO Categories (Name)
VALUES ('Electrónica'),
       ('Ropa'),
       ('Hogar y Jardín'),
       ('Libros'),
       ('Juguetes'),
       ('Salud y Belleza'),
       ('Deportes y Aire Libre'),
       ('Automóviles'),
       ('Alimentos y Comestibles'),
       ('Joyería'),
       ('Muebles'),
       ('Música'),
       ('Mascotas'),
       ('Electrodomésticos'),
       ('Películas y Series de TV'),
       ('Suministros Eléctricos'),
       ('Suministros de Oficina'),
       ('Bebés y Niños'),
       ('Herramientas y Ferretería'),
       ('Fitness'),
       ('Zapatos'),
       ('Viajes'),
       ('Manualidades y Pasatiempos'),
       ('Software'),
       ('Arte y Coleccionables'),
       ('Cámaras y Fotografía'),
       ('Relojes'),
       ('Industrial y Científico'),
       ('Jardinería'),
       ('Suministros para Fiestas');