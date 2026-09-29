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

# Multi-arch build (amd64 + arm64)

task build:multi     # build both platforms, no push
task build:publish   # build both platforms and push IMAGE_TAG (requires docker login)

`task build:build` still builds for the host architecture only.

# CI (GitHub Actions)

- `.github/workflows/ci.yml` - on push to main / PR: `task build:multi` (no push)
- `.github/workflows/release.yml` - on tag `v<IMAGE_TAG>`: CI, then `task build:publish`

Cut a release: bump `IMAGE_TAG` in `build/.env`, commit, then

git tag v0.1.11 && git push origin v0.1.11

The `check-tag` job fails the run if the tag does not match `IMAGE_TAG`.

On GitHub set `DOCKER_USERNAME` as a repository **variable** and
`DOCKERHUB_ACCESSTOKEN_RW` as a repository **secret** (Docker Hub access token, read/write).

# Local testing with act

Needs two gitignored files next to the workflows:

- `.github/workflows/.vars` - `DOCKER_USERNAME=isuperman`
- `.github/workflows/.secrets` - `DOCKERHUB_ACCESSTOKEN_RW=<token>` (only for `act:release`)

task act:lint      # actionlint
task act:ci        # run ci.yml locally
task act:release   # run release.yml locally - pushes a real image!
