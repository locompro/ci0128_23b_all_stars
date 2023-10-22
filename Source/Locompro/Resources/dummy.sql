-- Inserting User
INSERT INTO dbo.AspNetUsers
(Id, Name, Address, Rating, Status, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES
    ('1', 'Juan Perez', 'San Jose, Costa Rica', 4.5, 1, 'juan.perez@email.com', 'JUAN.PEREZ@EMAIL.COM', 'juan.perez@email.com', 'JUAN.PEREZ@EMAIL.COM', 1, 'hashed_password', 'security_stamp', 'concurrency_stamp', '12345678', 1, 0, 1, 0),
    ('2', 'Maria Fernandez', 'Heredia, Costa Rica', 4.7, 1, 'maria.fernandez@email.com', 'MARIA.FERNANDEZ@EMAIL.COM', 'maria.fernandez@email.com', 'MARIA.FERNANDEZ@EMAIL.COM', 1, 'hashed_password2', 'security_stamp2', 'concurrency_stamp2', '23456789', 1, 0, 1, 0),
    ('3', 'Carlos Rodriguez', 'Alajuela, Costa Rica', 4.6, 1, 'carlos.rodriguez@email.com', 'CARLOS.RODRIGUEZ@EMAIL.COM', 'carlos.rodriguez@email.com', 'CARLOS.RODRIGUEZ@EMAIL.COM', 1, 'hashed_password3', 'security_stamp3', 'concurrency_stamp3', '34567890', 1, 0, 1, 0),
    ('4', 'Sofia Vargas', 'Cartago, Costa Rica', 4.8, 1, 'sofia.vargas@email.com', 'SOFIA.VARGAS@EMAIL.COM', 'sofia.vargas@email.com', 'SOFIA.VARGAS@EMAIL.COM', 1, 'hashed_password4', 'security_stamp4', 'concurrency_stamp4', '45678901', 1, 0, 1, 0);

-- Agregar categorías de ejemplo a la tabla Category
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
       ('Almacén Desampa', 'Costa Rica', 'San José', 'Desamparados', 'Calle 5, Avenida 3', '2219-0123', 1, 9.89, -84.09),
       ('Super Heredia', 'Costa Rica', 'Heredia', 'Heredia', 'Avenida Principal', '2267-8912', 1, 10.00, -84.12),
       ('Tienda Alajuela', 'Costa Rica', 'Alajuela', 'Alajuela', 'Calle Central', '2440-1234', 1, 10.02, -84.20),
       ('Bazar Cartago', 'Costa Rica', 'Cartago', 'Cartago', 'Avenida 3, Calle 2', '2550-5678', 1, 9.87, -83.92),
       ('Mercado Limón', 'Costa Rica', 'Limón', 'Limón', 'Calle 5', '2758-9012', 1, 10.00, -83.03),
       ('Almacén Puntarenas', 'Costa Rica', 'Puntarenas', 'Puntarenas', 'Avenida Paseo de los Turistas', '2661-2345', 1, 9.98, -84.83);


-- Adding products
INSERT INTO Products (Name, Model, Brand, Status)
VALUES ('Laptop', 'Inspiron 15', 'Dell', 1),
       ('Celular', 'Galaxy S21', 'Samsung', 1),
       ('Refrigeradora', 'Modelo 3000', 'LG', 1),
       ('Cafetera', 'Express', 'Oster', 1),
       ('Televisor', 'Ultra HD 55"', 'Sony', 1),
       ('Microondas', 'QuickHeat', 'Panasonic', 1),
       ('Bicicleta', 'Mountain 200', 'Trek', 1),
       ('Consola', 'PlayStation 5', 'Sony', 1),
       ('Tablet', 'Tab S7', 'Samsung', 1),
       ('Afeitadora', 'Series 9', 'Braun', 1),
       ('Lavadora', 'TurboDrum', 'LG', 1),
       ('Cámara', 'EOS 2000D', 'Canon', 1),
       ('Cámara', 'EOS 2000D', null, 1),
       ('Laptop', null, 'Toshiba', 1);

DECLARE @userId int = 1;
DECLARE @productId int = 1;
-- using time offsets to create fake unique timestamps
DECLARE @userTimeOffset int = 0; -- Time offset for each user
DECLARE @productTimeOffset int = 0; -- Time offset for each product

-- Looping through users
WHILE @userId <= 4
BEGIN
    -- Resetting product ID and product time offset for each user
    SET @productId = 1;
    SET @productTimeOffset = 0;

    -- Loop through products
    WHILE @productId <= 14
BEGIN
        -- Calculate the total time offset
        DECLARE @totalTimeOffset int = @userTimeOffset + @productTimeOffset;

        -- Inserting submissions for each user and product
INSERT INTO dbo.Submissions (Username, EntryTime, Status, Price, Rating, Description, StoreName, ProductId)
VALUES
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -@totalTimeOffset, GETDATE()), 1, 300000, 4.5, 'Excelente producto', 'Super San José', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 100), GETDATE()), 1, 310000, 4.7, 'Muy buena calidad', 'Tienda Escazú', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 200), GETDATE()), 1, 305000, 4.6, 'Recomendado', 'Mercado Moravia', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 300), GETDATE()), 1, 312000, 4.8, 'Funciona perfectamente', 'Comercio Curri', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 400), GETDATE()), 1, 299000, 4.4, 'Buen precio', 'Bodega Tibás', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 500), GETDATE()), 1, 308000, 4.9, 'Excelente oferta', 'Almacén Desampa', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 600), GETDATE()), 1, 300000, 4.5, 'Excelente producto', 'Super Heredia', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 700), GETDATE()), 1, 310000, 4.7, 'Muy buena calidad', 'Tienda Alajuela', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 800), GETDATE()), 1, 305000, 4.6, 'Recomendado', 'Bazar Cartago', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 900), GETDATE()), 1, 312000, 4.8, 'Funciona perfectamente', 'Mercado Limón', @productId),
    (CAST(@userId AS nvarchar(450)), DATEADD(MILLISECOND, -(@totalTimeOffset + 1000), GETDATE()), 1, 299000, 4.4, 'Buen precio', 'Almacén Puntarenas', @productId);

-- Increment product ID and product time offset for unique timestamps
SET @productId = @productId + 1;
        SET @productTimeOffset = @productTimeOffset + 1100; -- Increment the product offset by 1.1 seconds
END

    -- Increment user ID and user time offset for the next loop iteration
    SET @userId = @userId + 1;
    SET @userTimeOffset = @userTimeOffset + 60000; -- Increment the user offset by 60 seconds
END;