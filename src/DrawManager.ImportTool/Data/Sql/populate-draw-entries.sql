DELETE FROM DrawEntries;

UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='DrawEntries';

INSERT INTO DrawEntries ([DrawId], [EntrantId], [RegisteredOn])
SELECT dr.Id AS [ DrawId ], e.Id AS [ EntrantId ], DATETIME('now')
FROM "Data" AS d
	LEFT JOIN Entrants AS e ON e.Code = d.Identificacion
	LEFT JOIN Draws AS dr ON dr.Name = d.Provincia;