DELETE FROM "Draws";

UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Draws';

INSERT INTO "Draws" ([Name], [Description], [AllowMultipleParticipations], [ProgrammedFor], [GroupName])
SELECT [Provincia] as [Name], [Provincia] as [Description], 0 as [AllowMultipleParticipations], '2020-02-20' as [ProgrammedFor], 'SORTEO_2020' as [GroupName] 
FROM "Data" GROUP BY [Provincia];

SELECT * FROM "Draws"