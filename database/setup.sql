-- init.sql – Initial PostgreSQL setup for MoodCast project

-- Create users table
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password TEXT NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    fullName VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create admin table
CREATE TABLE IF NOT EXISTS admin (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Optional: Insert a sample admin user (replace values as needed)
-- INSERT INTO admin (username, password) VALUES ('admin', 'securepassword');

-- Optional: Insert a sample user (replace values as needed)
-- INSERT INTO users (username, password, email, fullName) 
-- VALUES ('shimon', 'securepassword', 'shimon@example.com', 'Shimon Ifrach');
