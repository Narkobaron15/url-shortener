# Use the official Bun image as the base image
FROM oven/bun:latest

# Set the working directory
WORKDIR /app

# Copy the package.json and package-lock.json or yarn.lock files
COPY package.json ./
# COPY bun.lockb ./

# Install dependencies
RUN bun install

# Copy the rest of the application code
COPY . .

# Build the application
RUN bun vite build

EXPOSE 80

# Command to start the Vite preview
CMD ["bun", "run", "preview"]
