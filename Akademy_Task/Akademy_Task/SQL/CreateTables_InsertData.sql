-- Создаем базу данных.
CREATE DATABASE TrainingDB;
GO

-- Переключаемся на неё.
USE TrainingDB;
GO

--В таблицах, которые могут быть затронуты распределённой работой или синхронизацией (Users, Activities, Statistics1, DailyNorm), 
--использован тип UNIQUEIDENTIFIER для первичных ключей. 
--Это позволяет генерировать идентификаторы на клиентской стороне без обращения к серверу и гарантирует глобальную уникальность записей при слиянии данных из разных источников.
-- В справочных таблицах (TrainingPrograms, Exercises) используется INT IDENTITY, так как эти данные создаются только в централизованной системе, 
--не требуют офлайн-генерации и занимают меньше места, что ускоряет работу с ними.


-- 1. Таблица "Программы тренировок".
CREATE TABLE TrainingPrograms (
    ProgramId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,                    
    Type NVARCHAR(50) NOT NULL,                     
    IsActive BIT NOT NULL DEFAULT 1,                
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE() 
);
GO

-- 2. Таблица "Упражнения".
CREATE TABLE Exercises (
    ExerciseId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,                    
    ProgramId INT NOT NULL,                         
    IsActive BIT NOT NULL DEFAULT 1,                
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT FK_Exercise_Program FOREIGN KEY (ProgramId) 
        REFERENCES TrainingPrograms(ProgramId)
);
GO

-- 3. Таблица "Активности".
CREATE TABLE Activities (
    ActivityId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), 
    ExerciseId INT NOT NULL,                         
    ActivityDate DATE NOT NULL,                     
    DurationMinutes INT NOT NULL,                    
    Note NVARCHAR(200) NULL,                         
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT CHK_DurationMinutes CHECK (DurationMinutes > 0 AND DurationMinutes <= 1440),
    CONSTRAINT FK_Activity_Exercise FOREIGN KEY (ExerciseId) 
        REFERENCES Exercises(ExerciseId)
);
GO

-- 4. Таблица "Пользователи".
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), 
    Username NVARCHAR(50) NOT NULL UNIQUE,                
    Email NVARCHAR(100) NOT NULL UNIQUE,                  
    FullName NVARCHAR(100) NOT NULL,                      
    IsActive BIT NOT NULL DEFAULT 1,                     
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()       
);
GO

-- 5. Таблица "Статистика".
CREATE TABLE Statistics1 (
    StatisticId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), 
    UserId UNIQUEIDENTIFIER NOT NULL,
    ExerciseId INT NOT NULL,
    TotalMinutes INT NOT NULL DEFAULT 0,                     
    LastActivityDate DATE NULL,                             
    CONSTRAINT FK_Statistic_User FOREIGN KEY (UserId) 
        REFERENCES Users(UserId),
    CONSTRAINT FK_Statistic_Exercise FOREIGN KEY (ExerciseId) 
        REFERENCES Exercises(ExerciseId)
);
GO

-- 6. Таблица-связка "Тренировка-Пользователь".
CREATE TABLE UserPrograms (
    UserId UNIQUEIDENTIFIER NOT NULL,
    ProgramId INT NOT NULL,
    AssignedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_UserPrograms PRIMARY KEY (UserId, ProgramId),
    CONSTRAINT FK_UserPrograms_User FOREIGN KEY (UserId) 
        REFERENCES Users(UserId),
    CONSTRAINT FK_UserPrograms_Program FOREIGN KEY (ProgramId) 
        REFERENCES TrainingPrograms(ProgramId)
);
GO

-- 7. Таблица "Дневные_нормы".
CREATE TABLE DailyNorm (
    NormId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), 
    UserId UNIQUEIDENTIFIER NOT NULL,
    NormDate DATE NOT NULL,                              
    PlannedMinutes INT NOT NULL DEFAULT 30,              
    ActualMinutes INT NOT NULL DEFAULT 0,                
    CONSTRAINT FK_DailyNorm_User FOREIGN KEY (UserId) 
        REFERENCES Users(UserId)
);
GO

INSERT INTO Users (Username, Email, FullName, IsActive)
VALUES ('oleg', 'oleg@mail.ru', 'Олег Петров', 1);
GO


DECLARE @UserId UNIQUEIDENTIFIER;
SELECT @UserId = UserId FROM Users WHERE Username = 'oleg';
PRINT 'UserId: ' + CAST(@UserId AS VARCHAR(36));
GO

INSERT INTO TrainingPrograms (Name, Type, IsActive) VALUES
('Функциональный тренинг', 'Силовая', 1),
('Утренняя пробежка', 'Кардио', 1),
('Йога для начинающих', 'Растяжка', 1),
('Интенсивный жиросжигатель', 'Кардио', 1),
('Силовая на все группы', 'Силовая', 1);
GO

SELECT* FROM UserPrograms

INSERT INTO Exercises (Name, ProgramId, IsActive) VALUES
('Приседания со штангой', 1, 1),
('Отжимания от пола', 1, 1),
('Подтягивания', 1, 1),
('Берпи', 1, 1);

INSERT INTO Exercises (Name, ProgramId, IsActive) VALUES
('Бег 5 км', 2, 1),
('Бег с интервалами', 2, 1),
('Бег на месте', 2, 1);

INSERT INTO Exercises (Name, ProgramId, IsActive) VALUES
('Комплекс Сурья Намаскар', 3, 1),
('Поза дерева', 3, 1),
('Шавасана', 3, 1);
GO

DECLARE @UserId UNIQUEIDENTIFIER;
SELECT @UserId = UserId FROM Users WHERE Username = 'oleg';
INSERT INTO UserPrograms (UserId, ProgramId)
VALUES 
    (@UserId, 1),
    (@UserId, 2),
    (@UserId, 3);
GO

INSERT INTO Activities (ExerciseId, ActivityDate, DurationMinutes, Note)
VALUES
    (1, '2026-06-17', 20, 'Хорошее самочувствие, вес 75 кг'),
    (1, '2026-06-17', 15, 'Утренняя тренировка'),
    (2, '2026-06-18', 30, 'Пробежка в парке'),
    (3, '2026-06-18', 25, 'Отличная растяжка'),
    (4, '2026-06-19', 40, 'Интенсивная тренировка'),
    (5, '2026-06-19', 10, 'Разминка перед бегом'),
    (1, '2026-06-20', 20, 'Вечерняя тренировка'),
    (2, '2026-06-21', 45, 'Пробежка на выходных');
GO


