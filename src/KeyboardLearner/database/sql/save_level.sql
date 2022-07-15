-- Inserts a new level into the database
INSERT OR REPLACE INTO Level (
	title,
	difficulty,
	bpm,
	note_cnt,
	beat_cnt,
	rhythm_str,
	pitch_str
)
VALUES (
	@title,
	@difficulty,
	@bpm,
	@note_cnt,
	@beat_cnt,
	@rhythm_str,
	@pitch_str
)