SELECT COUNT(*) AS AnzahlStartboote, r.RaceCode, r.Gender, oc.Name AS AK, bc.Name AS BK, rc.Name AS RK
FROM
ReportedRaces r
INNER JOIN ReportedStartboats rsb ON rsb.ReportedRaceId = r.ReportedRaceId
INNER JOIN Oldclasses oc ON oc.OldclassId = r.OldclassId
INNER JOIN Competitions cp ON cp.CompetitionId = r.CompetitionId
INNER JOIN Boatclasses bc ON bc.BoatclassId = cp.BoatclassId
INNER JOIN Raceclasses rc ON rc.RaceclassId = cp.RaceclassId
WHERE r.RegattaId = 2
GROUP BY r.RaceCode, r.Gender, oc.Name, bc.Name, rc.Name
ORDER BY BK