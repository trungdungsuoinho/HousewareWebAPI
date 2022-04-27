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

