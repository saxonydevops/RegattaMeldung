SELECT COUNT(DISTINCT(m.MemberId)) AS AnzahlSportler, c.ShortName
  FROM [RegattaMeldung].[dbo].[ReportedStartboats] rsb
  INNER JOIN ReportedStartboatMembers rsbm ON rsbm.ReportedStartboatId = rsb.ReportedStartboatId
  INNER JOIN Members m ON m.MemberId = rsbm.MemberId
  INNER JOIN Clubs c ON rsb.ClubId = c.ClubId
  WHERE rsb.RegattaId = 2 AND m.MemberId NOT IN (1,2,3,4,5,6,7,8)
  GROUP BY c.ShortName