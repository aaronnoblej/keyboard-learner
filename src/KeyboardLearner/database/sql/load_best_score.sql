-- Loads best score for given level
SELECT MAX(score) AS score, accuracy FROM Scores
WHERE profile = @profile AND lvl = @lvl