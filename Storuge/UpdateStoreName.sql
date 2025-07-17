-- Script để cập nhật tên cửa hàng từ Tocotoco thành Toocha
UPDATE [Stores]
SET [Name] = REPLACE([Name], 'Tocotoco', 'Toocha')
WHERE [Name] LIKE '%Tocotoco%';

-- Cập nhật cả trường Toco thành Toocha
UPDATE [Stores]
SET [Name] = REPLACE([Name], 'Toco', 'Toocha')
WHERE [Name] LIKE '%Toco %';

-- Hiển thị số lượng bản ghi đã cập nhật
DECLARE @UpdatedCount INT;
SELECT @UpdatedCount = COUNT(*) FROM [Stores] WHERE [Name] LIKE '%Toocha%';
PRINT N'Đã cập nhật ' + CAST(@UpdatedCount AS NVARCHAR(10)) + N' cửa hàng thành công!'; 