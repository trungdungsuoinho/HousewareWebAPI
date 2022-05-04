-- Trigger upper Primary Key
GO
DROP TRIGGER upper_ClassificationId;
GO
CREATE TRIGGER upper_ClassificationId
ON Classifications
AFTER INSERT AS
BEGIN
	DECLARE @classificationId NVARCHAR(450);
	SELECT @classificationId = i.ClassificationId FROM inserted i;
	UPDATE Classifications SET ClassificationId = UPPER(@classificationId) WHERE ClassificationId = @classificationId;
END
GO

GO
DROP TRIGGER upper_CategoryId;
GO
CREATE TRIGGER upper_CategoryId
ON Categories
AFTER INSERT AS
BEGIN
	DECLARE @categoryId NVARCHAR(450);
	SELECT @categoryId = i.CategoryId FROM inserted i;
	UPDATE Categories SET CategoryId = UPPER(@categoryId) WHERE CategoryId = @categoryId;
END
GO

GO
DROP TRIGGER upper_ProductId;
GO
CREATE TRIGGER upper_ProductId
ON Products
AFTER INSERT AS
BEGIN
	DECLARE @productId NVARCHAR(450);
	SELECT @productId = i.ProductId FROM inserted i;
	UPDATE Products SET ProductId = UPPER(@productId) WHERE ProductId = @productId;
END
GO

GO
DROP TRIGGER upper_SpecificationId;
GO
CREATE TRIGGER upper_SpecificationId
ON Specifications
AFTER INSERT AS
BEGIN
	DECLARE @specificationId NVARCHAR(450);
	SELECT @specificationId = i.SpecificationId FROM inserted i;
	UPDATE Specifications SET SpecificationId = UPPER(@specificationId) WHERE SpecificationId = @specificationId;
END
GO



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



-- Trigger set value for ModifyDate of Product
GO
DROP TRIGGER set_ModifyDate_Product;
GO
CREATE TRIGGER set_ModifyDate_Product
ON Products
AFTER UPDATE AS
BEGIN
	UPDATE Products SET ModifyDate = (GETDATE() AT TIME ZONE 'N. Central Asia Standard Time')
	WHERE ProductId = (SELECT i.ProductId FROM inserted i);
END
GO



SELECT	*
FROM	sys.messages
WHERE	message_id BETWEEN 13000 AND 49999;