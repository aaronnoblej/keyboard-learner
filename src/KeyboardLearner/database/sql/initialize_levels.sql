-- Initializes all levels into the database so that they are loadable.
INSERT INTO Level (
	title
	difficulty,
	bpm,
	note_cnt,
	beat_cnt,
	rhythm_str,
	pitch_str
)
VALUES
('Fur Elise', 2,160,??,??,'888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888884', 'e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,g#4,b4,c5,e3,a3,e4,e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,c5,b4,a4,e3,a4,e4,e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,g#4,b4,c5,e3,a3,e4,e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,c5,b4,a4,e3,a3,b4,c5,d5,e5,g3,c4,g4,f5,e5,d5,g3,b3,f4,e5,d5,c5,e3,a3,e4,d5,c5,b4,e3,e4,c5,b4,a4')

