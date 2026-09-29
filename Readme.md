# Elm Development Environment Builder

[![Elm 0.19.1](https://img.shields.io/badge/Elm-0.19.1-60B5CC?style=flat-square&logo=elm)](https://elm-lang.org/)
[![Node.js 24.11.1](https://img.shields.io/badge/Node.js-24.11.1-339933?style=flat-square&logo=node.js)](https://nodejs.org/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat-square&logo=docker)](https://www.docker.com/)
[![.NET SDK 10.0.100](https://img.shields.io/badge/.NET%20SDK-10.0.100-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)

A containerized development environment for Elm projects, running on Debian 13 slim. This repository provides everything you need to start building Elm applications with a consistent, reproducible environment.

## 🚀 Features

- **Debian 13 slim** base image
- **Elm 0.19.1** pre-installed with common tools:
  - elm-test
  - elm-format
  - elm-watch (beta)
- **Development Tools**:
  - Neovim 0.12.5
  - .NET SDK 10.0.100
  - .NET Runtime 8.0.18 and ASP.NET Core Runtime 9.x
  - Node.js 24.11.1 via nvm
  - Git, SSH, and other essential utilities
  - ZSH with Oh-My-ZSH and Powerlevel10k theme
  - Tmux for terminal productivity
- **Containerized workflow** for consistent development across machines
- **VS Code integration** ready (devcontainer compatible)

## 🏗️ Quick Start

### Prerequisites

- [Docker](https://www.docker.com/get-started) installed on your system
- [Docker Compose](https://docs.docker.com/compose/install/) (usually included with Docker Desktop)
- Git for cloning the repository

### Setup and Run

#### Option 1: Use the pre-built Docker image

You can use the pre-built Docker image directly from Docker Hub. It is published for both `linux/amd64` and `linux/arm64`, so Docker picks the right one for your machine. Use the version from `IMAGE_TAG` in [build/.env](build/.env):

```bash
# Pull the image
docker pull isuperman/elm-devcontainer-foundation:0.1.10

# Run the container
docker run -it --name elm-dev isuperman/elm-devcontainer-foundation:0.1.10 zsh
```

Visit the Docker Hub repository for more information: [https://hub.docker.com/repository/docker/isuperman/elm-devcontainer-foundation/general](https://hub.docker.com/repository/docker/isuperman/elm-devcontainer-foundation/general)

#### Option 2: Build from source

1. Clone this repository:

```bash
git clone https://github.com/yourusername/elm-development-environment-builder.git
cd elm-development-environment-builder
```

1. Build and start the container:

```bash
# Set your Docker Hub username and desired image tag
export DOCKER_USERNAME=yourusername
export REPO_NAME=elm-dev-env
export IMAGE_TAG=latest

# Build the image
cd build
docker compose build

# Run the container
docker compose up -d
```

1. Connect to the running container:

```bash
docker exec -it build-app-1 zsh
```

## 📂 Repository Structure

```
├── .github/workflows/  # CI (build) and release (publish) workflows
├── scripts/ci/         # Helper scripts used by the workflows
├── build/              # Docker container definition
│   ├── docker-compose.yml
│   ├── Dockerfile
│   ├── .env            # Environment variables for Docker Compose
│   └── README.md
├── dotnet/             # .NET project examples and utilities
│   └── test/           # Sample .NET test project
└── elm/                # Elm application source
    ├── elm.json        # Elm project configuration
    └── src/            # Elm source code
        └── Main.elm    # Simple "Hello World" Elm application
```

## 🚢 Releasing a New Image

Images are published to Docker Hub by GitHub Actions as one multi-platform tag (`linux/amd64` + `linux/arm64`). The version is `IMAGE_TAG` in [build/.env](build/.env), and the git tag must match it.

| Workflow | Trigger | What it does |
| --- | --- | --- |
| [ci.yml](.github/workflows/ci.yml) | Push to `main`, pull request, manual | `task build:multi`: builds both platforms, no push |
| [release.yml](.github/workflows/release.yml) | Tag `v<IMAGE_TAG>`, manual | Checks the tag, runs CI, then `task build:publish` |

### Release procedure

Replace `0.1.11` below with the new version.

#### 1. Prepare the change

1. Make the Dockerfile change on a branch and open a pull request. `ci.yml` builds both platforms; wait until it is green.
2. Optionally run the same build locally first:

   ```bash
   task build:multi   # or: task act:ci to run the CI workflow itself
   ```

3. Merge the pull request into `main`.

#### 2. Bump the version

1. Pull `main` and set the new version in `build/.env`:

   ```bash
   git switch main && git pull
   # edit build/.env:  IMAGE_TAG=0.1.11
   ```

2. If the devcontainer in this repo should use the new image, update `FROM` in `.devcontainer/Dockerfile.devmachine` to `isuperman/elm-devcontainer-foundation:0.1.11`. Do this in a separate commit *after* the release, so `main` never points at an image that does not exist yet.
3. Commit and push:

   ```bash
   git commit -am "Release 0.1.11"
   git push
   ```

#### 3. Tag and publish

1. Create and push a tag that matches `IMAGE_TAG` exactly, with a `v` prefix:

   ```bash
   git tag v0.1.11
   git push origin v0.1.11
   ```

2. Follow the run:

   ```bash
   gh run watch
   ```

   `release.yml` runs three jobs in order:

   | Job | What happens | Fails when |
   | --- | --- | --- |
   | `check-tag` | Compares the tag with `IMAGE_TAG` in `build/.env` | The tag is not `v<IMAGE_TAG>` |
   | `ci` | Runs `ci.yml`: builds amd64 + arm64 without pushing | Either platform fails to build |
   | `docker-publish` | Logs in to Docker Hub and runs `task build:publish` | Login fails, or the build or push fails |

   Nothing is pushed unless all three succeed.

#### 4. Verify

```bash
docker buildx imagetools inspect isuperman/elm-devcontainer-foundation:0.1.11
```

The output should list both `linux/amd64` and `linux/arm64`. Then pull and smoke-test it:

```bash
docker run --rm isuperman/elm-devcontainer-foundation:0.1.11 \
  bash -c 'nvim --version | head -1 && elm --version && node --version && dotnet --list-runtimes'
```

#### 5. After the release

- Update `FROM` in `.devcontainer/Dockerfile.devmachine` (see step 2) and rebuild the devcontainer.
- Update the version badges and the feature list at the top of this README if tool versions changed.

#### If something fails

- **`check-tag` failed**: the tag and `IMAGE_TAG` differ. Delete the tag (see [Cleaning up a bad tag](#cleaning-up-a-bad-tag)), fix `build/.env` or the tag, and tag again.
- **`ci` or `docker-publish` failed**: read the first failing step with `gh run view <run-id> --log-failed`. Fix the problem on `main`, move the tag to the fixed commit and push it again:

  ```bash
  git tag -f v0.1.11
  git push -f origin v0.1.11
  ```

- **Transient failure** (network, Docker Hub): re-run the failed jobs with `gh run rerun <run-id> --failed`.

A manual run of `release.yml` (the **Run workflow** button, or `gh workflow run release.yml`) skips the tag check and publishes the current `IMAGE_TAG` from `main`, overwriting it if it already exists. Use it only to re-publish a version.

### One-time setup on GitHub

- Repository **variable** `DOCKER_USERNAME` (e.g. `isuperman`)
- Repository **secret** `DOCKERHUB_ACCESSTOKEN_RW`, a Docker Hub access token with read/write access

### Testing the workflows locally with act

Requires [act](https://github.com/nektos/act) (`brew install act`) and two gitignored files:

- `.github/workflows/.vars` containing `DOCKER_USERNAME=isuperman`
- `.github/workflows/.secrets` containing `DOCKERHUB_ACCESSTOKEN_RW=<token>` (only needed for `act:release`)

```bash
task act:lint      # lint the workflows with actionlint
task act:ci        # run ci.yml locally: builds both platforms, no push
task act:release   # run release.yml locally: PUSHES a real image to Docker Hub
```

### Cleaning up a bad tag

```bash
git push origin :refs/tags/v0.1.11   # remote tag
git tag -d v0.1.11                   # local tag
```

Deleting the git tag does not remove the image from Docker Hub. Delete that under *Tags* in the Docker Hub repository.

## 🧩 Elm Project Development

The repository includes a simple "Hello World" Elm application to get you started. To run the example:

```bash
cd /workspace/elm
elm reactor
```

Then open your browser to `http://localhost:8000` and click on `src/Main.elm` to see the application.

To start a new Elm project from scratch:

```bash
cd /workspace/elm
elm init  # If starting fresh
```

Run your Elm application:

```bash
elm reactor
```

To access the Elm application in your browser:

```bash
# From inside the container
$BROWSER http://localhost:8000
```

The included example demonstrates:

- Basic Elm architecture (Model, Update, View)
- HTML generation with styling
- A welcoming "Hello World" interface

## 🔧 Customization

### Adding Additional Elm Packages

To add packages to your Elm project:

```bash
cd /workspace/elm
elm install author/package-name
```

### Customizing the Docker Container

Edit the `build/Dockerfile` to add or modify tools and dependencies based on your needs. Then rebuild the image using the docker compose command shown above.

## 💡 Tips for Working with This Environment

- The container user has sudo privileges for installing additional packages
- Use `elm-watch` for hot reloading during development
- The Elm compiler and tools are available globally in the PATH
- Dotnet tools can be accessed through the `dotnet` command
- If a global dotnet tool requires .NET 8 (for example `sq`), the image includes `Microsoft.NETCore.App 8.0.18` at `/opt/dotnet`

## 🔒 Security Notes

- The container runs with a non-root user `container-user`
- SSH keys are not included and should be mounted as needed
- Container networking is managed through Docker Compose

## 📚 Additional Resources

- [Elm Language Documentation](https://guide.elm-lang.org/)
- [Elm Package Catalog](https://package.elm-lang.org/)
- [Debian Documentation](https://www.debian.org/doc/)
- [Docker Documentation](https://docs.docker.com/)

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## ✨ About This Documentation

This documentation was created entirely by GitHub Copilot, demonstrating the capabilities of AI assistance in software development documentation. All content has been (no so) carefully reviewed by a human to ensure accuracy and completeness.
