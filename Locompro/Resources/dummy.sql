-- Agregar categor�as de ejemplo a la tabla Category
INSERT INTO Categories (Name)
VALUES
    ('Electrónica'),
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

-- Adding stores in San José province
INSERT INTO Stores (Name, CantonCountryName, CantonProvinceName, CantonName, Address, Telephone, Status, Latitude, Longitude)
VALUES ('Super San José', 'Costa Rica', 'San José', 'San José', 'Calle Central, Avenida 2', '2256-7890', 1, 9.93, -84.08),
       ('Tienda Escazú', 'Costa Rica', 'San José', 'Escazú', 'Calle Los Laureles', '2228-3456', 1, 9.92, -84.14),
       ('Mercado Moravia', 'Costa Rica', 'San José', 'Moravia', 'Avenida Principal', '2240-5678', 1, 9.96, -84.05),
       ('Comercio Curri', 'Costa Rica', 'San José', 'Curridabat', 'Calle Freses', '2272-8910', 1, 9.91, -84.03),
       ('Bodega Tibás', 'Costa Rica', 'San José', 'Tibás', 'Avenida Central', '2236-2345', 1, 9.95, -84.07),
       ('Almacén Desampa', 'Costa Rica', 'San José', 'Desamparados', 'Calle 5, Avenida 3', '2219-0123', 1, 9.89, -84.09);

-- Adding products
INSERT INTO Products (Name, Model, Brand, Status)
VALUES ('Laptop', 'Inspiron 15', 'Dell', 1),
       ('Celular', 'Galaxy S21', 'Samsung', 1),
       ('Refrigeradora', 'Modelo 3000', 'LG', 1),
       ('Cafetera', 'Express', 'Oster', 1),
       ('Televisor', 'Ultra HD 55"', 'Sony', 1),
       ('Microondas', 'QuickHeat', 'Panasonic', 1);