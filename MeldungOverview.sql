
SELECT COUNT(*) AS AnzahlBoote, c.ShortName
  FROM [RegattaMeldung].[dbo].[ReportedStartboats] rsb
  INNER JOIN Clubs c ON rsb.ClubId = c.ClubId
  WHERE rsb.RegattaId = 2
  GROUP BY c.ShortName