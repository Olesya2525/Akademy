USE TrainingDB;
GO


-- 1. Выборка с фильтрацией и сортировкой. Все активности за последнюю неделю, отсортированные по дате
SELECT * FROM Activities 
WHERE ActivityDate >= DATEADD(day, -7, GETDATE())
ORDER BY ActivityDate DESC, DurationMinutes DESC;
GO

-- 2. Удаление и изменение данных. Изменить длительность активности
UPDATE Activities 
SET DurationMinutes = 30 
WHERE ActivityId = (SELECT TOP 1 ActivityId FROM Activities ORDER BY ActivityDate DESC);
GO

-- Удалить активности за 01.06.2026
DELETE FROM Activities 
WHERE ActivityDate = '2026-06-01';
GO


-- 3. Выборка с группировкой.
SELECT 
    e.Name AS Упражнение,
    COUNT(a.ActivityId) AS Количество_тренировок,
    SUM(a.DurationMinutes) AS Всего_минут,
    AVG(a.DurationMinutes) AS Средняя_длительность
FROM Exercises e
LEFT JOIN Activities a ON e.ExerciseId = a.ExerciseId
GROUP BY e.Name
ORDER BY Всего_минут DESC;
GO

-- 4. Левое соединение (LEFT JOIN)
SELECT 
    u.FullName AS Пользователь,
    tp.Name AS Программа,
    e.Name AS Упражнение,
    ISNULL(SUM(a.DurationMinutes), 0) AS Всего_минут
FROM Users u
LEFT JOIN UserPrograms up ON u.UserId = up.UserId
LEFT JOIN TrainingPrograms tp ON up.ProgramId = tp.ProgramId
LEFT JOIN Exercises e ON tp.ProgramId = e.ProgramId
LEFT JOIN Activities a ON e.ExerciseId = a.ExerciseId
GROUP BY u.FullName, tp.Name, e.Name
ORDER BY u.FullName, tp.Name, e.Name;
GO

-- 5. Правое соединение (RIGHT JOIN)
SELECT 
    tp.Name AS Программа,
    e.Name AS Упражнение,
    ISNULL(SUM(a.DurationMinutes), 0) AS Всего_минут
FROM Activities a
RIGHT JOIN Exercises e ON a.ExerciseId = e.ExerciseId
RIGHT JOIN TrainingPrograms tp ON e.ProgramId = tp.ProgramId
GROUP BY tp.Name, e.Name
ORDER BY tp.Name, e.Name;
GO

-- 6. Пересечение (INTERSECT) - упражнения, которые есть и в силовых, и в кардио программах
SELECT Name FROM Exercises WHERE ProgramId IN (SELECT ProgramId FROM TrainingPrograms WHERE Type = 'Силовая')
INTERSECT
SELECT Name FROM Exercises WHERE ProgramId IN (SELECT ProgramId FROM TrainingPrograms WHERE Type = 'Кардио');
GO

-- 7. Итоговая статистика (с GROUP BY + HAVING)
SELECT 
    CONVERT(DATE, ActivityDate) AS Дата,
    SUM(DurationMinutes) AS Сумма_минут,
    CASE 
        WHEN SUM(DurationMinutes) < 30 THEN 'Жёлтый (низкая активность)'
        WHEN SUM(DurationMinutes) BETWEEN 30 AND 90 THEN 'Зелёный (норма)'
        ELSE 'Красный (высокая активность)'
    END AS Стикер
FROM Activities
GROUP BY CONVERT(DATE, ActivityDate)
HAVING SUM(DurationMinutes) > 10
ORDER BY Дата DESC;
GO

