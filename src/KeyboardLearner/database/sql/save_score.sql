-- Inserts a new high score into the database
INSERT OR REPLACE INTO Scores (
	profile,
	lvl,
	score,
	accuracy
)
VALUES (
	@profile,
	@lvl,
	@score,
	@accuracy
)