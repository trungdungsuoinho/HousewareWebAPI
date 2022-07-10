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
	UPDATE Products SET ModifyDate = GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'	WHERE ProductId = @productId;
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
	UPDATE ProductSpecifications
	SET ProductId = UPPER(i.ProductId), SpecificationId = UPPER(i.SpecificationId)
	FROM inserted i
	WHERE ProductSpecifications.ProductId = i.ProductId AND ProductSpecifications.SpecificationId = i.SpecificationId
END
GO

GO
DROP TRIGGER UpperKeyProCartInsert;
GO
CREATE TRIGGER UpperKeyProCartInsert
ON Carts
AFTER INSERT AS
BEGIN
	UPDATE Carts
	SET ProductId = UPPER(i.ProductId)
	FROM inserted i
	WHERE Carts.ProductId = i.ProductId
END
GO

GO
DROP TRIGGER UpperKeyProStoredInsert;
GO
CREATE TRIGGER UpperKeyProStoredInsert
ON Storeds
AFTER INSERT AS
BEGIN
	UPDATE Storeds
	SET ProductId = UPPER(i.ProductId)
	FROM inserted i
	WHERE Storeds.ProductId = i.ProductId
END
GO

GO
DROP TRIGGER SetModifyDateAddressUpdate;
GO
CREATE TRIGGER SetModifyDateAddressUpdate
ON Addresses
AFTER UPDATE AS
BEGIN
	UPDATE Addresses
	SET ModifyDate = GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'
	FROM inserted i
	WHERE Addresses.AddressId != i.AddressId;
END
GO

--- Trigger set false for property enable cascase
GO
DROP TRIGGER EnableClassificationCascase;
GO
CREATE TRIGGER EnableClassificationCascase
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
DROP TRIGGER EnableCategoryCascase;
GO
CREATE TRIGGER EnableCategoryCascase
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

SELECT AddressId, ModifyDate, CustomerId FROM Addresses Order By ModifyDate Desc
SELECT * FROM Stores
SELECT GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'

SELECT p.ProductId, p.Name, s.SpecificationId, s.Name, ps.Value
FROM Products p
INNER JOIN ProductSpecifications ps ON p.ProductId = ps.ProductId
INNER JOIN Specifications s ON ps.SpecificationId = s.SpecificationId
WHERE s.SpecificationId = 'KICHTHUOC'

SELECT * FROM Addresses

SELECT s.ShopId, a.AddressId
FROM Stores s, Addresses a
WHERE a.CustomerId = '9AFFBCD5-2E26-42D6-7EA6-08DA3BB497FA'

SELECT * FROM Orders WHERE CustomerId = '2a0a27c3-2290-4806-7203-08da3b719d82' AND OrderStatus = 'PAIN';

SELECT TOP 10 * FROM Orders ORDER BY OrderDate
SELECT OrderStatus, COUNT(*) FROM Orders GROUP BY OrderStatus

UPDATE Orders SET OrderStatus = 'PRSS' WHERE OrderStatus = 'PAED'

SELECT * FROM Orders WHERE PaymentType = 'ONL' AND OrderCode iS NOT NULL Order by OrderDate desc;

SELECT * FROM Products Order By modifydate desc