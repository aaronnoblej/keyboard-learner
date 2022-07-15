-- Deletes a profile and the associated scores from the database
DELETE FROM Profile
WHERE p_name = @p_name;

DELETE FROM Scores
WHERE profile = @p_name;