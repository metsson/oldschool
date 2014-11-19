-- #1 Svarstid: 1318s/1175s
SELECT * 
FROM DemoMedlem.dbo.Medlem; 

-- #2 Svarstid: 182s/285s (Jmfr #1)
SELECT ENAMN, FNAMN, POSTNR, ORT
FROM DemoMedlem.dbo.Medlem;

-- #3 Svarstid: 1114s/1152s
SELECT * 
FROM DemoMedlem.dbo.Medlem
ORDER BY ENAMN;

-- #4 Svarstid: 1821s/1202s (Jmfr #1)
SELECT *
FROM DemoMedlem.dbo.Medlem
ORDER BY ENAMN, FNAMN;

-- #5 Svarstid: 335s/262s (Jmfr #2)
SELECT ENAMN, FNAMN, POSTNR, ORT
FROM DemoMedlem.dbo.Medlem
ORDER BY ENAMN;

-- #6 Svarstid: 247s/146s (Jmfr #5)
SELECT ENAMN, FNAMN, POSTNR, ORT
FROM DemoMedlem.dbo.Medlem
ORDER BY ENAMN, FNAMN;

-- #7 Svarstid: 36s/4s
SELECT *
FROM DemoMedlem.dbo.Medlem
WHERE ORT = 'KALMAR';

-- #8 Svarstid: 24s/30s
SELECT *
FROM DemoMedlem.dbo.Medlem
WHERE ORT LIKE '%Kal%';

-- #9 Svarstid: 12s/10s
SELECT *
FROM DemoMedlem.dbo.Medlem
WHERE ORT LIKE 'Kal%' AND ENAMN LIKE '%SON'
ORDER BY Fastnr;

-- #10 Svarstid: 0s/0s
SELECT *
FROM DemoMedlem.dbo.Medlem
WHERE ANNDATUM > 20130101;

-- #11 Svarstid: 2s/1s
SELECT COUNT(*) AS 'Annullerade', LEFT(ANNDATUM, 4) AS 'Årtal', ORT AS 'Ortsnamn'
FROM DemoMedlem.dbo.Medlem
WHERE ANNDATUM > 0
GROUP BY ORT, LEFT(ANNDATUM, 4)
ORDER BY ORT, LEFT(ANNDATUM, 4)

-- #12 Svarstid: 1s/1s
SELECT DISTINCT ORT 
FROM DemoMedlem.dbo.Medlem
ORDER BY ORT;

-- #13 Svarstid: 1454s/2601s(!)
SELECT * 
FROM DemoMedlem.dbo.Medlem
WHERE Telenr1 != Telenr2;