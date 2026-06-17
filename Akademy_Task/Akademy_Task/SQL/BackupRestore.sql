USE master;
GO

-- =============================================
-- Бэкап базы данных
-- =============================================
BACKUP DATABASE TrainingDB 
TO DISK = 'C:\Backup\TrainingDB.bak' 
WITH FORMAT, STATS = 10;
GO

-- =============================================
-- Восстановление базы данных
-- =============================================
-- Сначала нужно закрыть все соединения
ALTER DATABASE TrainingDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE TrainingDB 
FROM DISK = 'C:\Backup\TrainingDB.bak' 
WITH REPLACE, STATS = 10;
GO

ALTER DATABASE TrainingDB SET MULTI_USER;
GO