-- Gets all necessary info to calculate the WPM
SELECT bpm, note_cnt, beat_cnt, accuracy
FROM Scores LEFT JOIN Level ON Scores.lvl = Level.title
WHERE profile = @profile
