# Login to docker hub

docker login docker.io

# Open the shell in the build folder

cd build

# Build the image defined in the 'app' service

docker compose build app

# Push the image defined in the 'app' service

docker compose push app

# Alternative from project root without changing directory

docker compose -f build/docker-compose.yml build app
docker compose -f build/docker-compose.yml push app
