Use Locompro
GO

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
-- Autor: Ariel Antonio Arévalo Alvarado	B50562
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
            WHERE Username = @UpdatedUsername;
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





