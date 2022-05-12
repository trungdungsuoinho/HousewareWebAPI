-- Trigger upper Primary Key
GO
DROP TRIGGER UpperKeysClassInsert;
GO
CREATE TRIGGER UpperKeysClassInsert
ON Classifications
AFTER INSERT AS
BEGIN
	DECLARE @classificationId NVARCHAR(450);
	SELECT @classificationId = i.ClassificationId FROM inserted i;
	UPDATE Classifications SET ClassificationId = UPPER(@classificationId) WHERE ClassificationId = @classificationId;
END
GO

GO
DROP TRIGGER UpperKeysCatInsert;
GO
CREATE TRIGGER UpperKeysCatInsert
ON Categories
AFTER INSERT AS
BEGIN
	DECLARE @categoryId NVARCHAR(450), @classificationId NVARCHAR(450);
	SELECT @categoryId = i.CategoryId, @classificationId = i.ClassificationId FROM inserted i;
	UPDATE Categories SET CategoryId = UPPER(@categoryId) WHERE CategoryId = @categoryId;
END
GO

GO
DROP TRIGGER UpperClassIdCatUpdate;
GO
CREATE TRIGGER UpperClassIdCatUpdate
ON Categories
AFTER UPDATE AS
BEGIN
	DECLARE @categoryId NVARCHAR(450), @classificationId NVARCHAR(450);
	SELECT @categoryId = i.CategoryId, @classificationId = i.ClassificationId FROM inserted i;
	UPDATE Categories SET ClassificationId = UPPER(@classificationId) WHERE CategoryId = @categoryId AND ClassificationId != UPPER(@classificationId) COLLATE SQL_Latin1_General_CP1_CS_AS;
END
GO

GO
DROP TRIGGER UpperKeysProInsert;
GO
CREATE TRIGGER UpperKeysProInsert
ON Products
AFTER INSERT AS
BEGIN
	DECLARE @productId NVARCHAR(450), @categoryId NVARCHAR(450);
	SELECT @productId = i.ProductId, @categoryId = i.CategoryId FROM inserted i;
	UPDATE Products SET ProductId = UPPER(@productId) WHERE ProductId = @productId;
END
GO

GO
DROP TRIGGER UpperCatIdProUpdate;
GO
CREATE TRIGGER UpperCatIdProUpdate
ON Products
AFTER UPDATE AS
BEGIN
	DECLARE @productId NVARCHAR(450), @categoryId NVARCHAR(450);
	SELECT @productId = i.ProductId, @categoryId = i.CategoryId FROM inserted i;
	UPDATE Products SET CategoryId = UPPER(@categoryId) WHERE ProductId = @productId AND CategoryId != UPPER(@categoryId) COLLATE SQL_Latin1_General_CP1_CS_AS;
	UPDATE Products SET ModifyDate = GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'	WHERE ProductId = @productId;
END
GO

GO
DROP TRIGGER UpperKeysSpecInsert;
GO
CREATE TRIGGER UpperKeysSpecInsert
ON Specifications
AFTER INSERT AS
BEGIN
	DECLARE @specificationId NVARCHAR(450);
	SELECT @specificationId = i.SpecificationId FROM inserted i;
	UPDATE Specifications SET SpecificationId = UPPER(@specificationId) WHERE SpecificationId = @specificationId;
END
GO

GO
DROP TRIGGER UpperKeysProSpecInsert;
GO
CREATE TRIGGER UpperKeysProSpecInsert
ON ProductSpecifications
AFTER INSERT AS
BEGIN
	DECLARE @productId NVARCHAR(450), @specificationId NVARCHAR(450);
	SELECT @productId = i.ProductId, @specificationId = i.SpecificationId FROM inserted i;
	UPDATE ProductSpecifications SET ProductId = UPPER(@productId), SpecificationId = UPPER(@specificationId) WHERE ProductId = @productId AND SpecificationId = @specificationId;
END
GO



--GO
--DROP TRIGGER CheckKeyRefProdSpecInsert;
--GO
--CREATE TRIGGER CheckKeyRefProdSpecInsert
--ON ProductSpecifications
--FOR INSERT AS
--BEGIN
--	DECLARE @productId NVARCHAR(450) = NULL, @specificationId NVARCHAR(450) = NULL, @value NVARCHAR(MAX);
--	SELECT @productId = i.ProductId, @specificationId = i.SpecificationId, @value = i.[Value] FROM Products p, Specifications s, inserted i WHERE p.ProductId = i.ProductId AND s.SpecificationId = i.SpecificationId;
--	PRINT('----------------------')
--	PRINT(@productId)
--	PRINT(@specificationId)
--	PRINT('----------------------')
--	IF @productId IS NULL OR @specificationId IS NULL
--		RAISERROR(N'Product %s or specification %s does not exist! ', 16, 0, @productId, @specificationId)
--END
--GO




--INSERT INTO ProductSpecifications VALUES('SHG2703SA', 'CHATLIEU', '');

--SELECT * FROM Products;
--SELECT * FROM ProductSpecifications;
--SELECT * FROM Specifications;
--DELETE Products WHERE 1=1

--- Trigger set false for property enable cascase
GO
DROP TRIGGER enable_Classification;
GO
CREATE TRIGGER enable_Classification
ON Classifications
AFTER UPDATE AS
BEGIN
	DECLARE @classificationId NVARCHAR(450);
	SELECT @classificationId = i.ClassificationId FROM inserted i;
	IF (SELECT i.Enable FROM inserted i) = 0 AND (SELECT d.Enable FROM deleted d) = 1
	BEGIN
		UPDATE Categories SET Enable = 0 WHERE ClassificationId = @classificationId AND Enable = 1;
	END
END
GO

GO
DROP TRIGGER enable_Category;
GO
CREATE TRIGGER enable_Category
ON Categories
AFTER UPDATE AS
BEGIN
	DECLARE @categoryId NVARCHAR(450);
	SELECT @categoryId = i.CategoryId FROM inserted i;
	IF (SELECT i.Enable FROM inserted i) = 0 AND (SELECT d.Enable FROM deleted d) = 1
	BEGIN
		UPDATE Products SET Enable = 0 WHERE CategoryId = @categoryId AND Enable = 1;
	END
END
GO
