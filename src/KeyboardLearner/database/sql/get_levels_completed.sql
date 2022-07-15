-- Gets the count of levels completed
SELECT COUNT(DISTINCT lvl) AS num FROM Scores
WHERE profile = @profile;