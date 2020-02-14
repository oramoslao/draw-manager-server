DELETE FROM "Draws";

UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Draws';

INSERT INTO "Draws" ([Name], [Description], [AllowMultipleParticipations], [ProgrammedFor])
SELECT [Provincia] as [Name], [Provincia] as [Description], 0 as [AllowMultipleParticipations], '2020-02-20' as [ProgrammedFor] FROM "Data" GROUP BY [Provincia];

SELECT * FROM "Draws"