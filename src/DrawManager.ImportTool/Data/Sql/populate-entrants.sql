DELETE FROM "Entrants";

UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Entrants';


INSERT INTO "Entrants" ([Code], [Name], [City])
SELECT [Identificacion] ,[Nombre] ,[Provincia] FROM "Data" GROUP BY [Identificacion], [Nombre], [Provincia];


SELECT * FROM "Entrants";