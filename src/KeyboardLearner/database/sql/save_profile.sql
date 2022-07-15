-- Inserts a new profile into the database
INSERT OR REPLACE INTO Profile (
	p_name,
	color,
	create_date,
	lvls_completed,
	wpm,
	fav_lvl,
	group_id
)
VALUES (
	@p_name,
	@color,
	@create_date,
	@lvls_completed,
	@wpm,
	@fav_lvl,
	@group_id
)