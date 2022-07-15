-- Creates tables the KeyboardLearner
CREATE TABLE Level (
	title VARCHAR(255) PRIMARY KEY,
	difficulty INT,
	bpm INT,
	note_cnt INT,
	beat_cnt NUMERIC,
	rhythm_str VARCHAR(1024),
	pitch_str VARCHAR(1024)
);
CREATE TABLE Profile (
	p_name VARCHAR(255) PRIMARY KEY,
	color CHAR(6) NOT NULL,
	create_date DATE,
	lvls_completed INT,
	wpm INT,
	fav_lvl VARCHAR(255),
	group_id INT DEFAULT 0,
	FOREIGN KEY(fav_lvl) REFERENCES Level(title),
	FOREIGN KEY(role) REFERENCES Group(group_id),
	ON DELETE CASCADE
);
CREATE TABLE Scores (
	profile VARCHAR(255),
	lvl VARCHAR(255),
	score INT,
	accuracy NUMERIC,
	FOREIGN KEY(profile) REFERENCES Profile(p_name),
	FOREIGN KEY(lvl) REFERENCES Level(title)
);
CREATE TABLE Mapping (
	lvl VARCHAR(255),
	key CHAR(2),
	qwerty CHAR NOT NULL,
	FOREIGN KEY(lvl) REFERENCES Level(title),
	FOREIGN KEY(key) REFERENCES Key(note),
	PRIMARY KEY (lvl, key)
);
CREATE TABLE Key (
	note CHAR(2) PRIMARY KEY,
	filename VARCHAR(255)
);
CREATE TABLE Role (
	group_id INT PRIMARY KEY,
	name VARCHAR(255)
);