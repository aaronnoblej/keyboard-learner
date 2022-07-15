-- Gets the most played level for a profile (and the number of plays)
SELECT lvl, MAX(plays) FROM (
	SELECT lvl, COUNT(*) AS plays FROM Scores
	GROUP BY profile, lvl
	HAVING profile = @profile
)