-- init.sql – Initial PostgreSQL setup for MoodCast project

-- Create users table
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password TEXT NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    fullName VARCHAR(100),
    is_verified BOOLEAN DEFAULT FALSE,
     counter INT DEFAULT 5,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create admin table
CREATE TABLE IF NOT EXISTS admin (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE IF NOT EXISTS user_verifications (
                    id SERIAL PRIMARY KEY,
                    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                    code VARCHAR(10) NOT NULL,
                    expires_at TIMESTAMP NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )
DO $$ 
BEGIN 
    -- הוספת is_verified אם חסר
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                    WHERE table_name='users' AND column_name='is_verified') THEN
        ALTER TABLE users ADD COLUMN is_verified BOOLEAN DEFAULT FALSE;
    END IF;

    -- הוספת counter אם חסר (עם ערך ברירת מחדל 5)
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                    WHERE table_name='users' AND column_name='counter') THEN
        ALTER TABLE users ADD COLUMN counter INT DEFAULT 5;
    END IF;
END $$;

-- Optional: Insert a sample admin user (replace values as needed)
-- INSERT INTO admin (username, password) VALUES ('admin', 'securepassword');

-- Optional: Insert a sample user (replace values as needed)
-- INSERT INTO users (username, password, email, fullName) 
-- VALUES ('shimon', 'securepassword', 'shimon@example.com', 'Shimon Ifrach');
