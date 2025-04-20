# Login to docker hub
docker login docker.io

# Open the shell in the 'environment-base' foler

# Build the image defined in the 'app' service
docker compose build app

# Push the image defined in the 'app' service
docker compose push app
