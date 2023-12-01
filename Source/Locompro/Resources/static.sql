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

GO

INSERT INTO dbo.AspNetUsers
(Id, Name, Address, Rating, Status, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
 SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled,
 AccessFailedCount)
VALUES ('Anomaly_Service', 'Anomaly Detection Service', 'NotAplicable', 0, 0, 'Anomaly Detection Service',
        'ANOMALY DETECTION SERVICE',
        'Locompro@email.com', 'LOCOMPRO@EMAIL.COM', 1, 'hashed_password', 'security_stamp', 'concurrency_stamp',
        '12345678', 1, 0, 1, 0)

-- AS-14
-- Procedimiento para agregar toda la jerarqu�a de padres para la categor�a que trae un Product al ser insertado.
-- Autor: Brandon Alonso Mora Umaña	C15179
GO
CREATE OR ALTER PROCEDURE AddParents @category NVARCHAR(60), @productId int
AS
WITH ascendants AS (SELECT c.Name, c.ParentName
                    FROM Categories c
                    WHERE @category = c.Name
                    UNION ALL

                    SELECT Categories.Name, Categories.ParentName
                    FROM Categories
                             INNER JOIN ascendants ON Categories.Name = ascendants.ParentName)

INSERT
INTO CategoryProduct
SELECT c.Name, @productId
FROM Categories c
GO

-- AS-76
-- Función que retorna todos los IDs de usuarios que son elegibles
-- para asumir el rol de moderador.
-- Autor: Gabriel Molina Bulgarelli C14826
GO
CREATE OR ALTER FUNCTION GetQualifiedUserIDs(
)
    RETURNS TABLE
        AS
        RETURN
            (
                SELECT au.Id
                FROM AspNetUsers au
                         Left Join AspNetUserClaims ac on au.Id = ac.UserId
                WHERE (SELECT COUNT(*)
                       FROM Submissions s
                       WHERE s.UserId = au.Id)
                    > 10
                  AND au.Rating >= 4.9
            );
GO

--- AS-279 
-- Procedimiento que elimina todos los registros con el status de ser eliminados
-- luego de ser aprobados por un moderador
-- Autor: Ariel Antonio Arévalo Alvarado B50562
CREATE OR ALTER PROCEDURE DeleteModeratedSubmissions
AS
BEGIN
    DELETE
    FROM Submissions
    WHERE Status = 4;
END;

GO

-- AS-138
-- Función para obtener el número exacto de contribuciones hechas por
-- la persona usuaria especificada (que hayan sido calificadas)
-- Autor: A. Badilla Oliva b80874
CREATE OR ALTER FUNCTION CountRatedSubmissions(@UserID NVARCHAR(450))
    RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    SELECT @Count = COUNT(*)
    FROM Submissions
    WHERE UserID = @UserID
      AND Rating > 0;
    RETURN @Count;
END;
GO

-- AS-75 
-- Trigger que actualiza el rating de un usuario cuando se actualiza el rating de uno
-- de sus contribuciones (submissions)
-- Autor: Gabriel Molina Bulgarelli C14826
GO
CREATE OR ALTER TRIGGER UpdateUserRating
    ON Submissions
    AFTER UPDATE
    AS
BEGIN
    DECLARE @UpdatedUsername NVARCHAR(255);
    DECLARE @NewRating DECIMAL(5, 2);

    IF UPDATE(Rating)
        BEGIN
            SELECT @UpdatedUsername = i.UserId
            FROM inserted i;

            SELECT @NewRating = AVG(s.Rating)
            FROM Submissions s
            WHERE s.UserId = @UpdatedUsername;

            UPDATE AspNetUsers
            SET Rating = @NewRating
            WHERE Id = @UpdatedUsername;
        END
END;
GO
-- Obtiene una cantidad dada de imagenes tomadas de los mejores submissions de un producto
-- Autor: Joseph Stuart Valverde Kong	C18100
CREATE OR ALTER FUNCTION GetPictures(
    @StoreName nvarchar(60),
    @ProductId int,
    @MaxPictures int
)
    RETURNS TABLE
        AS
        RETURN
            (
                WITH ItemPictures AS
                         (SELECT S.UserId                                      AS SubmissionUserId,
                                 S.EntryTime                                   AS SubmissionEntryTime,
                                 P.PictureTitle,
                                 P.PictureData,
                                 ROW_NUMBER() OVER (ORDER BY S.EntryTime DESC) AS RowNum
                          FROM Submissions AS S
                                   JOIN Pictures AS P ON
                                      S.UserId = P.SubmissionUserId
                                  AND S.EntryTime = P.SubmissionEntryTime
                          WHERE S.StoreName = @StoreName
                            AND S.ProductId = @ProductId)
                SELECT PictureTitle, PictureData
                FROM ItemPictures
                WHERE RowNum <= @MaxPictures
            );

GO