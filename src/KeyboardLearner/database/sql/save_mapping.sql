-- Inserts a new mapping into the database
INSERT OR REPLACE INTO Mapping (
	lvl,
	key,
	qwerty
)
VALUES (
	@lvl,
	@key,
	@qwerty
)